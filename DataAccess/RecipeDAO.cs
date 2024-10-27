using BusinessObject.Dto.Recipe;
using BusinessObject.Dto.Staff;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class RecipeDAO
    {
        private static RecipeDAO instance = null;
        private static readonly object instanceLock = new object();

        public static RecipeDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new RecipeDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<IEnumerable<AllRecipeForStaffResponseDTO>> GetAllRecipesForStaffAsync()
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var recipes = await context.Recipes
                        .Where(s => s.Status == true)
                        .Include(s => s.CreateByNavigation)
                        .Include(s => s.Food)
                        .Select(s => new AllRecipeForStaffResponseDTO
                        {
                            RecipeId = s.RecipeId,
                            FoodName = s.Food.FoodName,
                            CreateBy = s.CreateByNavigation.FullName,
                            CreateDate = s.CreateDate,
                            RecipeImage = s.RecipeImage,
                            ChangeBy = s.CreateByNavigation.FullName,
                            ChangeDate = s.ChangeDate,
                            RecipeName = s.RecipeName,
                            Status = s.Status
                        })
                        .ToListAsync();

                    return recipes;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving recipes: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<AllRecipeForMemberResponseDTO>> GetAllRecipesForMemberAsync()
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var recipes = await context.Recipes
                        .Where(s => s.Status == true)
                        .Include(s => s.CreateByNavigation)
                        .Include(s => s.Food)
                        .Select(s => new AllRecipeForMemberResponseDTO
                        {
                            RecipeId = s.RecipeId,
                            FoodName = s.Food.FoodName,
                            //CreateBy = s.CreateByNavigation.FullName,
                            CreateDate = s.CreateDate,
                            RecipeImage = s.RecipeImage,
                            ChangeBy = s.CreateByNavigation.FullName,
                            ChangeDate = s.ChangeDate,
                            RecipeName = s.RecipeName,
                           
                        })
                        .ToListAsync();

                    return recipes;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving recipes: {ex.Message}", ex);
            }
        }

        public async Task<Recipe> CreateRecipeAsync(Recipe recipeModel)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    await context.Recipes.AddAsync(recipeModel);


                    await context.SaveChangesAsync();

                    return recipeModel;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating food: {ex.Message}", ex);
            }
        }
    }
}
