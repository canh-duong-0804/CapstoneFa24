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
        public bool? IsCardio { get; set; }
        public UpdateExerciseResistanceDTO? ResistanceMetrics { get; set; }
        public UpdateExerciseCardioDTO? CardioMetrics { get; set; }
    }

    public class UpdateExerciseResistanceDTO
    {
        public string? MetricsResistance { get; set; }
    }

    public class UpdateExerciseCardioDTO
    {
        public string? MetricsCardio { get; set; }
        public double? MetValue { get; set; }
    }
}
