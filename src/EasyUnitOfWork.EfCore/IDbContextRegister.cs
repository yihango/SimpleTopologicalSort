using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace EasyUnitOfWork
{
    /// <summary>
    /// 数据库上下文注册器
    /// </summary>
    public interface IDbContextRegister
    {
        /// <summary>
        /// 注册数据库上下文
        /// </summary>
        /// <typeparam name="TDbContext">数据库上下文类型</typeparam>
        /// <param name="action">配置回调函数</param>
        void Register<TDbContext>(Action<DbContextConfiguration> action);
    }
}
