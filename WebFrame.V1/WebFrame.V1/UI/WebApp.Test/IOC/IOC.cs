using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;

namespace WebApp.Test.IOC
{
    public class IOC
    {
        /// <summary>
        /// 根据程序集和服务的命名空间进行 MVC IOC 的注册
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="controllerName"></param>
        /// <param name="serviceNamespace"></param>
        /// <param name="serviceNameEndsWith"></param>
        public static void RegisterControllersByAssembly(string assemblyName, string controllerName,string serviceNamespace,
            string serviceNameEndsWith = "")
        {
            var assembly = Assembly.Load(assemblyName);
            Func<Type, bool> whereFunc = t => t.Namespace == serviceNamespace;
            if (!string.IsNullOrEmpty(serviceNameEndsWith))
            {
                whereFunc += t => t.Name.EndsWith(serviceNameEndsWith);
            }
            var types = assembly.GetTypes().Where(whereFunc);
            RegisterControllers(controllerName,types.ToArray());
        }

        /// <summary>
        /// 根据类型注册 MVC IOC
        /// </summary>
        /// <param name="types"></param>
        public static void RegisterControllers(params Type[] types)
        {
            //根据类型注册
            var builder = new ContainerBuilder();
            builder.RegisterTypes(types).AsImplementedInterfaces().PropertiesAutowired();
            builder.RegisterControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        public static void RegisterControllers(string controllAssembly, params Type[] types)
        {
            //根据类型注册
            var builder = new ContainerBuilder();
            builder.RegisterTypes(types).AsImplementedInterfaces().PropertiesAutowired();
            builder.RegisterControllers(Assembly.Load(controllAssembly)).PropertiesAutowired();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
