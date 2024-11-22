using BusinessObject.Dto.MealDetailMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.MealMember
{
    public class CopyPreviousMealRequestDTO
    {
            public double? TotalCalories { get; set; }
            public double? TotalProtein { get; set; }
            public double? TotalCarb { get; set; }
            public double? TotalFat { get; set; }
            

             //public int? Quantity { get; set; }



       public DateTime getDatePrevious {  get; set; }

            public List<GetAllFoodOfMealMemberResonseDTO> FoodDetails { get; set; } = new List<GetAllFoodOfMealMemberResonseDTO>();

    }
}
