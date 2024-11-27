using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class ExerciseResistance
    {
        public int ExerciseDetailId { get; set; }
        public int ExerciseId { get; set; }
        public string? MetricsResistance { get; set; }

        public virtual Exercise Exercise { get; set; } = null!;
    }
}
