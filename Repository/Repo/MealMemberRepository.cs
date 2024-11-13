using BusinessObject.Dto.MealDetailMember;
using BusinessObject.Dto.MealMember;
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
    public class MealMemberRepository : IMealMemberRepository
    {
        public Task<bool> AddMealMemberToDiaryDetailAsync(AddMealMemberToFoodDiaryDetailRequestDTO addMealMemberTOFoodDiary, int memberId) => MealMemberDAO.Instance.AddMealMemberToDiaryDetailAsync(addMealMemberTOFoodDiary, memberId);
        

        public Task<int> CreateMealMemberAsync(MealMember mealMember) => MealMemberDAO.Instance.CreateMealMemberAsync(mealMember);



        public Task CreateMealMemberDetailsAsync(List<MealMemberDetail> mealMemberDetails) => MealMemberDAO.Instance.CreateMealMemberDetailsAsync(mealMemberDetails);


        /* public Task<int> CreateMealMemberAsync(List<MealMemberDetail> mealMember) => MealMemberDAO.Instance.CreateMealMemberAsync(mealMember);



public Task CreateMealMemberDetailsAsync(MealMemberDetail mealMemberDetails) => MealMemberDAO.Instance.CreateMealMemberDetailsAsync(mealMemberDetails);*/


        //public Task CreateMealPlanDetailsOfMemberAsync(MealsMemberDetail mealMemberModel, int memberid, DateTime date)=> MealPlanDetailsDAO.Instance.CreateMealPlanDetailsOfMemberAsync
        public Task<bool> CreateMealPlanForMember(MealMember mealMember) => MealMemberDAO.Instance.CreateMealPlanDetailsOfMemberAsync(mealMember);

        public Task DeleteMealMemberAsync(int mealMemberId) => MealMemberDAO.Instance.DeleteMealMemberAsync(mealMemberId);
        

        public Task DeleteMealMemberDetailAsync(int detailId) => MealMemberDAO.Instance.DeleteMealMemberDetailAsync(detailId);

        public Task DeleteMealMemberDetailsByMealMemberIdAsync(int mealMemberId) => MealMemberDAO.Instance.DeleteMealMemberDetailsByMealMemberIdAsync(mealMemberId);
       

        public Task<IEnumerable<MealMember>> GetAllMealMembersAsync(int memberId) => MealMemberDAO.Instance.GetAllMealMembersAsync(memberId);

        public Task<MealMemberDetailResonseDTO> GetMealMemberDetailAsync(int mealMemberId) => MealMemberDAO.Instance.GetMealMemberDetailAsync(mealMemberId);



        public Task UpdateMealMemberTotalCaloriesAsync(int mealMemberId) => MealMemberDAO.Instance.UpdateMealMemberTotalCaloriesAsync(mealMemberId);


    }
}
