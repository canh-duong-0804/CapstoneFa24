using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.MainDashBoardMobile
{
    public class MainDashBoardMobileForMemberResponseDTO
    {
        public string DateMainDashBoard {  get; set; }
        public double DailyCalories { get; set; }
        public double BMI { get; set; }
        public string? ImageMember { get; set; }
        public double ProteinInGrams { get; set; }
        public double CarbsInGrams { get; set; }
        public double FatInGrams { get; set; }
        public double? Weight { get; set; }
        public double? TargetWeight { get; set; }

        public string TargetDate { get; set; }
        public string GoalType { get; set; }
        public string UserName { get; set; }
        public double WeightDifference { get; set; }

        public double? Height { get; set; }
        public string Gender { get; set; }
        public int? ExerciseLevel { get; set; }
        public int AgeMember { get; set; }
        public int DiaryExerciseId { get; set; }
        public int DiaryFoodId { get; set; }
        public double? CaloriesBurn { get; set; }

        public int? TotalDuration { get; set; }
    }
}
