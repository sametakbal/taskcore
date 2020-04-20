using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace taskcore.Models
{
    public class FinishedWork
    {
        
        public string Day{get;set;}
        public int Count { get; set; }
    }
}
