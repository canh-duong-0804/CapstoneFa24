using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class ExerciseCardio
    {
        public int ExerciseDetailId { get; set; }
        public int ExerciseId { get; set; }
        public int? ExerciseLevel { get; set; }
        public double? Calories { get; set; }
        public int? Minutes { get; set; }

        public virtual Exercise Exercise { get; set; } = null!;
    }
}
