using BusinessObject.Dto.MealPlanDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.FoodDiary
{
    public class AddFoodDiaryDetailForWebsiteRequestDTO
    {
        
        public int MealType { get; set; }
        public DateTime selectDate { get; set; }
        public List<FoodDiaryDetailForWebisteRequestDTO> ListFoodIdToAdd { get; set; } = new List<FoodDiaryDetailForWebisteRequestDTO>();

    }

    public class FoodDiaryDetailForWebisteRequestDTO
    {
        
       
        public int FoodId { get; set; }
        public double Quantity { get; set; }
        public string Portion { get; set; } = null!;

    }
}
