using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class Recipe
    {
        public Recipe()
        {
            RecipeIngredients = new HashSet<RecipeIngredient>();
        }

        public int RecipeId { get; set; }
        public int FoodId { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string RecipeImage { get; set; } = null!;
        public int? ChangeBy { get; set; }
        public DateTime? ChangeDate { get; set; }
        public string RecipeName { get; set; } = null!;
        public string? Description { get; set; }
        public string Instructions { get; set; } = null!;
        public int? DifficultyOfRecipe { get; set; }
        public int? PrepTime { get; set; }
        public int? CookTime { get; set; }
        public int? Servings { get; set; }
        public bool? Status { get; set; }

        public virtual staff CreateByNavigation { get; set; } = null!;
        public virtual Food Food { get; set; } = null!;
        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; }
    }
}
