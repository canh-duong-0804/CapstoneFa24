using BusinessObject.Dto.MealDetailMember;
using BusinessObject.Dto.MealMember;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IMealMemberRepository
    {
        Task<bool> AddMealMemberToDiaryDetailAsync(AddMealMemberToFoodDiaryDetailRequestDTO addMealMemberTOFoodDiary,int memberId);
        Task<int> CreateMealMemberAsync(MealMember mealMember);
        Task CreateMealMemberDetailsAsync(List<MealMemberDetail> mealMemberDetails);
        Task<bool> CreateMealPlanForMember(MealMember mealMember);
        Task DeleteMealMemberAsync(int mealMemberId);
        Task DeleteMealMemberDetailAsync(int detailId);
        Task DeleteMealMemberDetailsByMealMemberIdAsync(int mealMemberId);
        Task<IEnumerable<MealMember>> GetAllMealMembersAsync(int memberId);
        Task<MealMemberDetailResonseDTO> GetMealMemberDetailAsync(int mealMemberId);
        Task UpdateMealMemberTotalCaloriesAsync(int mealMemberId);

        //Task<bool> CreateMealPlanForMember(MealMember mealMemberModel);
    }
}
