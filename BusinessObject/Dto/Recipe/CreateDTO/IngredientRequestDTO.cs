using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Recipe.CreateDTO
{
    public class IngredientRequestDTO
    {
        public int IngredientId { get; set; }
        public string Name { get; set; } = null!;
       
    }
}
