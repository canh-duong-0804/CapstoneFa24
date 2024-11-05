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
        public Task<MainDashResponseDTO> GetFoodDairyDetailById(int memberId, DateTime date) => MainDashBoardMobileDAO.Instance.GetFoodDairyDetailById(memberId, date);


        public Task<MainDashBoardMobileForMemberResponseDTO> GetMainDashBoardForMemberById(int id, DateTime date) => MainDashBoardMobileDAO.Instance.GetMainDashBoardForMemberById(id,date);


    }
}
