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


        Task<IEnumerable<AllFoodForStaffResponseDTO>> GetAllFoodsForStaffAsync();


        Task<GetFoodForStaffByIdResponseDTO> GetFoodForStaffByIdAsync(int id);


        Task<Food> UpdateFoodAsync(Food blog);


        Task<bool> DeleteFoodAsync(int id);
        Task<IEnumerable<DietResponseDTO>> GetAllDietAsync();
        Task<IEnumerable<AllFoodForMemberResponseDTO>> GetAllFoodsForMemberAsync();
        Task<GetFoodForMemberByIdResponseDTO> GetFoodForMemberByIdAsync(int id);
        Task<List<FoodListBoxResponseDTO>> GetListBoxFoodForStaffAsync();
    }
}
