using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Tasky.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string Surname { get; set; }
        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(20)]
        public string Password { get; set; }

        [StringLength(1000)]
        public string About{get;set;}="Bir bilgi girilmedi";

    }
}