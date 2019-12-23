using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using EasyUnitOfWork.Uow;

namespace EasyUnitOfWork
{
    public static class EasyUnitOfWorkEfCoreIServiceCollectionExtensions
    {
        public static IServiceCollection AddEasyUnitOfWorkEfCore(this IServiceCollection services)
        {
            services.TryAddTransient<IDbContextRegister, DefaultDbContextManager>();

            services.TryAddTransient<IDbContextResolver, DefaultDbContextManager>();

            services.TryAddTransient<IEfCoreTransactionStrategy, DbContextEfCoreTransactionStrategy>();

            services.TryAddTransient<EfCoreUnitOfWork>();

            return services;
        }
    }
}
