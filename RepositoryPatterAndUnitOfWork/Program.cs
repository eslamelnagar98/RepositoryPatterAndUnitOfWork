using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RepositoryPatterAndUnitOfWork.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositoryPatterAndUnitOfWork
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            DatabaseMigrate(host);
            host.Run();
        }

        private static void DatabaseMigrate(IHost host)
        {
            //Creates a new IServiceScope that can be used to resolve scoped services.
            using var scope = host.Services.CreateScope();
            //The IServiceProvider used to resolve dependencies from the scope.
            var RepoDb = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            RepoDb.Database.Migrate();
            Console.WriteLine(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")??"Islammmmmmmmmmmmmm");
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
