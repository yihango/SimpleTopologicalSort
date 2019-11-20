using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SimpleTopologicalSortDemo
{
    public interface IModuleManager
    {
        IList<ModuleDescriptor> ModuleSort<TModule>()
            where TModule : RModule;
    }

    public class ModuleManager : IModuleManager
    {
        /// <summary>
        /// 模块接口类型全名称
        /// </summary>
        static string _moduleInterfaceTypeFullName = typeof(IRModule).FullName;


        /// <summary>
        /// 模块排序
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <returns></returns>
        public virtual IList<ModuleDescriptor> ModuleSort<TModule>()
            where TModule : RModule
        {
            var moduleDescriptors = VisitModule(typeof(TModule));

            return Topological.Sort(moduleDescriptors, o => o.Dependencies);
        }

        /// <summary>
        /// 遍历模块
        /// </summary>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        protected virtual List<ModuleDescriptor> VisitModule(Type moduleType)
        {
            var moduleDescriptors = new List<ModuleDescriptor>();

            // 过滤抽象类、接口、泛型类、非类
            if (moduleType.IsAbstract
                || moduleType.IsInterface
                || moduleType.IsGenericType
                || !moduleType.IsClass)
            {
                return moduleDescriptors;
            }

            // 过滤没有实现IRModule接口的类
            var baseInterfaceType = moduleType.GetInterface(_moduleInterfaceTypeFullName, false);
            if (baseInterfaceType == null)
            {
                return moduleDescriptors;
            }

            // 
            var dependModulesAttribute = moduleType.GetCustomAttribute<DependModulesAttribute>();

            // 依赖属性为空
            if (dependModulesAttribute == null)
            {
                moduleDescriptors.Add(new ModuleDescriptor(moduleType));
            }
            else
            {
                // 依赖属性不为空,递归获取依赖
                var dependModuleDescriptors = new List<ModuleDescriptor>();
                foreach (var dependModuleType in dependModulesAttribute.DependModuleTypes)
                {
                    dependModuleDescriptors.AddRange(
                        VisitModule(dependModuleType)
                    );
                }

                // 创建模块描述信息,内容为模块类型和依赖类型
                moduleDescriptors.Add(new ModuleDescriptor(moduleType, dependModuleDescriptors.ToArray()));
            }

            return moduleDescriptors;
        }

    }
}
