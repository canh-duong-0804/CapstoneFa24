using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Recipe.CreateDTO
{
    public class RecipeIngredientRequestDTO
    {
     
        
        public int IngredientId { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; } = null!;
        //public double? CaloriesPerUnit { get; set; }

        
    }
}
