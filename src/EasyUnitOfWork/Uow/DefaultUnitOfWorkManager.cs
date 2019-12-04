using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using EasyUnitOfWork.Uow.Handles;

namespace EasyUnitOfWork.Uow
{
    public class DefaultUnitOfWorkManager : IUnitOfWorkManager
    {

        protected readonly IServiceProvider _serviceProvider;

        public IActiveUnitOfWork Current => throw new NotImplementedException();

        public DefaultUnitOfWorkManager(
            IServiceProvider serviceProvider
            )
        {
            _serviceProvider = serviceProvider;
        }

        public IUnitOfWorkCompleteHandle Begin()
        {
            throw new NotImplementedException();
        }

        public IUnitOfWorkCompleteHandle Begin(TransactionScopeOption scope)
        {
            throw new NotImplementedException();
        }

        public IUnitOfWorkCompleteHandle Begin(UnitOfWorkOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
