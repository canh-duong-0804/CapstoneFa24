using BusinessObject.Dto.MealMember;
using BusinessObject.Dto.MealPlan;
using BusinessObject.Dto.MealPlanDetail;
using BusinessObject.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IMealPlanTrainnerRepository
    {
        Task<bool> UpdateMealPlanTrainerAsync(MealPlan mealPlanModel);
        Task<bool> CreateMealPlanTrainerAsync(MealPlan mealPlanModel);
        Task<bool> DeleteMealPlanAsync(int mealPlanId);
        Task<IEnumerable<GetAllMealPlanForMemberResponseDTO>> GetAllMealPlanForStaffsAsync(int currentPage, int currentPageSize);
        Task<GetMealPlanResponseDTO> GetMealPlanAsync(int mealPlanId);
        Task<int> GetTotalMealPlanAsync();
        Task<bool> CreateMealPlanDetailAsync(CreateMealPlanDetailRequestDTO request);
        Task<bool> UpdateMealPlanDetailAsync(CreateMealPlanDetailRequestDTO request);
        Task<GetMealPlanDetaiTrainnerlResponseDTO> GetMealPlanDetailAsync(int MealPlanId, int MealType, int Day);
    }
}
