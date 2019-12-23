using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace EasyUnitOfWork
{
    /// <summary>
    /// 数据库上下文配置信息
    /// </summary>
    public class DbContextConfiguration
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; internal set; }

        /// <summary>
        /// 已存在的连接
        /// </summary>
        public DbConnection ExistingConnection { get; internal set; }

        /// <summary>
        /// 数据库选项配置器
        /// </summary>
        public DbContextOptionsBuilder DbContextOptions { get; }

        public DbContextConfiguration(string connectionString, DbConnection existingConnection)
        {
            ConnectionString = connectionString;
            ExistingConnection = existingConnection;

            DbContextOptions = new DbContextOptionsBuilder();
        }
    }
}
