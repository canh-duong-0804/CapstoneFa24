using BusinessObject.Dto.MessageChatDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IAdminChatRepository
    {
        Task AssignStaffToChatAsync(int chatId, int staffId);
        Task<PagedResult<AllMessageChatDTO>> GetAllMessageChatForTrainerNeedAsign(int pageNumber, int pageSize);
        Task<List<GetMessageChatDetailDTO>> GetAllMessageForTrainerToAsign(int chatId, int staffId);
        Task SendMessageAsync(int chatId, int staffId, string messageContent);
    }
}
