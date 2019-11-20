using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleTopologicalSortDemo
{
    public class ModuleDescriptor
    {
        private object _instance;

        /// <summary>
        /// 模块类型
        /// </summary>
        public Type ModuleType { get; private set; }

        /// <summary>
        /// 依赖项
        /// </summary>
        public ModuleDescriptor[] Dependencies { get; private set; }

        /// <summary>
        /// 实例
        /// </summary>
        public object Instance { get => this._instance; }

        public ModuleDescriptor(Type moduleType, params ModuleDescriptor[] dependencies)
        {

            this.ModuleType = moduleType;
            this.Dependencies = dependencies ?? new ModuleDescriptor[0];
            this._instance = Activator.CreateInstance(this.ModuleType);
        }


    }
}
