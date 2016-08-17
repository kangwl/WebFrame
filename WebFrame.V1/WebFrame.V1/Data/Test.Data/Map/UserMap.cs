using System.Data.Entity.ModelConfiguration;
using Test.Model;

namespace Test.Data.Map
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            this.ToTable("User");
            this.HasKey(one => one.ID);
           // this.HasMany(one => one.CommissionDetails);
        }
    }
}
