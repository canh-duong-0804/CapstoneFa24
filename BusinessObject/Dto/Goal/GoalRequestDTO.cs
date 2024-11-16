using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Goal
{
    public class GoalRequestDTO
    {
        
        public string GoalType { get; set; } = null!;
        public double Weight { get; set; }
        public int? ExerciseLevel { get; set; }
        public double TargetWeight { get; set; }
        public DateTime TargetDate { get; set; }
    }
}
