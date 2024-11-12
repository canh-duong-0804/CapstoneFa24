using AutoMapper.Execution;
using BusinessObject.Dto.Diet;
using BusinessObject.Dto.FoodMember;
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
    public class FoodMemberRepository : IFoodMemberRepository
    {
        public Task<bool> CreateFoodMemberAsync(FoodMember foodMemberModel) => FoodMemberDAO.Instance.CreateFoodMemberAsync(foodMemberModel);

        public Task<bool> DeleteFoodMemberAsync(int foodMemberId) => FoodMemberDAO.Instance.DeleteFoodMemberAsync(foodMemberId);


        public Task<IEnumerable<GetAllFoodMemberResponseDTO>> GetAllFoodMemberByIdAsync(int memberId) => FoodMemberDAO.Instance.GetAllFoodMemberByIdAsync(memberId);

        public Task<GetFoodMemberDetailResponseDTO> GetFoodMemberDetailByIdAsync(int foodMemberId) => FoodMemberDAO.Instance.GetFoodMemberDetailByIdAsync(foodMemberId);

        public Task<bool> UpdateFoodMemberAsync(UpdateFoodMemberRequestDTO updateFoodMemberDto) => FoodMemberDAO.Instance.UpdateFoodMemberAsync(updateFoodMemberDto);
       
    }
}
