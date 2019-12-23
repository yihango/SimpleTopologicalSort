using System;
using System.Collections.Generic;
using System.Text;
using EasyUnitOfWork.Uow;
using EasyUnitOfWork.Uow.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EasyUnitOfWork
{
    public static class EasyUnitOfWorkIServiceCollectionExtensions
    {
        public static IServiceCollection AddEasyUnitOfWork(this IServiceCollection services)
        {
            services.TryAddTransient<IUnitOfWorkManager, DefaultUnitOfWorkManager>();
            services.TryAddTransient<ICurrentUnitOfWorkProvider, AsyncLocalCurrentUnitOfWorkProvider>();

            return services;
        }
    }
}
