using System;
using System.Linq;
using Test.Data; 
using Test.IService;
using Test.Model;

namespace Test.Service.Service
{
    public class UserServ : ServiceBase, IUserServ
    {
        public User GetOne(Guid id)
        { 
            using (TestContext context = GetDataContext())
            {
                return context.Set<User>().SingleOrDefault(one => one.ID == id);
            } 
        }
         

        public bool Insert(User user)
        { 
            using (TestContext context = GetDataContext())
            {
                context.Set<User>().Add(user);
                int ret = context.SaveChanges();
                return ret > 0;
            } 
        }
    }
}
