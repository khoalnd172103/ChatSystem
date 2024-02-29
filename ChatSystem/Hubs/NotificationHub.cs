using Microsoft.AspNetCore.SignalR;

namespace ChatSystem.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendFriendRequestNotification()
        {
            await Clients.All.SendAsync("ReceiveFriendRequestNotification");
        }

        //public override async Task OnConnectedAsync()
        //{
        //    // Add the connected user to a group with their user ID
        //    string userId = Context.UserIdentifier;
        //    await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        //    await base.OnConnectedAsync();
        //}
    }
}
