using BusinessObject.Dto.Food;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class FoodDAO
    {
        private static FoodDAO instance = null;
        private static readonly object instanceLock = new object();

        public static FoodDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new FoodDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<Food> CreateFoodAsync(Food food)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                   
                    await context.Foods.AddAsync(food);

                    
                    await context.SaveChangesAsync();

                    return food;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating food: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteFoodAsync(int id)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                
                    var existingFood = await context.Foods.FindAsync(id);

                    if (existingFood == null)
                    {
                        throw new Exception("Food not found.");
                    }

                    
                    existingFood.Status = false; 

                    await context.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating food status: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<AllFoodForStaffResponseDTO>> GetAllFoodsAsync()
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    var foods = await (from food in context.Foods
                                       join diet in context.Diets on food.DietId equals diet.DietId
                                       where food.Status == true
                                       select new AllFoodForStaffResponseDTO
                                       {
                                           Name = food.Name,
                                           CreateBy = food.CreateBy,
                                           CreateDate = food.CreateDate,
                                           ChangeDate = food.ChangeDate,
                                           FoodImage = food.FoodImage,
                                           Calories = food.Calories,
                                           DietName = food.Diet.DietName,
                                       }).ToListAsync();

                    return foods;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving blogs: {ex.Message}", ex);
            }
        }

        public async Task<GetFoodByIdResponseDTO> GetFoodByIdAsync(int id)
        {
            try
            {
             
                    using (var context = new HealthTrackingDBContext())
                    {
                       
                        var responseDto = await context.Foods
                            .Where(f => f.FoodId == id && f.Status == true)
                            .Select(food => new GetFoodByIdResponseDTO
                            {
                                Name = food.Name,
                                Portion = food.Portion,
                                Calories = food.Calories,
                                CreateBy = food.CreateBy,
                                CreateDate = food.CreateDate,
                                ChangeBy = food.ChangeBy,
                                ChangeDate = food.ChangeDate,
                                FoodImage = food.FoodImage,
                                Protein = food.Protein,
                                Carbs = food.Carbs,
                                Fat = food.Fat,
                                VitaminA = food.VitaminA,
                                VitaminC = food.VitaminC,
                                VitaminD = food.VitaminD,
                                VitaminE = food.VitaminE,
                                VitaminB1 = food.VitaminB1,
                                VitaminB2 = food.VitaminB2,
                                VitaminB3 = food.VitaminB3,
                                Status = food.Status,
                                DietId = food.DietId,
                                DietName = food.Diet.DietName 
                            })
                            .FirstOrDefaultAsync();

                        return responseDto;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving food: {ex.Message}", ex);
            }
        }


        public async Task<Food> UpdateFoodAsync(Food food)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    var existingFood = await context.Foods.FindAsync(food.FoodId);

                    if (existingFood == null)
                    {

                        throw new Exception("Food not found.");
                    }


                    existingFood.Name = food.Name;
                    existingFood.Portion = food.Portion;
                    existingFood.Calories = food.Calories;
                    existingFood.CreateBy = food.CreateBy;
                    existingFood.ChangeBy = food.ChangeBy;
                    existingFood.ChangeDate = DateTime.Now;
                    existingFood.FoodImage = food.FoodImage;
                    existingFood.Protein = food.Protein;
                    existingFood.Carbs = food.Carbs;
                    existingFood.Fat = food.Fat;
                    existingFood.VitaminA = food.VitaminA;
                    existingFood.VitaminC = food.VitaminC;
                    existingFood.VitaminD = food.VitaminD;
                    existingFood.VitaminE = food.VitaminE;
                    existingFood.VitaminB1 = food.VitaminB1;
                    existingFood.VitaminB2 = food.VitaminB2;
                    existingFood.VitaminB3 = food.VitaminB3;
                    existingFood.Status = food.Status;
                    existingFood.DietId = food.DietId;


                    await context.SaveChangesAsync();


                    return existingFood;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating food: {ex.Message}", ex);
            }
        }
    }
}
