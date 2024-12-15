using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.ExecrisePlan
{
    public class ExercisePlanDTO
    {
        public int ExercisePlanId { get; set; }
        public int TotalDay { get; set; }
        public string ExercisePlanImage { get; set; }
        public string Name { get; set; }
        public double TotalCaloriesBurned { get; set; }
        public double AvgDuration { get; set; }
        public List<ExercisePlanDetailDTO> Details { get; set; } = new();
    }

    public class ExercisePlanDetailDTO
    {
        public int ExercisePlanDetailId { get; set; }
        public int ExerciseId { get; set; }
        public string ExerciseName { get; set; } // Optional for details response
        public byte Day { get; set; }
        public int Duration { get; set; }
    }

    public class AddExercisePlanDTO
    {
        public string Name { get; set; }
        public double TotalCaloriesBurned { get; set; }

        public string ExercisePlanImage { get; set; }
        public List<AddExercisePlanDetailDTO> Details { get; set; } = new();
    }

    public class AddExercisePlanDetailDTO
    {
        public int ExerciseId { get; set; }
        public byte Day { get; set; }
        public int Duration { get; set; }
    }
}
