using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestConnection.Entitys;

namespace TestConnection.Database
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<Book> Books { get; set; }

        public AppDbContext(DbContextOptions options)
            : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
