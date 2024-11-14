using BusinessObject.Dto.MealPlan;
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
        public Task<bool> AddMealPlanToFoodDiaryAsync(int mealPlanId, int memberId) => MealPlanDAO.Instance.AddMealPlanToFoodDiaryAsync(mealPlanId,memberId);
        

        public Task<IEnumerable<GetAllMealPlanForMemberResponseDTO>> GetAllMealPlansForMemberAsync() => MealPlanDAO.Instance.GetAllMealPlansForMemberAsync();

       public Task<MealPlanDetailResponseDTO> GetMealPlanDetailForMemberAsync(int mealPlanId, int day) => MealPlanDAO.Instance.GetMealPlanDetailForMemberAsync(mealPlanId,day);
     
    }
}
