using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class Food
    {
        public Food()
        {
            FoodDiaryDetails = new HashSet<FoodDiaryDetail>();
            MealPlanDetails = new HashSet<MealPlanDetail>();
            Recipes = new HashSet<Recipe>();
            Tags = new HashSet<Tag>();
        }

        public int FoodId { get; set; }
        public string Name { get; set; } = null!;
        public string Portion { get; set; } = null!;
        public double Calories { get; set; }
        public int CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? ChangeBy { get; set; }
        public DateTime ChangeDate { get; set; }
        public string? FoodImage { get; set; }
        public double Protein { get; set; }
        public double Carbs { get; set; }
        public double Fat { get; set; }
        public double VitaminA { get; set; }
        public double VitaminC { get; set; }
        public double VitaminD { get; set; }
        public double VitaminE { get; set; }
        public double VitaminB1 { get; set; }
        public double VitaminB2 { get; set; }
        public double VitaminB3 { get; set; }
        public string? Diet { get; set; }

        public virtual staff CreateByNavigation { get; set; } = null!;
        public virtual ICollection<FoodDiaryDetail> FoodDiaryDetails { get; set; }
        public virtual ICollection<MealPlanDetail> MealPlanDetails { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
    }
}
