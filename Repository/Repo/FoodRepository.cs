using BusinessObject.Dto.Food;
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
    public class FoodRepository : IFoodRepository
    {
        public Task<Food> CreateFoodAsync(Food food) => FoodDAO.Instance.CreateFoodAsync(food);




        public Task<bool> DeleteFoodAsync(int id) => FoodDAO.Instance.DeleteFoodAsync(id);


        public Task<IEnumerable<AllFoodForStaffResponseDTO>> GetAllFoodsAsync() => FoodDAO.Instance.GetAllFoodsAsync();
       

        public Task<GetFoodByIdResponseDTO> GetFoodByIdAsync(int id) => FoodDAO.Instance.GetFoodByIdAsync(id);

     

        public Task<Food> UpdateFoodAsync(Food food) => FoodDAO.Instance.UpdateFoodAsync(food);

       
    }
}
