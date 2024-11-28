using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Exericse
{
    public class GetExerciseDetailOfCardiorResponseDTO
    {
        public int ExerciseId { get; set; }
    
        public int? TypeExercise { get; set; }
        public string? ExerciseImage { get; set; }
        public string CategoryExercise { get; set; }
        public string ExerciseName { get; set; } = null!;
        public string? Description { get; set; }
        /* public int Minutes1 { get; set; }
         public int Minutes2 { get; set; }
         public int Minutes3 { get; set; }
         public double Calories1 { get; set; }
         public double Calories2 { get; set; }
         public double Calories3 { get; set; }*/

        public string? MetricsCardio { get; set; }
        public double MetValue { get; set; }

    }
}
