using BusinessObject.Dto.MealDetailMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.MealPlan
{
    public class MealPlanDetailResponseDTO
    {
        public int MealPlanId { get; set; }

        public string? MealPlanImage { get; set; }

        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public int? DietId { get; set; }
        public string Name { get; set; } = null!;
        public double TotalCalories { get; set; }
        /* public List<GetAllFoodOfMealMemberResonseDTO> FoodDetails { get; set; } = new List<GetAllFoodOfMealMemberResonseDTO>();*/
        public string? MainFoodImageForBreakfast { get; set; }
        public List<GetAllFoodOfMealMemberResonseDTO> BreakfastFoods { get; set; } = new List<GetAllFoodOfMealMemberResonseDTO>();

        public string? MainFoodImageForLunch { get; set; }
        public List<GetAllFoodOfMealMemberResonseDTO> LunchFoods { get; set; } = new List<GetAllFoodOfMealMemberResonseDTO>();

        public string? MainFoodImageForDinner { get; set; }
        public List<GetAllFoodOfMealMemberResonseDTO> DinnerFoods { get; set; } = new List<GetAllFoodOfMealMemberResonseDTO>();

        public string? MainFoodImageForSnack { get; set; }
        public List<GetAllFoodOfMealMemberResonseDTO> SnackFoods { get; set; } = new List<GetAllFoodOfMealMemberResonseDTO>();
    }
}