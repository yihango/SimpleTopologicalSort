using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace EasyUnitOfWork.Uow
{
    /// <summary>
    /// 工作单元配置参数
    /// </summary>
    public class UnitOfWorkOptions
    {
        /// <summary>
        /// 事务范围设置
        /// </summary>
        public TransactionScopeOption? Scope { get; set; }

        /// <summary>
        /// 工作单元是否启用事务,为空则使用默认值
        /// </summary>
        public bool? IsTransactional { get; set; }

        /// <summary>
        /// 工作单元超时配置,为空则使用默认值
        /// </summary>
        public TimeSpan? Timeout { get; set; }

        /// <summary>
        /// 工作单元事务级别配置,为空则使用默认值
        /// </summary>
        public IsolationLevel? IsolationLevel { get; set; }

        /// <summary>
        /// 如果在异步范围中使用工作单元，则应将此选项设置为  <see cref="TransactionScopeAsyncFlowOption.Enabled"/>
        /// </summary>
        public TransactionScopeAsyncFlowOption? AsyncFlowOption { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public UnitOfWorkOptions()
        {

        }
    }
}