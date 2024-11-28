using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.ExerciseTrainer
{
    public class ExerciseRequestDTO
    {
        public int ExerciseId { get; set; }
        public string? ExerciseName { get; set; }
        public string? Description { get; set; }
        public string? ExerciseImage { get; set; }
        public int? TypeExercise { get; set; }
        public double? MetValue { get; set; }
        public UpdateExerciseResistanceDTO? ResistanceMetrics { get; set; }
        public UpdateExerciseCardioDTO? CardioMetrics { get; set; }
       
    }

    public class UpdateExerciseResistanceDTO
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

    public class UpdateExerciseCardioDTO
    {
        public int? Minutes1 { get; set; }
        public int? Minutes2 { get; set; }
        public int? Minutes3 { get; set; }
        public double? Calories1 { get; set; }
        public double? Calories2 { get; set; }
        public double? Calories3 { get; set; }
    }
}
