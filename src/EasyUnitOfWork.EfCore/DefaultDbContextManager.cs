using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.Text;
using System.Linq;

namespace EasyUnitOfWork
{
    /// <summary>
    /// 数据库上下文管理器
    /// </summary>
    public class DefaultDbContextManager : IDbContextResolver, IDbContextRegister
    {
        /// <summary>
        /// 数据库注册实例化回调函数
        /// </summary>
        protected static Dictionary<string, Action<DbContextConfiguration>> _dbContextConfigurationDict =
            new Dictionary<string, Action<DbContextConfiguration>>();



        #region 获取的实现

        /// <inheritdoc/>
        public virtual TDbContext Resolve<TDbContext>(string connectionString, DbConnection existingConnection)
           where TDbContext : DbContext
        {
            var dbContextType = typeof(TDbContext);

            var dbContextConfiguration = new DbContextConfiguration(connectionString, existingConnection);

            _dbContextConfigurationDict[dbContextType.FullName]?.Invoke(dbContextConfiguration);

            // TODO: 实例化数据库上下文
            var constructors = dbContextType.GetConstructors();
            var obj = constructors[0].Invoke(new object[] {
                dbContextConfiguration.DbContextOptions.Options
            });


            return (TDbContext)obj;
        }

        #endregion


        #region 注册的实现

        /// <inheritdoc/>
        public virtual void Register<TDbContext>(Action<DbContextConfiguration> action)
        {
            var key = typeof(TDbContext).FullName;
            _dbContextConfigurationDict[key] = action;
        }

        #endregion
    }
}
