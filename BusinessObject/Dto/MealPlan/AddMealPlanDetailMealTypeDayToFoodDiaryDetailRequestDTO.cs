using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.MealPlan
{
    public class AddMealPlanDetailMealTypeDayToFoodDiaryDetailRequestDTO
    {
        //public int DiaryId { get; set; }
        public int MealTypeDay { get; set; }
        public int SelectMealTypeToAdd { get; set; }

        public DateTime selectDateToAdd { get; set; }
        public int day { get; set; }
        public int MealPlanId { get; set; }
    }
}
