using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class ExerciseDiary
    {
        public ExerciseDiary()
        {
            ExerciseDiaryDetails = new HashSet<ExerciseDiaryDetail>();
        }

        public int ExerciseDiaryId { get; set; }
        public int MemberId { get; set; }
        public int? ExercisePlanId { get; set; }
        public DateTime? Date { get; set; }
        public int? TotalDuration { get; set; }
        public double? TotalCaloriesBurned { get; set; }


        public virtual ExercisePlan? ExercisePlan { get; set; }
        public virtual Member Member { get; set; } = null!;
        public virtual ICollection<ExerciseDiaryDetail> ExerciseDiaryDetails { get; set; }
    }
}
