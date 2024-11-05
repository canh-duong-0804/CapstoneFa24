using BusinessObject.Dto.MainDashBoardMobile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IMainDashBoardRepository
    {
        Task<MainDashResponseDTO> GetFoodDairyDetailById(int memberId, DateTime date);
        Task<MainDashResponseDTO> GetMainDashBoardForMemberById(int id, DateTime date);
    }
}
