using ChatServer.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatServer.Models
{
    public class Message
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        [ForeignKey("ChatId")]
        public Chat Chat { get; set; }
        public ApplicationUser user { get; set; }
    }
}
