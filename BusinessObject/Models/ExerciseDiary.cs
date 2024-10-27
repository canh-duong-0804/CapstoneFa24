﻿using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class ExerciseDiary
    {
        public int ExerciseDiaryId { get; set; }
        public int MemberId { get; set; }
        public int ExercisePlanId { get; set; }
        public int ExerciseId { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        public double CaloriesBurned { get; set; }

        public virtual Exercise Exercise { get; set; } = null!;
        public virtual ExercisePlan ExercisePlan { get; set; } = null!;
        public virtual Member Member { get; set; } = null!;
    }
}