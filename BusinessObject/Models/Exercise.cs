﻿using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class Exercise
    {
        public Exercise()
        {
            ExerciseDiaries = new HashSet<ExerciseDiary>();
            ExercisePlanDetails = new HashSet<ExercisePlanDetail>();
        }

        public int ExerciseId { get; set; }
        public int ExerciseCategoryId { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? ExerciseLevel { get; set; }
        public int? ChangeBy { get; set; }
        public int? Reps { get; set; }
        public int? Sets { get; set; }
        public int? Minutes { get; set; }
        public string? ExerciseImage { get; set; }
        public DateTime? ChangeDate { get; set; }
        public string ExerciseName { get; set; } = null!;
        public string? Description { get; set; }
        public double CaloriesPerHour { get; set; }
        public bool? Status { get; set; }

        public virtual staff CreateByNavigation { get; set; } = null!;
        public virtual ExerciseCategory ExerciseCategory { get; set; } = null!;
        public virtual ICollection<ExerciseDiary> ExerciseDiaries { get; set; }
        public virtual ICollection<ExercisePlanDetail> ExercisePlanDetails { get; set; }
    }
}
