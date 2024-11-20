using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class ExercisePlan
    {
        public ExercisePlan()
        {
            ExerciseDiaries = new HashSet<ExerciseDiary>();
            ExercisePlanDetails = new HashSet<ExercisePlanDetail>();
        }

        public int ExercisePlanId { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? ChangeBy { get; set; }
        public DateTime? ChangeDate { get; set; }
        public string Name { get; set; } = null!;
        public double TotalCaloriesBurned { get; set; }
        public string? ExercisePlanImage { get; set; }

        public virtual staff CreateByNavigation { get; set; } = null!;
        public virtual ICollection<ExerciseDiary> ExerciseDiaries { get; set; }
        public virtual ICollection<ExercisePlanDetail> ExercisePlanDetails { get; set; }
    }
}
