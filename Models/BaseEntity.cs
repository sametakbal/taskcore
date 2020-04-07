using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tasky.Models
{
    public class BaseEntity
    {
        public string Title { get; set; }
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
