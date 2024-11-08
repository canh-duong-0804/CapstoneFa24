using BusinessObject.Dto.Food;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.MealDetailMember
{
    public class MealMemberDetailResonseDTO
    {
        public int MealPlanId { get; set; }
       
        public string? ImageMealMember { get; set; }
        public string NameMealPlanMember { get; set; } = null!;
        public double? TotalCalories { get; set; }
        public double? TotalProtein { get; set; }
        public double? TotalCarb { get; set; }
        public double? TotalFat { get; set; }
        public DateTime? MealDate { get; set; }

       // public int? Quantity { get; set; }




        public List<GetAllFoodOfMealMemberResonseDTO> FoodDetails { get; set; } = new List<GetAllFoodOfMealMemberResonseDTO>();








    }
}
