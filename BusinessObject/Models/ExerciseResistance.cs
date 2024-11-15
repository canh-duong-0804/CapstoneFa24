using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class ExerciseResistance
    {
        public int ExerciseDetailId { get; set; }
        public int ExerciseId { get; set; }
        public int? Reps1 { get; set; }
        public int? Reps2 { get; set; }
        public int? Reps3 { get; set; }
        public double? MetValue { get; set; }
        public int? Sets1 { get; set; }
        public int? Sets2 { get; set; }
        public int? Sets3 { get; set; }
        public int? Minutes { get; set; }
        public double? Calories { get; set; }

        public virtual Exercise Exercise { get; set; } = null!;
    }
}
