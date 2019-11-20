using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleTopologicalSortDemo
{
    /// <summary>
    /// 依赖
    /// </summary>
    public class DependModulesAttribute : Attribute
    {

        public Type[] DependModuleTypes { get; private set; }

        public DependModulesAttribute(params Type[] dependModuleTypes)
        {
            DependModuleTypes = dependModuleTypes;
        }
    }
}
