using BusinessObject.Dto.MainDashBoardMobile;
using DataAccess;
using Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repo
{
    public class MainDashBoardRepository : IMainDashBoardRepository
    {
        public Task<MainDashBoardMobileForMemberResponseDTO> GetMainDashBoardForMemberById(int id) => MainDashBoardMobileDAO.Instance.GetMainDashBoardForMemberById(id);


    }
}
