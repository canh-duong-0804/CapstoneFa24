using BusinessObject.Dto.Diet;
using BusinessObject.Dto.Food;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IFoodRepository
    {
        Task<Food> CreateFoodAsync(Food food);


        Task<IEnumerable<AllFoodForStaffResponseDTO>> GetAllFoodsForStaffAsync(int currentPage, int pageSize, string? searchFood);


        Task<GetFoodForStaffByIdResponseDTO> GetFoodForStaffByIdAsync(int id);


        Task<Food> UpdateFoodAsync(Food blog);


        Task<bool> DeleteFoodAsync(int id);
        Task<IEnumerable<DietResponseDTO>> GetAllDietAsync();
        Task<IEnumerable<AllFoodForMemberResponseDTO>> GetAllFoodsForMemberAsync();
        Task<GetFoodForMemberByIdResponseDTO> GetFoodForMemberByIdAsync(int FoodId, DateTime SelectDate, int memberId);
        Task<List<FoodListBoxResponseDTO>> GetListBoxFoodForStaffAsync();
        Task<IEnumerable<AllFoodForMemberResponseDTO>> SearchFoodsForMemberAsync(string foodName);
        Task<bool> UploadImageFood(string v, int foodId);
        Task<int> GetTotalFoodsForStaffAsync(string? searchFood);
    }
}
