using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.MealPlanDetail
{
    public class GetMealPlanDetailRequestDTO
    {
        public int MealPlanId { get; set; }
        public int MealType { get; set; }
        public int Day { get; set; }
    }
}
