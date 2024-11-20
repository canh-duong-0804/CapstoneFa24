using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Goal
{
    public class GoalResponseDTO
    {
        public int GoalId { get; set; }
     
        public float GoalType { get; set; }
        public int? ExerciseLevel { get; set; } = null!;
        public double WeightGoal { get; set; }


        public string TargetDate { get; set; }
        public double? CurrentWeight { get; set; }
        public double? startWeight { get; set; }
          public string? DateInitial { get; set; }
    }
}
