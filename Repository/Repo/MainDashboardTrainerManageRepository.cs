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

        public Task<MainDashBoardInfoForTrainerExerciseDTO> GetMainDashBoardForExerciseTrainer(DateTime selectDate) => MainDashboardTrainerDAO.Instance.GetMainDashBoardForExerciseTrainer(selectDate);
        

        public Task<MainDashBoardInfoForTrainerFoodDTO> GetMainDashBoardForFoodTrainer(DateTime selectDate) => MainDashboardTrainerDAO.Instance.GetMainDashBoardForFoodTrainer(selectDate);
        
    }
}
