using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class ExerciseOther
    {
        public int ExerciseDetailId { get; set; }
        public int ExerciseId { get; set; }
        public string? MetricsOther { get; set; }
        public int? Duration { get; set; }

        public virtual Exercise Exercise { get; set; } = null!;
    }
}
