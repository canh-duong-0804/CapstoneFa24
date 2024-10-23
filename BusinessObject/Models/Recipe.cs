using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class Recipe
    {
        public int RecipeId { get; set; }
        public int FoodId { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? ChangeBy { get; set; }
        public DateTime? ChangeDate { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string Instructions { get; set; } = null!;
        public int? PrepTime { get; set; }
        public int? CookTime { get; set; }
        public int? Servings { get; set; }
        public bool? Status { get; set; }

        public virtual staff CreateByNavigation { get; set; } = null!;
        public virtual Food Food { get; set; } = null!;
    }
}
