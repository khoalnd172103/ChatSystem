using Microsoft.AspNetCore.SignalR;

namespace ChatSystem.Hubs
{
    public class GroupChatHub: Hub
    {
        public async Task UpdateConversationName(int conversationId, string newGroupName)
        {
            await Clients.All.SendAsync("ReceiveUpdatedConversationName", conversationId, newGroupName);
        }
    }
}
