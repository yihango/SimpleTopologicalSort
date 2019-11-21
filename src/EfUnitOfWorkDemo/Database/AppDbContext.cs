using EfUnitOfWorkDemo.Domain.Books;
using EfUnitOfWorkDemo.Domain.BookStores;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EfUnitOfWorkDemo.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options)
            :base(options)
        {

        }


        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<BookStore> BookStores { get; set; }
        public virtual DbSet<BookAndStoreAssociated> BookAndStoreAssociateds { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
