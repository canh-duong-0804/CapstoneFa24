using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class MealMember
    {
        public MealMember()
        {
            MealMemberDetails = new HashSet<MealMemberDetail>();
        }

        public int MealMemberId { get; set; }
        public int MemberId { get; set; }
        public string? Image { get; set; }
        public string NameMealMember { get; set; } = null!;
        public double? TotalCalories { get; set; }
        public double? TotalProtein { get; set; }
        public double? TotalCarb { get; set; }
        public double? TotalFat { get; set; }
        public DateTime? MealDate { get; set; }

        public virtual Member Member { get; set; } = null!;
        public virtual ICollection<MealMemberDetail> MealMemberDetails { get; set; }
    }
}
