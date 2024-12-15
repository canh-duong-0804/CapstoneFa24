using Microsoft.AspNetCore.SignalR;

namespace HealthTrackingManageAPI.NewFolder.Chat
{
    public class ChatHub : Hub
    {
        public async Task SendMessageToClients(int chatId, string senderId, string messageContent)
        {
            await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", senderId, messageContent);
        }
    }
}