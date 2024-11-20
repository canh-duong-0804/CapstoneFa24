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
        Task<bool> AddMealPlanToFoodDiaryAsync(int mealPlanId, int memberId, DateTime selectDate);
        Task<bool> CreateMealPlanTrainerAsync(MealPlan mealPlanModel);
        Task<bool> DeleteMealPlanAsync(int mealPlanId);
        Task<IEnumerable<GetAllMealPlanForMemberResponseDTO>> GetAllMealPlanForStaffsAsync(int currentPage, int currentPageSize);
        Task<IEnumerable<GetAllMealPlanForMemberResponseDTO>> GetAllMealPlansForMemberAsync();
        Task<GetMealPlanResponseDTO> GetMealPlanAsync(int mealPlanId);
        Task<MealPlanDetailResponseDTO> GetMealPlanDetailForMemberAsync(int mealPlanId,int day);
        Task<int> GetTotalMealPlanAsync();
        Task<IEnumerable<GetAllMealPlanForMemberResponseDTO>> SearchMealPlanForMemberAsync(string mealPlanName);
        Task<bool> UpdateMealPlanTrainerAsync(MealPlan mealPlanModel);
    }
}
