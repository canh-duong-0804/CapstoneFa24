using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Ingredient
{
    public class CreateIngredientRequestDTO
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
