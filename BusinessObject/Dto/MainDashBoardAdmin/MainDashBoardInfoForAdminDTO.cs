using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.MainDashBoardTrainer
{
    public class MainDashBoardInfoForAdminDTO
    {
        public int TotalFoods { get; set; }
        public int TotalExercises { get; set; }
        public int TotalUsers { get; set; }
        public int NewUsersThisMonth { get; set; } // New user registrations in the selected month
        public List<TopFoodStatisticsDTO> TopFoodStatistics { get; set; } = new List<TopFoodStatisticsDTO>();
        public List<UserRegistrationStatisticsDTO> UserRegistrationStatistics { get; set; } = new List<UserRegistrationStatisticsDTO>();
    }

}
