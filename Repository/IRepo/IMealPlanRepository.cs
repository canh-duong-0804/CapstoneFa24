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
        Task<bool> AddMealPlanDetailWithDayToFoodDiaryAsync(AddMealPlanDetailDayToFoodDiaryDetailRequestDTO addMealPlanDetail, int memberId);
        Task<bool> AddMealPlanToFoodDiaryAsync(int mealPlanId, int memberId);
        Task<IEnumerable<GetAllMealPlanForMemberResponseDTO>> GetAllMealPlansForMemberAsync();
        Task<MealPlanDetailResponseDTO> GetMealPlanDetailForMemberAsync(int mealPlanId,int day);
        Task<IEnumerable<GetAllMealPlanForMemberResponseDTO>> SearchMealPlanForMemberAsync(string mealPlanName);
    }
}
