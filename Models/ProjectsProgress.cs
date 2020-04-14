using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasky.Models
{
    public class ProjectsProgress
    {
        public int Id { get; set; }
        public Project Project { get; set; }

        public int Progress { get; set; }

    }
}