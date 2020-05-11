using System;
using System.ComponentModel.DataAnnotations;

namespace taskcore.Models
{
    public class Project : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        [DataType(DataType.Date)]
        public DateTime GoalTime { get; set; }
    }
}
