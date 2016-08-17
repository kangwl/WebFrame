using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Model;

namespace Test.Data.Map
{
    public class CommissionDetailMap : EntityTypeConfiguration<CommissionDetail>
    {
        public CommissionDetailMap()
        {
            this.ToTable("CommissionDetail");
            this.HasKey(one => one.ID);
           // this.Ignore(one => one.UpdateTime).Ignore(one => one.CreateTime);
        }
    }
}
