using System;
using System.Collections.Generic;

namespace BusinessObject.Entities
{
    public partial class MealPlanDetail
    {
        public int MealPlanDetailId { get; set; }
        public int MealPlanId { get; set; }
        public DateTime MealDate { get; set; }
        public string? MealType { get; set; }
        public string? Description { get; set; }
        public int FoodId { get; set; }
        public int Quantity { get; set; }

        public virtual Food Food { get; set; } = null!;
        public virtual MealPlan MealPlan { get; set; } = null!;
    }
}
