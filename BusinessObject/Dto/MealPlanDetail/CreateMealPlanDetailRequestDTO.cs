using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.MealPlanDetail
{
    public class CreateMealPlanDetailRequestDTO
    {
      
        public int MealPlanId { get; set; }
      
        public int MealType { get; set; }
        public byte Day { get; set; }
        public string? Description { get; set; }
        public List<FoodQuantityResponseDTO> FoodIds { get; set; } = new List<FoodQuantityResponseDTO>();
       
    }
    public class FoodQuantityResponseDTO
    {
        public int FoodId { get; set; }
       
        public int Quantity { get; set; } 
    }

}
