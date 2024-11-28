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
        Task<MessageChat> CreateChatAsync(int memberId, string initialMessage);
        Task<List<MessageChat>> GetMemberChatsAsync(int memberId);
        Task<MessageChat> GetMemberChatDetailsAsync(int memberId, int chatId);
        Task RateChatAsync(int memberId, int chatId, double rating);
    }
}
