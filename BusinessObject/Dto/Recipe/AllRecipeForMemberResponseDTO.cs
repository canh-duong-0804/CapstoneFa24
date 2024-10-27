using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Recipe
{
    public class AllRecipeForMemberResponseDTO
    {
        public int RecipeId { get; set; }
        public string FoodName { get; set; }
        public string RecipeImage { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public string? ChangeBy { get; set; }
        public DateTime? ChangeDate { get; set; }
        public string RecipeName { get; set; } = null!;
    }
}
