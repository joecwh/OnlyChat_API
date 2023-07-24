using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Message
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; } 
    }
}
