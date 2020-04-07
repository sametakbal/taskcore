using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasky.Models
{
    public class UserWorks
    {
        public int Id{get;set;}
        public User User{get;set;}
        public int UserId { get; set; }
        public Work Work { get; set; }
        public int WorkId { get; set; }
    }
}