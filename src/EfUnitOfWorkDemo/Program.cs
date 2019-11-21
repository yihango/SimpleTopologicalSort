using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EfUnitOfWorkDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var aa = typeof(Program).Assembly.Location;

            var configurationBuilder = new ConfigurationBuilder()
                    .SetBasePath(typeof(Program).Assembly.Location)
          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
