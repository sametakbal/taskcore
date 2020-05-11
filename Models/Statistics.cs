using System;
using System.ComponentModel.DataAnnotations;

namespace task.samet.works.Models
{
    public class Statistics
    {
        [DataType(DataType.Date)]
        public DateTime? Day { get; set; }
        public int FinishedWorks { get; set; }
    }
}
