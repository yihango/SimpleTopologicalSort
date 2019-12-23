using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyUnitOfWork.Uow
{
    /// <summary>
    /// EF Core 事务策略
    /// </summary>
    public interface IEfCoreTransactionStrategy : IDisposable
    {
        /// <summary>
        /// 初始化工作单元选项
        /// </summary>
        /// <param name="options"></param>
        void InitOptions(UnitOfWorkOptions options);

        /// <summary>
        /// 创建数据库上下文对象
        /// </summary>
        /// <typeparam name="TDbContext">数据库上下文类型</typeparam>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="dbContextResolver">数据库上下文实例化器</param>
        /// <returns></returns>
        DbContext CreateDbContext<TDbContext>(string connectionString, IDbContextResolver dbContextResolver)
            where TDbContext : DbContext;

        /// <summary>
        /// 提交
        /// </summary>
        void Commit();
    }
}
