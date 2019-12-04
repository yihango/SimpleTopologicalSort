using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyUnitOfWork.Uow.Handles
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUnitOfWorkCompleteHandle : IDisposable
    {
        /// <summary>
        /// 提交工作单元
        /// </summary>
        void Complete();

        /// <summary>
        /// 提交工作单元 - 异步
        /// </summary>
        Task CompleteAsync();
    }
}

