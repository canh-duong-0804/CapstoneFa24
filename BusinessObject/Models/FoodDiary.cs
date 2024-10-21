using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class FoodDiary
    {
        public FoodDiary()
        {
            FoodDiaryDetails = new HashSet<FoodDiaryDetail>();
        }

        public int DiaryId { get; set; }
        public int MemberId { get; set; }
        public int? MealPlanId { get; set; }
        public DateTime Date { get; set; }
        public double? GoalCalories { get; set; }
        public double? Calories { get; set; }
        public double? Protein { get; set; }
        public double? Fat { get; set; }
        public double? Carbs { get; set; }

        public virtual MealPlan? MealPlan { get; set; }
        public virtual Member Member { get; set; } = null!;
        public virtual ICollection<FoodDiaryDetail> FoodDiaryDetails { get; set; }
    }
}
