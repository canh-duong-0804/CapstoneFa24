using BusinessObject.Dto.MainDashBoardTrainer;
using DataAccess;
using Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repo
{
    public class MainDashboardAdminRepository : IMainDashboardAdminRepository
    {
        public Task<MainDashBoardInfoForTrainerDTO> GetAllInformationForMainTrainer(DateTime SelectDate) => MainDashboardAdminDAO.Instance.GetAllInformationForMainTrainer(SelectDate);


    }
}
