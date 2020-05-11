using System;
using System.ComponentModel.DataAnnotations;

namespace taskcore.Models
{
    public class BaseEntity
    {
        [StringLength(150)]
        public string Title { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public DateTime? StartTime { get; set; }
        [DataType(DataType.Date)]
        public DateTime? FinishTime { get; set; }
        public Status Status { get; set; }

    }
    public enum Status
    {
        NotStarted,
        OnWorking,
        Test,
        Done
    };
}
