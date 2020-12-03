using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            string username = Context.User.Identity.Name;
            await Clients.All.SendAsync("ReceiveMessage", username, message);
        }

        public async Task SendDirectMessage(string user, string message)
        {
            string username = Context.User.Identity.Name;
            await Clients.User(user).SendAsync("ReceiveMessage", username, message);
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("ReceiveMessageBlank", $"{Context.User.Identity.Name} has joined the group {groupName}.");
        }

        public async Task SendGroupMessage(string groupName, string message)
        {
            string username = Context.User.Identity.Name;
            await Clients.Group(groupName).SendAsync("ReceiveMessage", username, message);
        }


        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
        }
    }
}