using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Test.Util.Common.IOC
{
    public static class IOCHepler
    {
        private static IContainer container { get; set; }

        private static readonly ContainerBuilder builder;
        static IOCHepler()
        {
            if (builder == null)
            {
                builder = new ContainerBuilder();
            }
        }

        public static void Build()
        {
            container = builder.Build();
        }

        public static void Regist<T>(string name="")
        {
            if (string.IsNullOrEmpty(name))
            {
                builder.RegisterType<T>();
            }
            else
            {
                builder.RegisterType<T>().Named<T>(name);
            }
        }


        public static void RegistSingleton<T>()
        {
            builder.RegisterType<T>().SingleInstance();
        }


        public static void Regist<T, IT>(string name)
        {
            builder.RegisterType<T>().Named<IT>(name).As<IT>().AsImplementedInterfaces().PropertiesAutowired();
        }

        public static void Regist(params Type[] types)
        {
            if (!types.Any()) return;
            builder.RegisterTypes(types).PropertiesAutowired();
        }

        public static void RegistSingleton<T, IT>()
        {
            builder.RegisterType<T>().As<IT>().SingleInstance();
        }

        public static TIT Resolve<TIT>(string name = "")
        {
            using (var scop = container.BeginLifetimeScope())
            { 
                var tIt = string.IsNullOrEmpty(name) ? scop.Resolve<TIT>() : scop.ResolveNamed<TIT>(name);
                return tIt;
            }
        }

    }
}
