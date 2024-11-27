using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class ExerciseCardio
    {
        public int ExerciseDetailId { get; set; }
        public int ExerciseId { get; set; }
        public string? MetricsCardio { get; set; }
        public double? MetValue { get; set; }

        public virtual Exercise Exercise { get; set; } = null!;
    }
}
