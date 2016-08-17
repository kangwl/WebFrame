using System;
using System.ComponentModel.DataAnnotations;

namespace Test.Model.Extend
{
    public class EntityBase
    {
        [Key]
        public Guid ID { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
