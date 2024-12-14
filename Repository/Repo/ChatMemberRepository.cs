using BusinessObject.Dto.MessageChatDetail;
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
        public Task CreateChatAsync(int memberId, string createNewChat)=> ChatDAO.Instance.CreateChatAsync(memberId, createNewChat);

        public Task<bool> EndChatsAsync(int memberId) => ChatDAO.Instance.EndChatsAsync(memberId);


        public Task<List<GetMessageChatDetailDTO>> GetMemberChatDetailsAsync(int memberId, int chatId) => ChatDAO.Instance.GetMemberChatDetailsAsync(memberId, chatId);


        public Task<List<MessageChat>> GetMemberChatsAsync(int memberId) => ChatDAO.Instance.GetMemberChatsAsync(memberId);
        

        public Task RateChatAsync(int memberId, int chatId, int rating) => ChatDAO.Instance.RateChatAsync(memberId, chatId, rating);

        public Task SendMessageMemberAsync(int memberId, int chatId, string messageContent) => ChatDAO.Instance.SendMessageMemberAsync(memberId, chatId, messageContent);

        
    }
}
