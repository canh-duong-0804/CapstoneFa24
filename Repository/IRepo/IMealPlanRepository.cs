using BusinessObject.Dto.MealMember;
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
        Task<bool> AddMealPlanDetailWithMealTypeDayToFoodDiary(AddMealPlanDetailMealTypeDayToFoodDiaryDetailRequestDTO addMealPlanDetail, int memberId);
        Task<int> AddMealPlanToFoodDiaryAgainAsync(int mealPlanId, int memberId, DateTime selectDate);
        Task<int> AddMealPlanToFoodDiaryAsync(int mealPlanId, int memberId, DateTime selectDate);
        
        Task<IEnumerable<GetAllMealPlanForMemberResponseDTO>> GetAllMealPlansForMemberAsync();
       
        Task<MealPlanDetailResponseDTO> GetMealPlanDetailForMemberAsync(int mealPlanId,int day);
       
        Task<IEnumerable<GetAllMealPlanForMemberResponseDTO>> SearchMealPlanForMemberAsync(string mealPlanName);
       
    }
}
