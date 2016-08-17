using System;
using Test.Model.Extend;

namespace Test.Model
{
    public class CommissionDetail : EntityBase
    {
        public decimal Amt { get; set; } 
        public Guid UserID { get; set; }
    }
}
