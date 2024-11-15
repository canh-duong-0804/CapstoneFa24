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
     
        public string GoalType { get; set; } = null!;
        public string ExerciseLevel { get; set; } = null!;
        public double WeightGoal { get; set; }


        public DateTime TargetDate { get; set; }
        public double? CurrentWeight { get; set; }
        public double? startWeight { get; set; }
          public DateTime? DateInitial { get; set; }
    }
}
