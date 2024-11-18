using BusinessObject.Dto.MealPlan;
using BusinessObject.Models;
using DataAccess;
using Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repo
{
    public class MealPlanRepository : IMealPlanRepository
    {
        public Task<bool> AddMealPlanDetailWithDayToFoodDiaryAsync(AddMealPlanDetailDayToFoodDiaryDetailRequestDTO addMealPlanDetail, int memberId) => MealPlanDAO.Instance.AddMealPlanDetailWithDayToFoodDiaryAsync(addMealPlanDetail, memberId);

        public Task<bool> AddMealPlanDetailWithMealTypeDayToFoodDiary(AddMealPlanDetailMealTypeDayToFoodDiaryDetailRequestDTO addMealPlanDetail, int memberId) => MealPlanDAO.Instance.AddMealPlanDetailWithMealTypeDayToFoodDiary(addMealPlanDetail, memberId);


        public Task<bool> AddMealPlanToFoodDiaryAsync(int mealPlanId, int memberId, DateTime selectDate) => MealPlanDAO.Instance.AddMealPlanToFoodDiaryAsync(mealPlanId,memberId,selectDate);
        

        public Task<IEnumerable<GetAllMealPlanForMemberResponseDTO>> GetAllMealPlansForMemberAsync() => MealPlanDAO.Instance.GetAllMealPlansForMemberAsync();

       public Task<MealPlanDetailResponseDTO> GetMealPlanDetailForMemberAsync(int mealPlanId, int day) => MealPlanDAO.Instance.GetMealPlanDetailForMemberAsync(mealPlanId,day);

        public Task<IEnumerable<GetAllMealPlanForMemberResponseDTO>> SearchMealPlanForMemberAsync(string mealPlanName) => MealPlanDAO.Instance.SearchMealPlanForMemberAsync(mealPlanName);

        
    }
}
