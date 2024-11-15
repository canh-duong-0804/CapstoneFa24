using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class ExerciseDiaryDetail
    {
        public int ExerciseDiaryDetailsId { get; set; }
        public int? ExerciseDiaryId { get; set; }
        public bool? IsPractice { get; set; }
        public int ExerciseId { get; set; }
        public int? Duration { get; set; }
        public double? CaloriesBurned { get; set; }

        public virtual Exercise Exercise { get; set; } = null!;
        public virtual ExerciseDiary? ExerciseDiary { get; set; }
    }
}
