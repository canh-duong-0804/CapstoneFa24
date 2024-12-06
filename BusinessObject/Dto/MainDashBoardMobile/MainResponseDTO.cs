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
        public double? TargetWeight { get; set; }
        public int streakNumberFood { get; set; }
        public int streakNumberExercise { get; set; }
        public double? CaloriesBurn { get; set; }
        public int? TotalDuration { get; set; }


        public double? Height { get; set; }
        public string Gender { get; set; }
        public int? ExerciseLevel { get; set; }
        public int AgeMember { get; set; }
        public int DiaryExerciseId { get; set; }
        public int DiaryFoodId { get; set; }



        public string? ImageMember { get; set; }
        public string TargetDate { get; set; }

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
