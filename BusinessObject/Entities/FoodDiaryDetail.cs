using System;
using System.Collections.Generic;

namespace BusinessObject.Entities
{
    public partial class FoodDiaryDetail
    {
        public int DiaryDetailId { get; set; }
        public int DiaryId { get; set; }
        public int FoodId { get; set; }
        public double Quantity { get; set; }
        public int MealType { get; set; }
        public bool? StatusFoodDiary { get; set; }

        public virtual FoodDiary Diary { get; set; } = null!;
        public virtual Food Food { get; set; } = null!;
    }
}
