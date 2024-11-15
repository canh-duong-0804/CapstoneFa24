using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.MealPlan
{
    public class AddMealPlanDetailDayToFoodDiaryDetailRequestDTO
    {
        public int DiaryId { get; set; }
       
        public int day { get; set; }  
        public int MealPlanId { get; set; }
    }
}
