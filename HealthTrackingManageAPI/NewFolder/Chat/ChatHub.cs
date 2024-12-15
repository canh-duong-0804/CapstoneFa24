using Microsoft.AspNetCore.SignalR;

namespace HealthTrackingManageAPI.NewFolder.Chat
{
    public class ChatHub : Hub
    {
        public async Task JoinChat(string chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        }

        // Phương thức để rời khỏi chat room 
        public async Task LeaveChat(string chatId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
        }

        // Gửi tin nhắn trong chat
        public async Task SendMessage(string chatId, string senderId, string message)
        {
            await Clients.Group(chatId).SendAsync("ReceiveMessage", new
            {
                SenderId = senderId,
                Message = message,
                Timestamp = DateTime.Now
            });
        }
    }
}
