using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PromoCodeFactory.DataAccess;
using PromoCodeFactory.DataAccess.Data;
using System;

namespace PromoCodeFactory.WebHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            MigrateDatabase(host.Services);

            host.Run();
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

            var newDatabase = !dbContext.Database.CanConnect();
            dbContext.Database.Migrate();

            if (newDatabase)
            {
                var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                dbInitializer.InitializeDb();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}