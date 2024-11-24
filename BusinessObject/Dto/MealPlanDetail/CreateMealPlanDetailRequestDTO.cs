using System;
using System.Collections.Generic;

namespace BusinessObject.Dto.MealPlanDetail
{
    public class CreateMealPlanDetailRequestDTO
    {
        public int MealPlanId { get; set; }
        public byte Day { get; set; }
        public List<MealPlanDetailDTO> MealPlanDetails { get; set; } = new List<MealPlanDetailDTO>();
    }

    public class MealPlanDetailDTO
    {
   

        public BreakfastDTO Breakfast { get; set; } = new BreakfastDTO();
        public LunchDTO Lunch { get; set; } = new LunchDTO();
        public DinnerDTO Dinner { get; set; } = new DinnerDTO();
        public SnackDTO Snack { get; set; } = new SnackDTO();
    }

    public class BreakfastDTO
    {
        public string Description { get; set; } // General description for Breakfast
        public List<FoodQuantityBreakfastResponseDTO> ListFoodIdBreakfasts { get; set; } = new List<FoodQuantityBreakfastResponseDTO>();
    }

    public class LunchDTO
    {
        public string Description { get; set; }
        public List<FoodQuantityLunchResponseDTO> ListFoodIdLunches { get; set; } = new List<FoodQuantityLunchResponseDTO>();
    }

    public class DinnerDTO
    {
        public string Description { get; set; }
        public List<FoodQuantityDinnerResponseDTO> ListFoodIdDinners { get; set; } = new List<FoodQuantityDinnerResponseDTO>();
    }

    public class SnackDTO
    {
        public string Description { get; set; }
        public List<FoodQuantitySnackResponseDTO> ListFoodIdSnacks { get; set; } = new List<FoodQuantitySnackResponseDTO>();
    }

    public class FoodQuantityBreakfastResponseDTO
    {
        public int FoodIdBreakfast { get; set; } // Food ID for Breakfast
        public int Quantity { get; set; } // Quantity of Breakfast Items
    }

    public class FoodQuantityLunchResponseDTO
    {
        public int FoodIdLunch { get; set; } // Food ID for Lunch
        public int Quantity { get; set; } // Quantity of Lunch Items
    }

    public class FoodQuantityDinnerResponseDTO
    {
        public int FoodIdDinner { get; set; } // Food ID for Dinner
        public int Quantity { get; set; } // Quantity of Dinner Items
    }

    public class FoodQuantitySnackResponseDTO
    {
        public int FoodIdSnack { get; set; } // Food ID for Snack
        public int Quantity { get; set; } // Quantity of Snack Items
    }
}
