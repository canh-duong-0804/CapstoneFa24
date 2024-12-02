using BusinessObject.Dto.MainDashBoardTrainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IMainDashboardTrainerRepository
    {
        Task<MainDashBoardInfoForTrainerDTO> GetAllInformationForMainTrainer(DateTime selectDate);
    }
}
