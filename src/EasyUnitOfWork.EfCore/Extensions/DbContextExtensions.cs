using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;


namespace EasyUnitOfWork.Extensions
{
    public static class DbContextExtensions
    {
        public static bool HasRelationalTransactionManager(this DbContext dbContext)
        {
            return dbContext.Database.GetService<IDbContextTransactionManager>() is IRelationalTransactionManager;
        }
    }
}
