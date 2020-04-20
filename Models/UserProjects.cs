using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace taskcore.Models
{
    public class UserProjects
    {
        public int Id { get; set; }
        public User User { get; set; }

        public int UserId { get; set; }
        public Project Project { get; set; }
        public int ProjectId { get; set; }

        public bool IsAccept{get;set;}=false;
    }

}
