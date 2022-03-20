using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Estore.Models
{
    /// <summary>
    /// Represents a message sent between users regarding a specific item.
    /// </summary>
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public int SubjectItemId { get; set; }
        public DateTime Date { get; set; }

        [Required, StringLength(2000, MinimumLength = 2), Display(Name = "message")]
        public string MessageBody { get; set; }

        /*
         * In order to circumvent identity column limitations with many-to-many in EF.
         * An alternative solution could be to create three additional DbSets.
         */
        [NotMapped]
        public User Sender { get; set; }
        
        [NotMapped]
        public User Recipient { get; set; }

        [NotMapped]
        public Item SubjectItem { get; set; }
    }
}
