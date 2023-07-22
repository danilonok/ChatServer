using ChatServer.Areas.Identity.Data;

namespace ChatServer.Models
{
    public class Chat
    {

        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }

        public ICollection<ApplicationUser> ChatUsers { get; set; } = new List<ApplicationUser>();

        public ICollection<Message> Messages { get; set; }
    }
}
