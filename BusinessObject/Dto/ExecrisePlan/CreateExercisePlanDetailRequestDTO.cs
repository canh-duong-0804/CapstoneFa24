using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.ExecrisePlan
{
    public class CreateExercisePlanDetailRequestDTO
    {
        public int ExercisePlanId { get; set; }
        public int ExerciseId { get; set; }
        public byte Day { get; set; } // 1 = Monday, 2 = Tuesday, etc.
        public int Duration { get; set; }
    }
}
