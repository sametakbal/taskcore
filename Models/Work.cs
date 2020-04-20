using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace taskcore.Models
{
    public class Work : BaseEntity
    {
        public Project Project { get; set; }
        public int ProjectId { get; set; }
        public int Id { get; set;}
    }
}