using System;
using System.ComponentModel.DataAnnotations;

namespace Estore.Models
{
    public class Message
    {
        public int Id { get; set; }
        public User Sender { get; set; }
        public User Recipient { get; set; }
        public Item SubjectItem { get; set; }
        public DateTime Date { get; set; }

        [Required, StringLength(2000, MinimumLength = 2)]
        public string MessageBody { get; set; }
    }
}
