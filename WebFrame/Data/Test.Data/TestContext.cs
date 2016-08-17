using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using Test.Data.Map;

namespace Test.Data
{
    public class TestContext : DbContext
    {
        public TestContext() : base("name=mysqlConn")
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<TestContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new CommissionDetailMap());
            base.OnModelCreating(modelBuilder);
        }

        //public DbSet<User> User { get; set; } 
        //public DbSet<Commission> Commission { get; set; } 

    }
}
