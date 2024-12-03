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
    public class MainDashboardTrainerManageRepository : IMainDashboardTrainerManageRepository
    {
        public Task<MainDashBoardInfoForTrainerDTO> GetAllInformationForMainTrainer(DateTime selectDate) => MainDashboardTrainerDAO.Instance.GetAllInformationForMainTrainer(selectDate);


    }
}
