using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TestConnection.Database
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder();

            var dir = Path.GetDirectoryName(this.GetType().Assembly.Location);
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
              .SetBasePath(dir)
              .AddJsonFile("appsettings.json", true, true);

            var configuration = configurationBuilder.Build();

            var connectionString = configuration["ConnectionStrings:Default"];

            builder.UseSqlServer(connectionString);

            return new AppDbContext(builder.Options);
        }
    }
}
