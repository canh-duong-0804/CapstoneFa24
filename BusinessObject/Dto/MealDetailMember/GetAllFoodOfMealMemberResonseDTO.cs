using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.MealDetailMember
{
    public class GetAllFoodOfMealMemberResonseDTO
    {

        public int FoodId { get; set; }
        public string FoodName { get; set; }
       
        public string? FoodImage { get; set; }
        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Carbs { get; set; }
        public double Fat { get; set; }
        public string DietName { get; set; }
        public double? Quantity { get; set; }
    }
}
