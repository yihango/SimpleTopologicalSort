using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleTopologicalSortDemo
{
    /// <summary>
    /// 模块依赖的属性
    /// </summary>
    public class DependModulesAttribute : Attribute
    {
        /// <summary>
        /// 依赖的模块类型
        /// </summary>
        public Type[] DependModuleTypes { get; private set; }

        public DependModulesAttribute(params Type[] dependModuleTypes)
        {
            DependModuleTypes = dependModuleTypes ?? new Type[0];
        }
    }
}
