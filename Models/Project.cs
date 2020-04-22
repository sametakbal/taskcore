﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
