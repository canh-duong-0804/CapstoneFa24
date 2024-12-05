using BusinessObject.Dto.Food;
using BusinessObject.Dto.FoodDiary;
using BusinessObject.Dto.FoodDiaryDetails;
using BusinessObject.Dto.MainDashBoardMobile;
using BusinessObject.Dto.MealDetailMember;
using BusinessObject.Dto.Streak;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IFoodDiaryRepository
    {
        Task<bool> AddFoodListToDiaryAsync(FoodDiaryDetailRequestDTO listFoodDiaryDetail);
        Task<bool> DeleteFoodListToDiaryAsync(int foodDiaryDetailId);
        Task getDailyFoodDiaryFollowMeal(int dairyID, int mealType);
        Task<FoodDiary> GetOrCreateFoodDiaryByDate(int memberId, DateTime date);
        Task<UpdateFoodDiaryRequestDTO> UpdateFoodDiary(UpdateFoodDiaryRequestDTO updatedFoodDiary);
        Task<MainDashResponseDTO> GetFoodDairyDetailById(int memberId, DateTime date);
        //Task<MainDashBoardMobileForMemberResponseDTO> GetMainDashBoardForMemberById(int id, DateTime date);
        Task<FoodDiaryForMemberMobileResponse> GetFoodDairyByDate(int memberId, DateTime date);
        Task<IEnumerable<AllFoodForMemberResponseDTO>> GetFoodHistoryAsync(int memberId);
        Task<IEnumerable<AllFoodForMemberResponseDTO>> GetFoodSuggestionAsync(int memberId);
        //Task<List<GetFoodDiaryDateResponseDTO>> GetFoodDairyDateAsync(int memberId);
        Task<CalorieStreakDTO> GetCalorieStreakAsync(int memberId, DateTime date);
        Task<bool> addFoodListToDiaryForWebsite(AddFoodDiaryDetailForWebsiteRequestDTO request, int memberId);
        Task<AddFoodDiaryDetailForWebsiteRequestDTO> GetFoodDairyDetailWebsite(int memberId, DateTime selectDate, int mealtype);
        Task<List<FoodDiaryWithMealTypeDTO>> GetAllDiariesForMonthWithMealTypesAsync(DateTime date, int memberId);
    }
}
