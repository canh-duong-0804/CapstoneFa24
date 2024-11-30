using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.ExerciseTrainer
{
    public class CreateExerciseRequestDTO
    {
        public string ExerciseName { get; set; } = null!;
        public string? Description { get; set; }
       
        public string? ExerciseImage { get; set; }
        public int TypeExercise { get; set; }
        public double? MetValue { get; set; }
        public CreateExerciseResistanceDTO? ResistanceMetrics { get; set; }
        public CreateExerciseCardioDTO? CardioMetrics { get; set; }
    }

    public class CreateExerciseResistanceDTO
    {
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

    public class CreateExerciseCardioDTO
    {
        public int? Minutes1 { get; set; }
        public int? Minutes2 { get; set; }
        public int? Minutes3 { get; set; }
        public double? Calories1 { get; set; }
        public double? Calories2 { get; set; }
        public double? Calories3 { get; set; }
        
    }

}
