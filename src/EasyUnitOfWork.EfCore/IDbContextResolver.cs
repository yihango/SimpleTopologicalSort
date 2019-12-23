using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace EasyUnitOfWork
{
    /// <summary>
    /// 数据库上下文实例化器
    /// </summary>
    public interface IDbContextResolver
    {
        /// <summary>
        /// 创建数据库上下文
        /// </summary>
        /// <typeparam name="TDbContext">数据库上下文类型</typeparam>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="existingConnection">已存在的数据库连接</param>
        /// <returns>数据库上下文</returns>
        TDbContext Resolve<TDbContext>(string connectionString, DbConnection existingConnection)
          where TDbContext : DbContext;
    }
}
