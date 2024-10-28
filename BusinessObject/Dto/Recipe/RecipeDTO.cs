using BusinessObject.Dto.Recipe.CreateDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Recipe
{
    public class RecipeDTO
    {
        public int RecipeId { get; set; }
        public string RecipeName { get; set; }
        public string? Description { get; set; }
        public List<RecipeIngredientRequestDTO> RecipeIngredients { get; set; }
    }
}
