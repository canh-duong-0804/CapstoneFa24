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
        public bool IsCardio { get; set; } 
        public CreateExerciseResistanceDTO? ResistanceMetrics { get; set; }
        public CreateExerciseCardioDTO? CardioMetrics { get; set; }
    }

    public class CreateExerciseResistanceDTO
    {
        public string? MetricsResistance { get; set; }
    }

    public class CreateExerciseCardioDTO
    {
        public string? MetricsCardio { get; set; }
        public double? MetValue { get; set; }
    }

}
