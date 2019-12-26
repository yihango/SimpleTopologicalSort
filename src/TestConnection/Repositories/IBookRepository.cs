using EasyUnitOfWork.Uow.Providers;
using EasyUnitOfWork.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestConnection.Entitys;
using TestConnection.Database;
using Microsoft.EntityFrameworkCore;

namespace TestConnection.Repositories
{
    public interface IBookRepository
    {
        IQueryable<Book> GetAll(bool asNoTracking = false);

        Task InsertAsync(Book entity);

        Task<long> InsertAndGetIdAsync(Book entity);

        Task RemoveAsync(Book entity);

        Task UpdateAsync(Book entity);
    }


    public class BookRepository : IBookRepository
    {
        private readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;

        private AppDbContext DbContext => _currentUnitOfWorkProvider.Current.GetDbContext<AppDbContext>();

        public virtual DbSet<Book> DbQueryTable => DbContext.Set<Book>();

        public BookRepository(
            ICurrentUnitOfWorkProvider currentUnitOfWorkProvider
            )
        {
            _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
        }

        public IQueryable<Book> GetAll(bool asNoTracking = false)
        {
            if (asNoTracking)
            {
                return DbQueryTable.AsNoTracking();
            }

            return DbQueryTable;
        }

        public async Task InsertAsync(Book entity)
        {
            await DbQueryTable.AddAsync(entity);
        }

        public async Task<long> InsertAndGetIdAsync(Book entity)
        {
            await DbQueryTable.AddAsync(entity);
            await DbContext.SaveChangesAsync();
            return entity.BookId;
        }

        public async Task RemoveAsync(Book entity)
        {
            await Task.Yield();

            DbQueryTable.Remove(entity);
        }

        public async Task UpdateAsync(Book entity)
        {
            await Task.Yield();

            DbQueryTable.Update(entity);
        }
    }
}
