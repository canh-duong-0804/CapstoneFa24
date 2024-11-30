using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Nutrition
{
    public class MacroNutrientDto
    {
        public double TotalCarbs { get; set; }
        public double TotalFat { get; set; }
        public double TotalProtein { get; set; }
        public FoodMacroDto HighestCarbFood { get; set; }
        public FoodMacroDto HighestFatFood { get; set; }
        public FoodMacroDto HighestProteinFood { get; set; }
    }

    public class FoodMacroDto
    {
        public string FoodName { get; set; }
        public double Quantity { get; set; }
        public double MacroValue { get; set; }
    }

}
