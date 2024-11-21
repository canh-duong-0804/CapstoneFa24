using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.MealPlan
{
    public class GetAllMealPlanForMemberResponseDTO
    {
        public int MealPlanId { get; set; }
        public string DietName { get; set; }
     
        public string? MealPlanImage { get; set; }
        public string? ShortDescription { get; set; }
       
        public string Name { get; set; } = null!;
        public double TotalCalories { get; set; }
       
    }
}
