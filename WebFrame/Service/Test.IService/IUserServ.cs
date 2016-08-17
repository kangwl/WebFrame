using System;
using Test.Model;

namespace Test.IService
{
    public interface IUserServ
    {
        User GetOne(Guid id);
        bool Insert(User user);
    }
}