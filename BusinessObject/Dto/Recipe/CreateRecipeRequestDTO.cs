using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Recipe
{
    public class CreateRecipeRequestDTO
    {
        public int FoodId { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string RecipeImage { get; set; } = null!;
        
        public string RecipeName { get; set; } = null!;
        public string? Description { get; set; }
        public string Instructions { get; set; } = null!;
        public int? PrepTime { get; set; }
        public int? CookTime { get; set; }
        public int? Servings { get; set; }
       
    }
}
