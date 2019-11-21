using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EfUnitOfWorkDemo.Uow
{
    public interface IUnitOfWorkManager
    {

    }

    public class UnitOfWorkManager : IUnitOfWorkManager
    {
        public IUnitOfWork Current => GetCurrentUnitOfWork();

        protected readonly IAmbientUnitOfWork _ambientUnitOfWork;
        protected readonly IServiceScopeFactory _serviceScopeFactory;

        public UnitOfWorkManager(IAmbientUnitOfWork ambientUnitOfWork, IServiceScopeFactory serviceScopeFactory)
        {
            _ambientUnitOfWork = ambientUnitOfWork;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public IUnitOfWork Begin(UnitOfWorkOptions options, bool requiresNew = false)
        {
            var currentUow = Current;
            if (currentUow != null && !requiresNew)
            {
                return new ChildUnitOfWork(currentUow);
            }

            var unitOfWork = CreateNewUnitOfWork();
            unitOfWork.Initialize(options);

            return unitOfWork;
        }

        private IUnitOfWork GetCurrentUnitOfWork()
        {
            var uow = _ambientUnitOfWork.UnitOfWork;

            //Skip reserved unit of work
            while (uow != null && (uow.IsReserved || uow.IsDisposed || uow.IsCompleted))
            {
                uow = uow.Outer;
            }

            return uow;
        }
    }
}
