using Microsoft.AspNetCore.SignalR;

namespace ChatSystem.Hubs
{
    public class MessageHub : Hub
    {
        //private static readonly Dictionary<string, string> userConnections = new Dictionary<string, string>();

        //public void RegisterUser(string userId)
        //{
        //    string connectionId = Context.ConnectionId;
        //    // Store the mapping between user ID and connection ID
        //    userConnections[userId] = connectionId;
        //}

        //public async Task SendMessageToUser(string userId, string message)
        //{
        //    if (userConnections.TryGetValue(userId, out string connectionId))
        //    {
        //        await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
        //    }
        //    else
        //    {
        //        // Handle case where user is not found or offline
        //    }
        //}


        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }


    }
}
