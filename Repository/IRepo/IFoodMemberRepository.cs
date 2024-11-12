using BusinessObject.Dto.Diet;
using BusinessObject.Dto.FoodMember;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IFoodMemberRepository
    {
        Task<bool> CreateFoodMemberAsync(FoodMember foodMemberModel);
        Task<bool> DeleteFoodMemberAsync(int foodMemberId, int memberId);
        Task<IEnumerable<GetAllFoodMemberResponseDTO>> GetAllFoodMemberByIdAsync(int memberId);
        Task<GetFoodMemberDetailResponseDTO> GetFoodMemberDetailByIdAsync(int foodMemberId,int memberId);
        Task<bool> UpdateFoodMemberAsync(UpdateFoodMemberRequestDTO updateFoodMemberDto, int memberId);
    }
}
