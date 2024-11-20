using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Exericse
{
    public class GetExerciseDetailOfResitanceResponseDTO
    {
        public int ExerciseId { get; set; }

        public bool? IsCardio { get; set; }
        public string? ExerciseImage { get; set; }
        public string CategoryExercise { get; set; }
        public string ExerciseName { get; set; } = null!;
        public string? Description { get; set; }
        public int? Reps1 { get; set; }
        public int? Reps2 { get; set; }
        public int? Reps3 { get; set; }
        public int? Sets1 { get; set; }
        public int? Sets2 { get; set; }
        public int? Sets3 { get; set; }
        public int? Minutes1 { get; set; }
        public int? Minutes2 { get; set; }
        public int? Minutes3 { get; set; }

    }
}
