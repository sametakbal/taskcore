using System;
using System.ComponentModel.DataAnnotations;

namespace Tasky.Models
{
    public class Mail
    {
        public int Id { get; set; }
        [StringLength(150)]
        public string Subject { get; set; }
        public User Sender { get; set; }
        public int SenderId { get; set; }
        public User Receiver { get; set; }
        public int ReceiverId { get; set; }

        [StringLength(2000)]
        public string Content { get; set; }

        public bool IsRead { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;

    }
}