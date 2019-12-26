using EasyUnitOfWork.Uow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyUnitOfWork.Extensions
{
    public static class EfCoreUnitOfWorkExtensions
    {
        public static TDbContext GetDbContext<TDbContext>(this IActiveUnitOfWork unitOfWork, string name = null)
          where TDbContext : DbContext
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork");
            }

            if (!(unitOfWork is EfCoreUnitOfWork))
            {
                throw new ArgumentException("unitOfWork is not type of " + typeof(EfCoreUnitOfWork).FullName, "unitOfWork");
            }

            return (unitOfWork as EfCoreUnitOfWork).GetOrCreateDbContext<TDbContext>(name);
        }
    }
}
