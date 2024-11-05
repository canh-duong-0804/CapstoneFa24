using BusinessObject.Dto.FoodDiary;
using BusinessObject.Dto.FoodDiaryDetails;
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
        Task<bool> DeleteFoodListToDiaryAsync(int id);
        Task getDailyFoodDiaryFollowMeal(int dairyID, int mealType);
        Task<FoodDiary> GetOrCreateFoodDiaryByDate(int memberId, DateTime date);
        Task<UpdateFoodDiaryRequestDTO> UpdateFoodDiary(UpdateFoodDiaryRequestDTO updatedFoodDiary);
    }
}
