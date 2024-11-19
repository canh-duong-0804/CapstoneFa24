using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Exericse
{
    public class GetAllExerciseForMember
    {
        public int ExerciseId { get; set; }
      
        public string CategoryExercise { get; set; }
        public string? ExerciseImage { get; set; }
     
        public string ExerciseName { get; set; } = null!;
      
      
    }
}
