using Microsoft.AspNetCore.SignalR;
using ChatServer.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using ChatServer.Models;

namespace ChatServer.Hubs
{
    public class ChatHub : Hub
    {
        public ChatHub(ApplicationDbContext userContext)
        {
            _userContext = userContext;
            
        }


        private readonly ApplicationDbContext _userContext;
        private static Dictionary<string, List<string>> _userGroups  = new Dictionary<string, List<string>>();


        public async Task JoinGroup(string groupName)
        {
            if (_userGroups.ContainsKey(Context.ConnectionId) && !_userGroups[Context.ConnectionId].Contains(groupName))
            {
                _userGroups[Context.ConnectionId].Add(groupName);
            }
            else
            {
                List<string> temp = new List<string>();
                temp.Add(groupName);
                _userGroups.Add(Context.ConnectionId, temp);
            }
            
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        }
        





        public async Task SendMessage(string chatID)
        {
            await Clients.Group(chatID).SendAsync("ReceiveMessage");
        }

        public async Task AddToGroup(string chatId)
        {
            // Получить список пользователей в чате
           
        }
    }
}
