using BusinessObject.Dto.MainDashBoardAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.MainDashBoardTrainer
{
    public class MainDashBoardInfoForTrainerDTO
    {
        public int TotalFoods { get; set; }
        public int TotalExercises { get; set; }
        public int TotalUsers { get; set; }
        public int TotalMemberUseEatClean { get; set; }
        public int TotalMemberUseLowCarb { get; set; }
        public int TotalMemberUseNormal { get; set; }
        public int TotalMemberUseVegetarian { get; set; }
        public double PercentageEatClean { get; set; }
        public double PercentageVegetarian { get; set; }
        public double PercentageLowCarb { get; set; }
        public double PercentageNormal { get; set; }


        public int NewUsersThisMonth { get; set; } // New user registrations in the selected month
        public List<TopFoodStatisticsDTO> TopFoodStatistics { get; set; } = new List<TopFoodStatisticsDTO>();
        public List<TopExeriseStatisticsDTO> TopExerciseStatistics { get; set; } = new List<TopExeriseStatisticsDTO>();
        public List<UserRegistrationStatisticsDTO> UserRegistrationStatistics { get; set; } = new List<UserRegistrationStatisticsDTO>();
    }
}
