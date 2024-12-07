using BusinessObject.Dto.MealPlanDetailMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.MealDetailMember
{
    public class AddMoreFoodToMealMemberRequestDTO
    {
        public int mealMemberId { get; set; }
         public List<CreateMealDetailMemberRequestDTO> MealDetails { get; set; } = new List<CreateMealDetailMemberRequestDTO>();

    }
}
