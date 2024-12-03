using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Exericse
{
    public class GetExerciseDetailOfOtherResponseDTO
    {
        public int ExerciseId { get; set; }
       
        public int? TypeExercise { get; set; }
        public string? ExerciseImage { get; set; }
      
        public string ExerciseName { get; set; } = null!;
        public string? Description { get; set; }
        public double? MetValue { get; set; }
        public string CategoryExercise { get; set; }
    }
}
