using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            string username = Context.User.Identity.Name;  //Retrieves current users username
            await Clients.All.SendAsync("ReceiveMessage", username, message);
        }


        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);  //Adds current connection id to the group indicated

            await Clients.Group(groupName).SendAsync("ReceiveMessageBlank", $"{Context.User.Identity.Name} has joined the group {groupName}.");   //Broadcasts message to the group that this user has joined the group
        }

        public async Task SendGroupMessage(string groupName, string message)
        {
            string username = Context.User.Identity.Name;   //Retrieves current users username
            await Clients.Group(groupName).SendAsync("ReceiveMessageGroup", username, groupName, message);   //Sends message to specified group
        }


        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);         //Removes specified person from the group

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");    //Broadcasts message to the group that this user has left the group
        }
    }
}