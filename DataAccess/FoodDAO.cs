using AutoMapper.Execution;
using BusinessObject.Dto.Diet;
using BusinessObject.Dto.Food;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
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

        public async Task<IEnumerable<AllFoodForStaffResponseDTO>> GetAllFoodsForStaffAsync(int currentPage,int pageSize)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    // Calculate total count of foods
                    /* int totalCount = await context.Foods
                         .Where(food => food.Status == true)
                         .CountAsync();*/

                    // Get paginated foods
                    var foods = await (from food in context.Foods
                                       join diet in context.Diets on food.DietId equals diet.DietId
                                       where food.Status == true
                                       orderby food.FoodId descending// Ensure consistent ordering
                                       select new AllFoodForStaffResponseDTO
                                       {
                                           FoodId = food.FoodId,
                                           CreateByName = food.CreateByNavigation.FullName,
                                           ChangeBy = food.ChangeBy,
                                           ChangeByName = food.CreateByNavigation.FullName,

                                           FoodName = food.FoodName,
                                           CreateBy = food.CreateBy,
                                           CreateDate = food.CreateDate,
                                           ChangeDate = food.ChangeDate,
                                           FoodImage = food.FoodImage,
                                           Calories = food.Calories,
                                           DietName = food.Diet.DietName,
                                       })
                                       .Skip((currentPage - 1) * pageSize) // Skip items for previous pages
                                       .Take(pageSize) // Take items for the current page
                                       .ToListAsync();

                    return foods;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving foods: {ex.Message}", ex);
            }
        }

        public async Task<GetFoodForStaffByIdResponseDTO> GetFoodForStaffByIdAsync(int id)
        {
            try
            {

                using (var context = new HealthTrackingDBContext())
                {

                    var responseDto = await context.Foods
                        .Where(f => f.FoodId == id && f.Status == true)
                        .Include(f => f.CreateByNavigation)
                        .Select(food => new GetFoodForStaffByIdResponseDTO
                        {
                            FoodId = food.FoodId,
                            FoodName = food.FoodName,
                            Portion = food.Portion,
                            Calories = food.Calories,
                            CreateBy = food.CreateByNavigation.FullName,
                            CreateDate = food.CreateDate,
                            ChangeBy = food.CreateByNavigation.FullName,
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


                    existingFood.FoodName = food.FoodName;
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

        public async Task<IEnumerable<DietResponseDTO>> GetAllDietAsync()
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    var diets = await context.Diets.ToListAsync();


                    var dietResponse = diets.Select(d => new DietResponseDTO
                    {
                        DietId = d.DietId,
                        DietName = d.DietName,

                    }).ToList();

                    return dietResponse;


                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving diets: {ex.Message}", ex);
            }

        }

        public async Task<IEnumerable<AllFoodForMemberResponseDTO>> GetAllFoodsForMemberAsync()
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    var foods = await (from food in context.Foods
                                       join diet in context.Diets on food.DietId equals diet.DietId
                                       where food.Status == true
                                       select new AllFoodForMemberResponseDTO
                                       {
                                           FoodId = food.FoodId,
                                           FoodName = food.FoodName,
                                           FoodImage = food.FoodImage,
                                           Calories = food.Calories,
                                           Fat = food.Fat,
                                           Carbs = food.Carbs,
                                           Protein = food.Protein,
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


        


        public async Task<GetFoodForMemberByIdResponseDTO> GetFoodForMemberByIdAsync(int FoodId, DateTime SelectDate, int memberId)
        {
            try
            {

                using (var context = new HealthTrackingDBContext())
                {

                    var responseDto = await context.Foods
                        .Where(f => f.FoodId == FoodId && f.Status == true)
                        .Select(food => new GetFoodForMemberByIdResponseDTO
                        {
                            FoodId = food.FoodId,
                            FoodName = food.FoodName,
                            Portion = food.Portion,
                            Calories = food.Calories,

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








                    var getGoalCalo = await context.FoodDiaries
                   .FirstOrDefaultAsync(fd => fd.MemberId == memberId && fd.Date.Date == SelectDate);

                    if (getGoalCalo == null)
                    {


                        await FoodDiaryDAO.Instance.GetOrCreateFoodDiaryAsync(memberId, SelectDate);
                        getGoalCalo = await context.FoodDiaries
                           .FirstOrDefaultAsync(fd => fd.MemberId == memberId && fd.Date.Date == SelectDate);

                    }






                    responseDto.totalFat = (double)Math.Round((decimal)(getGoalCalo.GoalCalories * 0.25) / 9, 1);
                    responseDto.totalCalories = (double)getGoalCalo.GoalCalories;
                    responseDto.totalCarb = (double)Math.Round((decimal)(getGoalCalo.GoalCalories * 0.45) / 4, 1);
                    responseDto.totalProtein = (double)Math.Round((decimal)(getGoalCalo.GoalCalories * 0.3) / 4, 1);
                    return responseDto;


                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving food: {ex.Message}", ex);
            }
        }

        public async Task<List<FoodListBoxResponseDTO>> GetListBoxFoodForStaffAsync()
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    var foods = await (from food in context.Foods
                                       join diet in context.Diets on food.DietId equals diet.DietId
                                       where food.Status == true
                                       select new FoodListBoxResponseDTO
                                       {
                                           Value = food.FoodId,
                                           Label = food.FoodName,
                                           Calories=food.Calories


                                       }).ToListAsync();

                    return foods;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving blogs: {ex.Message}", ex);
            }

        }

        public async Task<IEnumerable<AllFoodForMemberResponseDTO>> SearchFoodsForMemberAsync(string? foodName)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    if (string.IsNullOrWhiteSpace(foodName))
                    {

                        return await (from food in context.Foods
                                      join diet in context.Diets on food.DietId equals diet.DietId
                                      where food.Status == true
                                      select new AllFoodForMemberResponseDTO
                                      {
                                          FoodId = food.FoodId,
                                          FoodName = food.FoodName,
                                          FoodImage = food.FoodImage,
                                          Calories = food.Calories,
                                          Fat = food.Fat,
                                          Carbs = food.Carbs,
                                          Protein = food.Protein,
                                          DietName = diet.DietName
                                      }).ToListAsync();
                    }


                    var foods = await (from food in context.Foods
                                       join diet in context.Diets on food.DietId equals diet.DietId
                                       where food.Status == true && EF.Functions.Collate(food.FoodName.ToLower(), "Vietnamese_CI_AI").Contains(foodName.ToLower())
                                       select new AllFoodForMemberResponseDTO
                                       {
                                           FoodId = food.FoodId,
                                           FoodName = food.FoodName,
                                           FoodImage = food.FoodImage,
                                           Calories = food.Calories,
                                           Fat = food.Fat,
                                           Carbs = food.Carbs,
                                           Protein = food.Protein,
                                           DietName = diet.DietName
                                       }).ToListAsync();


                    if (!foods.Any())
                    {
                        /*return await GetAllFoodsForMemberAsync();*/

                        foods = await(from food in context.Foods
                                       join diet in context.Diets on food.DietId equals diet.DietId
                                       where food.Status == true
                                       select new AllFoodForMemberResponseDTO
                                       {
                                           FoodId = food.FoodId,
                                           FoodName = food.FoodName,
                                           FoodImage = food.FoodImage,
                                           Calories = food.Calories,
                                           Fat = food.Fat,
                                           Carbs = food.Carbs,
                                           Protein = food.Protein,
                                           DietName = food.Diet.DietName,
                                       }).ToListAsync();
                    }

                    return foods;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UploadImageFood(string urlImage, int foodId)
        {
            try
            {
                using var context = new HealthTrackingDBContext();


                var mealMember = await context.Foods
                    .FirstOrDefaultAsync(m => m.FoodId == foodId);

                if (mealMember == null)
                {
                    throw new Exception("MealMember not found.");
                }


                mealMember.FoodImage = urlImage;


                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error uploading image for meal member: {ex.Message}", ex);
            }
        }

        public async Task<int> GetTotalFoodsForStaffAsync()
        {
            using (var context = new HealthTrackingDBContext())
            {
                return await context.Foods.CountAsync();
            }
        }
    }
}
