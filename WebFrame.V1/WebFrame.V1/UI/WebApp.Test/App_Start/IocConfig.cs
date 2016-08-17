using System.Reflection;
using Test.Util.Mvc.IOCMvc;

namespace WebApp.Test
{
    public class IocConfig
    {
        public static void Regist()
        { 
              IOC.RegisterControllersByAssembly(Assembly.GetExecutingAssembly(),Assembly.Load("Test.Service"), "Test.Service","Serv");
        }
    }
}