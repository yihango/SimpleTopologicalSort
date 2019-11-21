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
                services.AddSingleton(item.ModuleType, item.Instance);
            }

            foreach (var item in moduleDescriptors)
            {
                var aModule = GetSingletonInstanceOrNull(services, item.ModuleType);
            }


            serviceProvider = services.BuildServiceProvider();


            Console.ReadKey();
        }

        public static T GetSingletonInstanceOrNull<T>(IServiceCollection services)
        {
            return (T)services
                .FirstOrDefault(d => d.ServiceType == typeof(T))
                ?.ImplementationInstance;
        }

        public static object GetSingletonInstanceOrNull(IServiceCollection services, Type type)
        {
            return services
                .FirstOrDefault(d => d.ServiceType == type)
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



}