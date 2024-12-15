using Microsoft.AspNetCore.SignalR;

namespace HealthTrackingManageAPI.NewFolder.Chat
{
    public class ChatHub : Hub
    {
        public enum UserType
        {
            Member,
            Trainer
        }

        // Method to send message with user type
        public async Task SendMessageToClients(
            int chatId,
            string senderId,
            string messageContent,
            UserType senderType)
        {
            await Clients.Group(chatId.ToString()).SendAsync(
                "ReceiveMessage",
                new
                {
                    SenderId = senderId,
                    MessageContent = messageContent,
                    SenderType = senderType,
                    Timestamp = DateTime.UtcNow
                }
            );
        }

        // Method to join a specific chat room
        public async Task JoinChat(int chatId, string userId, UserType userType)
        {
            // Create a unique group name that includes both chat ID and user type
            string groupName = $"{chatId}_{userType}";
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
