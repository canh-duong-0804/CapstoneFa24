using System;
using System.Collections.Generic;

namespace BusinessObject.Dto.MealPlanDetail
{
    public class CreateMealPlanDetailRequestDTO
    {
        public int MealPlanId { get; set; }
        public byte Day { get; set; }
        public string DescriptionBreakFast { get; set; }
        public List<FoodQuantityResponseDTO> ListFoodIdBreakfasts { get; set; } = new List<FoodQuantityResponseDTO>();
        public string DescriptionLunch { get; set; }
        public List<FoodQuantityResponseDTO> ListFoodIdLunches { get; set; } = new List<FoodQuantityResponseDTO>();
        public string DescriptionDinner { get; set; }
        public List<FoodQuantityResponseDTO> ListFoodIdDinners { get; set; } = new List<FoodQuantityResponseDTO>();
        public string DescriptionSnack { get; set; }
        public List<FoodQuantityResponseDTO> ListFoodIdSnacks { get; set; } = new List<FoodQuantityResponseDTO>();
    }

  
    public class FoodQuantityResponseDTO
    {
        public int FoodId { get; set; } 
        public int Quantity { get; set; }
    }

   /* public class FoodQuantityLunchResponseDTO
    {
        public int FoodId { get; set; }
        public int Quantity { get; set; } 
    }

    public class FoodQuantityDinnerResponseDTO
    {
        public int FoodId { get; set; } 
        public int Quantity { get; set; } 
    }

    public class FoodQuantitySnackResponseDTO
    {
        public int FoodId { get; set; } 
        public int Quantity { get; set; } 
    }*/
}
