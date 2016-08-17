using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Data; 

namespace Test.Service
{
    public class ServiceBase
    {
       
        public TestContext GetDataContext()
        {
            return new TestContext();
        }
    }
}
