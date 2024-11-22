using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.ExecrisePlan
{
    public class UpdateExercisePlanDetailRequestDTO
    {
        public int ExercisePlanDetailId { get; set; }
        public int ExercisePlanId { get; set; }
        public int ExerciseId { get; set; }
        public byte Day { get; set; }
        public int Duration { get; set; }
    }
}
