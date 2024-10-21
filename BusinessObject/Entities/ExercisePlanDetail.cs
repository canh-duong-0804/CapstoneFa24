using System;
using System.Collections.Generic;

namespace BusinessObject.Entities
{
    public partial class ExercisePlanDetail
    {
        public int ExercisePlanDetailId { get; set; }
        public int ExercisePlanId { get; set; }
        public int ExerciseId { get; set; }
        public int Duration { get; set; }

        public virtual Exercise Exercise { get; set; } = null!;
        public virtual ExercisePlan ExercisePlan { get; set; } = null!;
    }
}
