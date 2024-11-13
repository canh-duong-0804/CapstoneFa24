using BusinessObject.Dto.MealPlan;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IMealPlanRepository
    {
        Task<bool> AddMealPlanToFoodDiaryAsync(int mealPlanId, int memberId);
        Task<IEnumerable<GetAllMealPlanForMemberResponseDTO>> GetAllMealPlansForMemberAsync();
       Task<bool> GetMealPlanDetailForMemberAsync(int mealPlanId,int day);
    }
}
