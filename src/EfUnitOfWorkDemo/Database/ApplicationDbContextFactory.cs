using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace EfUnitOfWorkDemo.Database
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AppDbContext>();

            var workPath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
            var configurationBuilder = new ConfigurationBuilder()
                   .SetBasePath(workPath)
                   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = configurationBuilder.Build();
            var connectionString = configuration["ConnectionStrings:Default"];

            builder.UseSqlServer(connectionString);

            return new AppDbContext(builder.Options);
        }
    }
}