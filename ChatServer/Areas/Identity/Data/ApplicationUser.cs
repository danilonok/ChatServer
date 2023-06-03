using ChatServer.Models;
using Microsoft.AspNetCore.Identity;

namespace ChatServer.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public ICollection<Chat> Chats { get; set; }
}

