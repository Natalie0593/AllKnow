using System;
using System.Threading.Tasks;
using BlogHost.Initializer;
using Entities;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Services;

namespace BlogHost
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var context = services.GetRequiredService<AppDBContext>();
                    await RoleInitializer.InitializeAsync(userManager, rolesManager);
                   
                    DBInitializer.Initial(context);

                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile(
                    "mailconfig.json", optional: false, reloadOnChange: false);
                config.AddJsonFile(
                    "managerconfig.json", optional: false, reloadOnChange: false);
            })
            .UseStartup<Startup>()
            .ConfigureLogging(logging => logging.SetMinimumLevel(LogLevel.Trace))
            .Build();
    }
}
