using BusinessObject.Dto.MealMember;
using BusinessObject.Dto.MealPlan;
using BusinessObject.Dto.MealPlanDetail;
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
    public class MealPlanTrainnerRepository : IMealPlanTrainnerRepository
    {
        public Task<int> GetTotalMealPlanAsync() => MealPlanTrainnerDAO.Instance.GetTotalMealPlanAsync();


        //public Task<IEnumerable<GetAllMealPlanForMemberResponseDTO>> SearchMealPlanForMemberAsync(string mealPlanName) => MealPlanTrainnerDAO.Instance.SearchMealPlanForMemberAsync(mealPlanName);

        public Task<bool> UpdateMealPlanTrainerAsync(MealPlan mealPlanModel) => MealPlanTrainnerDAO.Instance.UpdateMealPlanTrainerAsync(mealPlanModel);
        public Task<bool> CreateMealPlanTrainerAsync(MealPlan mealPlanModel) => MealPlanTrainnerDAO.Instance.CreateMealPlanTrainerAsync(mealPlanModel);

        public Task<bool> DeleteMealPlanAsync(int mealPlanId) => MealPlanTrainnerDAO.Instance.DeleteMealPlanAsync(mealPlanId);

        public Task<GetMealPlanResponseDTO> GetMealPlanAsync(int mealPlanId) => MealPlanTrainnerDAO.Instance.GetMealPlanAsync(mealPlanId);
        public Task<IEnumerable<GetAllMealPlanForMemberResponseDTO>> GetAllMealPlanForStaffsAsync(int currentPage, int currentPageSize) => MealPlanTrainnerDAO.Instance.GetAllMealPlanForStaffsAsync(currentPage, currentPageSize);

        public Task<bool> CreateMealPlanDetailAsync(CreateMealPlanDetailRequestDTO request) => MealPlanTrainnerDAO.Instance.CreateMealPlanDetailAsync(request);

        public Task<bool> UpdateMealPlanDetailAsync(CreateMealPlanDetailRequestDTO request) => MealPlanTrainnerDAO.Instance.UpdateMealPlanDetailAsync(request);

        public Task<GetMealPlanDetaiTrainnerlResponseDTO> GetMealPlanDetailAsync(int MealPlanId, int MealType, int Day) => MealPlanTrainnerDAO.Instance.GetMealPlanDetailAsync(MealPlanId, MealType, Day);
        
    }
}
