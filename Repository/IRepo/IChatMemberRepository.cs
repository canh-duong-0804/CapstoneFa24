using BusinessObject.Dto.MessageChatDetail;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IChatMemberRepository
    {
        Task CreateChatAsync(int memberId, string createNewChat);
        Task<List<MessageChat>> GetMemberChatsAsync(int memberId);
        Task<List<GetMessageChatDetailDTO>> GetMemberChatDetailsAsync(int memberId, int chatId);
        Task RateChatAsync(int memberId, int chatId, int rating);
        Task SendMessageMemberAsync(int memberId, int chatId, string messageContent);
        Task<bool> EndChatsAsync(int memberId);
    }
}
