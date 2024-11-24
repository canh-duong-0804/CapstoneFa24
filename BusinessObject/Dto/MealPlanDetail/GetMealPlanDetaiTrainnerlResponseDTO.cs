﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.MealPlanDetail
{
    public class GetMealPlanDetaiTrainnerlResponseDTO
    {
       
          
            public int MealPlanId { get; set; }

           // public int MealType { get; set; }
            public int Day { get; set; }
        //

        
        /*  public List<GetFoodInMealPlanResponseDTO> FoodIds { get; set; } = new List<GetFoodInMealPlanResponseDTO>();*/
        public List<GetFoodInMealPlanBreakfastResponseDTO> BreakfastFoods { get; set; }
        public List<GetFoodInMealPlanLunchResponseDTO> LunchFoods { get; set; }
        public List<GetFoodInMealPlanDinnerResponseDTO> DinnerFoods { get; set; }
        public List<GetFoodInMealPlanSnackResponseDTO> SnackFoods { get; set; }



    }
    public class GetFoodInMealPlanBreakfastResponseDTO
    {
        public int FoodId { get; set; }
        public string FoodName { get; set; }
        public int Quantity { get; set; }
        public double Calories { get; set; }
        public int MealType { get; set; }

        public string? FoodImage { get; set; }
        public double Protein { get; set; }
        public double Carbs { get; set; }
        public double Fat { get; set; }
        public string Description { get; set; }


    }public class GetFoodInMealPlanSnackResponseDTO
    {
        public int FoodId { get; set; }
        public string FoodName { get; set; }
        public int Quantity { get; set; }
        public double Calories { get; set; }
        public int MealType { get; set; }

        public string? FoodImage { get; set; }
        public double Protein { get; set; }
        public double Carbs { get; set; }
        public double Fat { get; set; }
        public string Description { get; set; }


    }
    public class GetFoodInMealPlanLunchResponseDTO
    {
        public int FoodId { get; set; }
        public string FoodName { get; set; }
        public int Quantity { get; set; }
        public double Calories { get; set; }
        public int MealType { get; set; }

        public string? FoodImage { get; set; }
        public double Protein { get; set; }
        public double Carbs { get; set; }
        public double Fat { get; set; }
        public string Description { get; set; }


    }
    public class GetFoodInMealPlanDinnerResponseDTO
    {
        public int FoodId { get; set; }
        public string FoodName { get; set; }
        public int Quantity { get; set; }
        public double Calories { get; set; }
        public int MealType { get; set; }

        public string? FoodImage { get; set; }
        public double Protein { get; set; }
        public double Carbs { get; set; }
        public double Fat { get; set; }
        public string Description { get; set; }


    }
}
