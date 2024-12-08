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
        public double? Weight { get; set; }
        public int? TypeExercise { get; set; }
        public string CategoryExercise { get; set; }
        public string? ExerciseImage { get; set; }
        public double? MetValue { get; set; }
        public string ExerciseName { get; set; } = null!;
       /* public GetExerciseDetailOfResitanceResponseDTO getExerciseDetailOfResitanceResponseDTO { get; set; }
        public GetExerciseDetailOfCardiorResponseDTO getExerciseDetailOfCardiorResponseDTO { get; set; }*/


    }
}
