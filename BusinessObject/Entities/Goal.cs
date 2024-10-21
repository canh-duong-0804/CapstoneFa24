using System;
using System.Collections.Generic;

namespace BusinessObject.Entities
{
    public partial class Goal
    {
        public int GoalId { get; set; }
        public int MemberId { get; set; }
        public string GoalType { get; set; } = null!;
        public double TargetValue { get; set; }
        public DateTime TargetDate { get; set; }

        public virtual Member Member { get; set; } = null!;
    }
}
