using Microsoft.AspNetCore.SignalR;

namespace ChatSystem.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendFriendRequestNotification(string recipientUserId)
        {
            await Clients.User(recipientUserId).SendAsync("ReceiveFriendRequestNotification");
        }

        //public async Task NotificationFriendRequest(string userId)
        //{
        //    await Clients.User(userId).SendAsync("ReceiveFriendRequestNotification");
        //}

        public async Task JoinGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }

        public async Task NewFriendRequest(string userId)
        {
            await Clients.User(userId).SendAsync("OnSendFriendRequest");
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
