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
        Task<MainDashBoardMobileForMemberResponseDTO> GetMainDashBoardForMemberById(int id);
    }
}
