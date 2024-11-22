using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.MainDashBoardMobile
{
    public class MainResponseDTO
    {   
        public string FullName { get; set; } = null!;
        public double totalCalories { get; set; }
        public double BMI { get; set; }
        public double totalProtein { get; set; }
        public double totalCarb { get; set; }
        public double totalFat { get; set; }
        public double? Weight { get; set; }
        public int streakNumber { get; set; }
        public double CaloriesBurn { get; set; }

        //public DateTime? TargetDate { get; set; }

        public string SelectDate { get; set; }
        public string GoalType { get; set; }
        public double WeightDifference { get; set; }




        public double? CaloriesIntake { get; set; }
        public double AmountWater { get; set; }

        public double? ProteinIntake { get; set; }
        public double? FatIntake { get; set; }
        public double? CarbsIntake { get; set; }






    }
}
