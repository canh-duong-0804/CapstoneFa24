using BusinessObject.Dto.MealMember;
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

        public Task<bool> CreateMealPlanTrainerAsync(MealPlan mealPlanModel) => MealPlanDAO.Instance.CreateMealPlanTrainerAsync(mealPlanModel);

        public Task<bool> DeleteMealPlanAsync(int mealPlanId) => MealPlanDAO.Instance.DeleteMealPlanAsync(mealPlanId);


        public Task<IEnumerable<GetAllMealPlanForMemberResponseDTO>> GetAllMealPlanForStaffsAsync(int currentPage, int currentPageSize) => MealPlanDAO.Instance.GetAllMealPlanForStaffsAsync(currentPage,currentPageSize);
        

        public Task<IEnumerable<GetAllMealPlanForMemberResponseDTO>> GetAllMealPlansForMemberAsync() => MealPlanDAO.Instance.GetAllMealPlansForMemberAsync();

        public Task<GetMealPlanResponseDTO> GetMealPlanAsync(int mealPlanId) => MealPlanDAO.Instance.GetMealPlanAsync(mealPlanId);


        public Task<MealPlanDetailResponseDTO> GetMealPlanDetailForMemberAsync(int mealPlanId, int day) => MealPlanDAO.Instance.GetMealPlanDetailForMemberAsync(mealPlanId,day);

        public Task<int> GetTotalMealPlanAsync() => MealPlanDAO.Instance.GetTotalMealPlanAsync();


        public Task<IEnumerable<GetAllMealPlanForMemberResponseDTO>> SearchMealPlanForMemberAsync(string mealPlanName) => MealPlanDAO.Instance.SearchMealPlanForMemberAsync(mealPlanName);

        public Task<bool> UpdateMealPlanTrainerAsync(MealPlan mealPlanModel) => MealPlanDAO.Instance.UpdateMealPlanTrainerAsync(mealPlanModel);
        
    }
}
