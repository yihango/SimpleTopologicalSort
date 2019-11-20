using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SimpleTopologicalSortDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // https://www.cnblogs.com/myzony/p/9201768.html


            IServiceCollection services = new ServiceCollection();
            IServiceProvider serviceProvider = null;


            var moduleManager = new ModuleManager();
            var moduleDescriptors = moduleManager.ModuleSort<AModule>();
            foreach (var item in moduleDescriptors)
            {
                services.AddSingleton(item.ModuleType);
            }

            var aModule = GetSingletonInstanceOrNull<AModule>(services);

            serviceProvider = services.BuildServiceProvider();


            Console.ReadKey();
        }

        public static T GetSingletonInstanceOrNull<T>(IServiceCollection services)
        {
            return (T)services
                .FirstOrDefault(d => d.ServiceType == typeof(T))
                ?.ImplementationInstance;
        }



    }

    //    public static class ModuleEx
    //    {
    //        private static Type _dependModulesAttributeType = typeof(DependModulesAttribute);
    //        
    //        public static DependModulesAttribute GetDependModulesAttribute(Type moduleType)
    //        {
    //            try
    //            return moduleType.GetCustomAttribute<DependModulesAttribute>();
    //        }
    //    }


    public class ModuleDescriptor
    {
        /// <summary>
        /// 模块类型
        /// </summary>
        public Type ModuleType { get; private set; }

        /// <summary>
        /// 依赖项
        /// </summary>
        public ModuleDescriptor[] Dependencies { get; private set; }

        public ModuleDescriptor(Type moduleType, params ModuleDescriptor[] dependencies)
        {
            ModuleType = moduleType;
            Dependencies = dependencies ?? new ModuleDescriptor[0];
        }

        public override string ToString()
        {
            return ModuleType.Name;
        }
    }
}