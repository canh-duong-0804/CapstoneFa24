using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class Exercise
    {
        public Exercise()
        {
            ExerciseCardios = new HashSet<ExerciseCardio>();
            ExerciseDiaryDetails = new HashSet<ExerciseDiaryDetail>();
            ExercisePlanDetails = new HashSet<ExercisePlanDetail>();
            ExerciseResistances = new HashSet<ExerciseResistance>();
        }

        public int ExerciseId { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? ChangeBy { get; set; }
        public bool? IsCardio { get; set; }
        public string? ExerciseImage { get; set; }
        public DateTime? ChangeDate { get; set; }
        public string ExerciseName { get; set; } = null!;
        public string? Description { get; set; }
        public bool? Status { get; set; }

        public virtual staff CreateByNavigation { get; set; } = null!;
        public virtual ICollection<ExerciseCardio> ExerciseCardios { get; set; }
        public virtual ICollection<ExerciseDiaryDetail> ExerciseDiaryDetails { get; set; }
        public virtual ICollection<ExercisePlanDetail> ExercisePlanDetails { get; set; }
        public virtual ICollection<ExerciseResistance> ExerciseResistances { get; set; }
    }
}
