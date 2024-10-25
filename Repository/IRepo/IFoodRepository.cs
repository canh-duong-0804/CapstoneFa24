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


        Task<IEnumerable<AllFoodForStaffResponseDTO>> GetAllFoodsAsync();


        Task<GetFoodByIdResponseDTO> GetFoodByIdAsync(int id);


        Task<Food> UpdateFoodAsync(Food blog);


        Task<bool> DeleteFoodAsync(int id);
    }
}
