using DataAccess;
using Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repo
{
    public class AdminChatRepository : IAdminChatRepository
    {
        public Task AssignStaffToChatAsync(int chatId, int staffId) => ChatDAO.Instance.AssignStaffToChatAsync(chatId, staffId);
        

        public Task SendMessageAsync(int chatId, int staffId, string messageContent) => ChatDAO.Instance.SendMessageAsync(chatId, staffId, messageContent);
       
    }
}
