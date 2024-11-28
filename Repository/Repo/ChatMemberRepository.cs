using BusinessObject.Models;
using DataAccess;
using Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repo
{
    public class ChatMemberRepository : IChatMemberRepository
    {
        public Task CreateChatAsync(int memberId, string initialMessage)=> ChatDAO.Instance.CreateChatAsync(memberId, initialMessage);



        public Task<MessageChat> GetMemberChatDetailsAsync(int memberId, int chatId) => ChatDAO.Instance.GetMemberChatDetailsAsync(memberId, chatId);


        public Task<List<MessageChat>> GetMemberChatsAsync(int memberId) => ChatDAO.Instance.GetMemberChatsAsync(memberId);
        

        public Task RateChatAsync(int memberId, int chatId, double rating) => ChatDAO.Instance.RateChatAsync(memberId, chatId, rating);
       
    }
}
