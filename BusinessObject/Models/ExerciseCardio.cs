using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class ExerciseCardio
    {
        public int ExerciseDetailId { get; set; }
        public int ExerciseId { get; set; }
        public double? Calories { get; set; }
        public int? Minutes1 { get; set; }
        public int? Minutes2 { get; set; }
        public int? Minutes3 { get; set; }
        public double? Calorines1 { get; set; }
        public double? Calorines2 { get; set; }
        public double? Calorines3 { get; set; }
        public double? MetValue { get; set; }

        public virtual Exercise Exercise { get; set; } = null!;
    }
}
