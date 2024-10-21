using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class MealPlan
    {
        public MealPlan()
        {
            FoodDiaries = new HashSet<FoodDiary>();
            MealPlanDetails = new HashSet<MealPlanDetail>();
        }

        public int MealPlanId { get; set; }
        public int CreateBy { get; set; }
        public string? MealPlanImage { get; set; }
        public DateTime? ChangeDate { get; set; }
        public int? ChangeBy { get; set; }
        public int? DietId { get; set; }
        public string Name { get; set; } = null!;
        public double TotalCalories { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool? Status { get; set; }

        public virtual staff CreateByNavigation { get; set; } = null!;
        public virtual Diet? Diet { get; set; }
        public virtual ICollection<FoodDiary> FoodDiaries { get; set; }
        public virtual ICollection<MealPlanDetail> MealPlanDetails { get; set; }
    }
}
