using BusinessObject.Dto.Chat;
using BusinessObject.Dto.MessageChatDetail;
using BusinessObject.Dto.Trainer;
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
    public class AdminChatRepository : IAdminChatRepository
    {
        public Task AssignStaffToChatAsync(int chatId, int staffId) => ChatDAO.Instance.AssignStaffToChatAsync(chatId, staffId);

        public Task<PagedResult<AllMessageChatDTO>> GetAllMessageChatForTrainerNeedAsign(int pageNumber, int pageSize) => ChatDAO.Instance.GetAllMessageChatForTrainerNeedAsign(pageNumber,pageSize);


        public Task<List<GetMessageChatDetailDTO>> GetAllMessageForTrainerToAsign(int chatId, int staffId) => ChatDAO.Instance.GetAllMessageForTrainerToAsign(chatId, staffId);

        public Task<List<GetAllAccountTrainer>> GetAllTrainerToAssign() => ChatDAO.Instance.GetAllTrainerToAssign();

        public Task<List<OverViewMessageDTO>> OverviewAllMessageOfTrainer(int staffId) => ChatDAO.Instance.OverviewAllMessageOfTrainer(staffId);


        public Task SendMessageAsync(int chatId, int staffId, string messageContent) => ChatDAO.Instance.SendMessageAsync(chatId, staffId, messageContent);
       
    }
}
