using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class MealPlanMember
    {
        public MealPlanMember()
        {
            MealsPlanMemberDetails = new HashSet<MealsMemberDetail>();
        }

        public int MealPlanId { get; set; }
        public int MemberId { get; set; }
        public string? Image { get; set; }
        public string NameMealPlanMember { get; set; } = null!;
        public int? TotalCalories { get; set; }
        public double? TotalProtein { get; set; }
        public double? TotalCarb { get; set; }
        public double? TotalFat { get; set; }
        public DateTime? MealDate { get; set; }

        public virtual Member Member { get; set; } = null!;
        public virtual ICollection<MealsMemberDetail> MealsPlanMemberDetails { get; set; }
    }
}
