using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyUnitOfWork.Uow
{
    /// <summary>
    /// 当前激活的工作单元。
    /// 必须通过
    /// <see cref="IUnitOfWorkManager"/>
    /// 实例化
    /// </summary>
    public interface IActiveUnitOfWork
    {
        /// <summary>
        /// 成功
        /// </summary>
        event EventHandler Completed;

        /// <summary>
        /// 失败
        /// </summary>
        event EventHandler<Exception> Failed;

        /// <summary>
        /// 释放
        /// </summary>
        event EventHandler Disposed;

        /// <summary>
        /// 选项
        /// </summary>
        UnitOfWorkOptions Options { get; }

        /// <summary>
        /// 自定义选项
        /// </summary>
        Dictionary<string, object> Items { get; set; }

        /// <summary>
        /// 是否被释放
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// 保存
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// 保存-异步
        /// </summary>
        Task SaveChangesAsync();
    }
}
