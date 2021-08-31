using Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace LoymaxTest
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var serviceScope = host.Services.GetService<IServiceScopeFactory>()?.CreateScope())
            {
                if (serviceScope != null) 
                    await RunDbMigrations(serviceScope.ServiceProvider);
            }

            await host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }

        private static async Task RunDbMigrations(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<LoymaxTestContext>();
            context.Database.SetCommandTimeout(TimeSpan.FromMinutes(5));
            await context.Database.MigrateAsync();
            context.Database.SetCommandTimeout(TimeSpan.FromSeconds(30));
        }
    }
}
