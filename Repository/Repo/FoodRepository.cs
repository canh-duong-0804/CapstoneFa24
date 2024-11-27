using BusinessObject.Dto.Diet;
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

        public Task<IEnumerable<DietResponseDTO>> GetAllDietAsync() => FoodDAO.Instance.GetAllDietAsync();

        public Task<IEnumerable<AllFoodForMemberResponseDTO>> GetAllFoodsForMemberAsync() => FoodDAO.Instance.GetAllFoodsForMemberAsync();


        public Task<IEnumerable<AllFoodForStaffResponseDTO>> GetAllFoodsForStaffAsync() => FoodDAO.Instance.GetAllFoodsForStaffAsync();
       

        public Task<GetFoodForMemberByIdResponseDTO> GetFoodForMemberByIdAsync(int FoodId, DateTime SelectDate, int memberId) => FoodDAO.Instance.GetFoodForMemberByIdAsync(FoodId,SelectDate,memberId);

        public Task<GetFoodForStaffByIdResponseDTO> GetFoodForStaffByIdAsync(int id) => FoodDAO.Instance.GetFoodForStaffByIdAsync(id);

        public Task<List<FoodListBoxResponseDTO>> GetListBoxFoodForStaffAsync() => FoodDAO.Instance.GetListBoxFoodForStaffAsync();

        public Task<IEnumerable<AllFoodForMemberResponseDTO>> SearchFoodsForMemberAsync(string foodName) => FoodDAO.Instance.SearchFoodsForMemberAsync(foodName);
        

        public Task<Food> UpdateFoodAsync(Food food) => FoodDAO.Instance.UpdateFoodAsync(food);

        public Task<bool> UploadImageFood(string urlImage, int foodId) => FoodDAO.Instance.UploadImageFood(urlImage, foodId);

    }
}
