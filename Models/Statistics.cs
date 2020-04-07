using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace task.samet.works.Models
{
    public class Statistics
    {
        [DataType(DataType.Date)]
        public DateTime? Day{get;set;}
        public int FinishedTask { get; set; }
    }
}
