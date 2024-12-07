using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Goal
{
    public class AddOnlyGoalMemberRequestDTO
    {
       
        public int? ExerciseLevel { get; set; }
        public double TargetWeight { get; set; }

        public float GoalType { get; set; }
    }
}
