using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class MealsMemberDetail
    {
        public int DetailId { get; set; }
        public int MealPlanId { get; set; }
        public int MemberId { get; set; }
        public int? Calories { get; set; }
        public double? Protein { get; set; }
        public double? Carb { get; set; }
        public double? Fat { get; set; }

        public virtual MealPlanMember MealPlan { get; set; } = null!;
    }
}
