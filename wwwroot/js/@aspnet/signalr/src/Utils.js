// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
import { LogLevel } from "./ILogger";
import { NullLogger } from "./Loggers";
export class Arg {
    static isRequired(val, name) {
        if (val === null || val === undefined) {
            throw new Error(`The '${name}' argument is required.`);
        }
    }
    static isIn(val, values, name) {
        // TypeScript enums have keys for **both** the name and the value of each enum member on the type itself.
        if (!(val in values)) {
            throw new Error(`Unknown ${name} value: ${val}.`);
        }
    }
}
export function getDataDetail(data, includeContent) {
    let length = null;
    if (data instanceof ArrayBuffer) {
        length = `Binary data of length ${data.byteLength}`;
        if (includeContent) {
            length += `. Content: '${formatArrayBuffer(data)}'`;
        }
    }
    else if (typeof data === "string") {
        length = `String data of length ${data.length}`;
        if (includeContent) {
            length += `. Content: '${data}'.`;
        }
    }
    return length;
}
export function formatArrayBuffer(data) {
    const view = new Uint8Array(data);
    // Uint8Array.map only supports returning another Uint8Array?
    let str = "";
    view.forEach((num) => {
        const pad = num < 16 ? "0" : "";
        str += `0x${pad}${num.toString(16)} `;
    });
    // Trim of trailing space.
    return str.substr(0, str.length - 1);
}
export async function sendMessage(logger, transportName, httpClient, url, accessTokenFactory, content, logMessageContent) {
    let headers;
    const token = await accessTokenFactory();
    if (token) {
        headers = {
            ["Authorization"]: `Bearer ${token}`,
        };
    }
    logger.log(LogLevel.Trace, `(${transportName} transport) sending data. ${getDataDetail(content, logMessageContent)}.`);
    const response = await httpClient.post(url, {
        content,
        headers,
    });
    logger.log(LogLevel.Trace, `(${transportName} transport) request complete. Response status: ${response.statusCode}.`);
}
export function createLogger(logger) {
    if (logger === undefined) {
        return new ConsoleLogger(LogLevel.Information);
    }
    if (logger === null) {
        return NullLogger.instance;
    }
    if (logger.log) {
        return logger;
    }
    return new ConsoleLogger(logger);
}
export class Subject {
    constructor(cancelCallback) {
        this.observers = [];
        this.cancelCallback = cancelCallback;
    }
    next(item) {
        for (const observer of this.observers) {
            observer.next(item);
        }
    }
    error(err) {
        for (const observer of this.observers) {
            if (observer.error) {
                observer.error(err);
            }
        }
    }
    complete() {
        for (const observer of this.observers) {
            if (observer.complete) {
                observer.complete();
            }
        }
    }
    subscribe(observer) {
        this.observers.push(observer);
        return new SubjectSubscription(this, observer);
    }
}
export class SubjectSubscription {
    constructor(subject, observer) {
        this.subject = subject;
        this.observer = observer;
    }
    dispose() {
        const index = this.subject.observers.indexOf(this.observer);
        if (index > -1) {
            this.subject.observers.splice(index, 1);
        }
        if (this.subject.observers.length === 0) {
            this.subject.cancelCallback().catch((_) => { });
        }
    }
}
export class ConsoleLogger {
    constructor(minimumLogLevel) {
        this.minimumLogLevel = minimumLogLevel;
    }
    log(logLevel, message) {
        if (logLevel >= this.minimumLogLevel) {
            switch (logLevel) {
                case LogLevel.Critical:
                case LogLevel.Error:
                    console.error(`${LogLevel[logLevel]}: ${message}`);
                    break;
                case LogLevel.Warning:
                    console.warn(`${LogLevel[logLevel]}: ${message}`);
                    break;
                case LogLevel.Information:
                    console.info(`${LogLevel[logLevel]}: ${message}`);
                    break;
                default:
                    // console.debug only goes to attached debuggers in Node, so we use console.log for Trace and Debug
                    console.log(`${LogLevel[logLevel]}: ${message}`);
                    break;
            }
        }
    }
}
//# sourceMappingURL=Utils.js.map