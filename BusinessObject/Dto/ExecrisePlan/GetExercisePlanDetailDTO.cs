using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.ExecrisePlan
{
    public class GetExercisePlanDetailDTO
    {
        public int ExercisePlanId { get; set; }
        public byte Day { get; set; }
        public List<DayExerciseDTO> listExercise { get; set; } = new List<DayExerciseDTO>();
    }

    public class DayExerciseDTO
    {

      


        public int ExerciseId { get; set; }

        public int Duration { get; set; }

        public string ExerciseName { get; set; } = null!;
        

    }
}
