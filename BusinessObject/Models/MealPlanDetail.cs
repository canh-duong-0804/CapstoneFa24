using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class MealPlanDetail
    {
        public int MealPlanDetailId { get; set; }
        public int MealPlanId { get; set; }
        public DateTime MealDate { get; set; }
        public int MealType { get; set; }
        public byte Day { get; set; }
        public string? Description { get; set; }
        public int FoodId { get; set; }
        public bool? Status { get; set; }
        public int Quantity { get; set; }

        public virtual Food Food { get; set; } = null!;
        public virtual MealPlan MealPlan { get; set; } = null!;
    }
}
