// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
import { AbortController } from "./AbortController";
import { HttpError, TimeoutError } from "./Errors";
import { LogLevel } from "./ILogger";
import { TransferFormat } from "./ITransport";
import { Arg, getDataDetail, sendMessage } from "./Utils";
const SHUTDOWN_TIMEOUT = 5 * 1000;
// Not exported from 'index', this type is internal.
export class LongPollingTransport {
    constructor(httpClient, accessTokenFactory, logger, logMessageContent, shutdownTimeout) {
        this.httpClient = httpClient;
        this.accessTokenFactory = accessTokenFactory || (() => null);
        this.logger = logger;
        this.pollAbort = new AbortController();
        this.logMessageContent = logMessageContent;
        this.shutdownTimeout = shutdownTimeout || SHUTDOWN_TIMEOUT;
    }
    // This is an internal type, not exported from 'index' so this is really just internal.
    get pollAborted() {
        return this.pollAbort.aborted;
    }
    async connect(url, transferFormat) {
        Arg.isRequired(url, "url");
        Arg.isRequired(transferFormat, "transferFormat");
        Arg.isIn(transferFormat, TransferFormat, "transferFormat");
        this.url = url;
        this.logger.log(LogLevel.Trace, "(LongPolling transport) Connecting");
        if (transferFormat === TransferFormat.Binary && (typeof new XMLHttpRequest().responseType !== "string")) {
            // This will work if we fix: https://github.com/aspnet/SignalR/issues/742
            throw new Error("Binary protocols over XmlHttpRequest not implementing advanced features are not supported.");
        }
        const pollOptions = {
            abortSignal: this.pollAbort.signal,
            headers: {},
            timeout: 90000,
        };
        if (transferFormat === TransferFormat.Binary) {
            pollOptions.responseType = "arraybuffer";
        }
        const token = await this.accessTokenFactory();
        this.updateHeaderToken(pollOptions, token);
        let closeError;
        // Make initial long polling request
        // Server uses first long polling request to finish initializing connection and it returns without data
        const pollUrl = `${url}&_=${Date.now()}`;
        this.logger.log(LogLevel.Trace, `(LongPolling transport) polling: ${pollUrl}`);
        const response = await this.httpClient.get(pollUrl, pollOptions);
        if (response.statusCode !== 200) {
            this.logger.log(LogLevel.Error, `(LongPolling transport) Unexpected response code: ${response.statusCode}`);
            // Mark running as false so that the poll immediately ends and runs the close logic
            closeError = new HttpError(response.statusText, response.statusCode);
            this.running = false;
        }
        else {
            this.running = true;
        }
        this.poll(this.url, pollOptions, closeError);
        return Promise.resolve();
    }
    updateHeaderToken(request, token) {
        if (token) {
            // tslint:disable-next-line:no-string-literal
            request.headers["Authorization"] = `Bearer ${token}`;
            return;
        }
        // tslint:disable-next-line:no-string-literal
        if (request.headers["Authorization"]) {
            // tslint:disable-next-line:no-string-literal
            delete request.headers["Authorization"];
        }
    }
    async poll(url, pollOptions, closeError) {
        try {
            while (this.running) {
                // We have to get the access token on each poll, in case it changes
                const token = await this.accessTokenFactory();
                this.updateHeaderToken(pollOptions, token);
                try {
                    const pollUrl = `${url}&_=${Date.now()}`;
                    this.logger.log(LogLevel.Trace, `(LongPolling transport) polling: ${pollUrl}`);
                    const response = await this.httpClient.get(pollUrl, pollOptions);
                    if (response.statusCode === 204) {
                        this.logger.log(LogLevel.Information, "(LongPolling transport) Poll terminated by server");
                        this.running = false;
                    }
                    else if (response.statusCode !== 200) {
                        this.logger.log(LogLevel.Error, `(LongPolling transport) Unexpected response code: ${response.statusCode}`);
                        // Unexpected status code
                        closeError = new HttpError(response.statusText, response.statusCode);
                        this.running = false;
                    }
                    else {
                        // Process the response
                        if (response.content) {
                            this.logger.log(LogLevel.Trace, `(LongPolling transport) data received. ${getDataDetail(response.content, this.logMessageContent)}`);
                            if (this.onreceive) {
                                this.onreceive(response.content);
                            }
                        }
                        else {
                            // This is another way timeout manifest.
                            this.logger.log(LogLevel.Trace, "(LongPolling transport) Poll timed out, reissuing.");
                        }
                    }
                }
                catch (e) {
                    if (!this.running) {
                        // Log but disregard errors that occur after we were stopped by DELETE
                        this.logger.log(LogLevel.Trace, `(LongPolling transport) Poll errored after shutdown: ${e.message}`);
                    }
                    else {
                        if (e instanceof TimeoutError) {
                            // Ignore timeouts and reissue the poll.
                            this.logger.log(LogLevel.Trace, "(LongPolling transport) Poll timed out, reissuing.");
                        }
                        else {
                            // Close the connection with the error as the result.
                            closeError = e;
                            this.running = false;
                        }
                    }
                }
            }
        }
        finally {
            // Indicate that we've stopped so the shutdown timer doesn't get registered.
            this.stopped = true;
            // Clean up the shutdown timer if it was registered
            if (this.shutdownTimer) {
                clearTimeout(this.shutdownTimer);
            }
            // Fire our onclosed event
            if (this.onclose) {
                this.logger.log(LogLevel.Trace, `(LongPolling transport) Firing onclose event. Error: ${closeError || "<undefined>"}`);
                this.onclose(closeError);
            }
            this.logger.log(LogLevel.Trace, "(LongPolling transport) Transport finished.");
        }
    }
    async send(data) {
        if (!this.running) {
            return Promise.reject(new Error("Cannot send until the transport is connected"));
        }
        return sendMessage(this.logger, "LongPolling", this.httpClient, this.url, this.accessTokenFactory, data, this.logMessageContent);
    }
    async stop() {
        // Send a DELETE request to stop the poll
        try {
            this.running = false;
            this.logger.log(LogLevel.Trace, `(LongPolling transport) sending DELETE request to ${this.url}.`);
            const deleteOptions = {
                headers: {},
            };
            const token = await this.accessTokenFactory();
            this.updateHeaderToken(deleteOptions, token);
            const response = await this.httpClient.delete(this.url, deleteOptions);
            this.logger.log(LogLevel.Trace, "(LongPolling transport) DELETE request accepted.");
        }
        finally {
            // Abort the poll after the shutdown timeout if the server doesn't stop the poll.
            if (!this.stopped) {
                this.shutdownTimer = setTimeout(() => {
                    this.logger.log(LogLevel.Warning, "(LongPolling transport) server did not terminate after DELETE request, canceling poll.");
                    // Abort any outstanding poll
                    this.pollAbort.abort();
                }, this.shutdownTimeout);
            }
        }
    }
}
//# sourceMappingURL=LongPollingTransport.js.map