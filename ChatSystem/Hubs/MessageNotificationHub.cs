using Microsoft.AspNetCore.SignalR;

namespace ChatSystem.Hubs
{
    public class MessageNotificationHub : Hub
    {
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
