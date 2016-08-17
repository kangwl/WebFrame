using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Test.IService;
using Test.Model;
using Test.Service;
using Test.Service.Service;
using Test.Util.Common.IOC;

namespace ConsoleApp.Test
{
    public interface IWrite
    {
        void Some();
    }

    public class Write : IWrite
    {
        public void Some()
        {
            Console.WriteLine("hello kwl");
            Console.Read();
        }
    }

    public class MyWrite : IWrite
    {
        public void Some()
        {
            Console.WriteLine("mywrite some");
            Console.Read();
        }
    }
    
    class Program
    {

        static void Main(string[] args)
        {
            //CreateUser(); 

            //Console.WriteLine("ok");


            //ioc 
            //IOCHepler.Regist<Write, IWrite>("Write");
           // IOCHepler.Regist<MyWrite, IWrite>("MyWrite");
            IOCHepler.Regist(typeof(Write),typeof(MyWrite));
            IOCHepler.Build();
            var write = IOCHepler.Resolve<MyWrite>();
            write.Some();
            Console.Read();
        }

        private static void CreateUser()
        { 

            IUserServ userServ = new UserServ();
            User user = new User();
            user.ID = Guid.NewGuid();
            user.Age = 12;
            user.Name = "kwl";
            user.QQ2 = "5566667";

            //user.CommissionDetails = new List<CommissionDetail>()
            //{
            //    new CommissionDetail() {ID = Guid.NewGuid(), Amt = 12, User = user},
            //     new CommissionDetail() {ID = Guid.NewGuid(), Amt = 15, User = user}
            //};

            userServ.Insert(user);
        }
    }
}
