/*using BusinessObject.Dto.Recipe;
using BusinessObject.Dto.Recipe.CreateDTO;
using BusinessObject.Dto.Recipe.UpdateDTO;
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


                    *//* foreach (var ingredient in recipeModel.RecipeIngredients)
                     {
                         ingredient.RecipeId = recipeModel.RecipeId; 
                         ingredient.RecipeIngredientId = 0;
                         await context.RecipeIngredients.AddAsync(ingredient);
                     }*//*


                    await context.SaveChangesAsync();

                    return recipeModel;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating recipe: {ex.Message}", ex);
            }
        }

        public async Task<GetRecipeForStaffByIdResponseDTO> GetRecipeForStaffByIdAsync(int recipeId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    // Truy vấn Recipe kèm theo RecipeIngredients và Ingredients
                    var recipe = await context.Recipes
                         .Where(r => r.Status == true)
                        .Include(r => r.RecipeIngredients)
                            .ThenInclude(ri => ri.Ingredient)
                        .FirstOrDefaultAsync(r => r.RecipeId == recipeId);

                    
                    if (recipe == null)
                    {
                        return null;
                    }

                    // Mapping Recipe sang RecipeDetailDTO
                    var recipeDetail = new GetRecipeForStaffByIdResponseDTO
                    {
                        RecipeId = recipe.RecipeId,
                        RecipeName = recipe.RecipeName,
                        FoodName =recipe.Food.FoodName,
                        CreateBy = recipe.CreateByNavigation.FullName,
                        ChangeBy =recipe.CreateByNavigation.FullName,
                        ChangeDate =recipe.ChangeDate,
                        Description = recipe.Description,
                        Instructions = recipe.Instructions,
                        PrepTime = recipe.PrepTime,
                        CookTime = recipe.CookTime,
                        Servings = recipe.Servings,
                        RecipeIngredients = recipe.RecipeIngredients.Select(ri => new RecipeIngredientRequestDTO
                        {
                            IngredientId = ri.IngredientId,
                            // IngredientName = ri.Ingredient.Name,
                            Quantity = ri.Quantity,
                            Unit = ri.Unit,
                            //CaloriesPerUnit = ri.CaloriesPerUnit
                        }).ToList()
                    };

                    return recipeDetail;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving recipe details: {ex.Message}", ex);
            }
        }

        *//*  public Task<> GetRecipeForStaffByIdAsync()
          {
              throw new NotImplementedException();
          }*/
       /* public async Task<GetRecipeForStaffByIdResponseDTO> GetRecipeByIdAsync(int recipeId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    var recipe = await context.Recipes
                        .Include(r => r.RecipeIngredients)
                            .ThenInclude(ri => ri.Ingredient)
                        .FirstOrDefaultAsync(r => r.RecipeId == recipeId);


                    if (recipe == null)
                    {
                        throw new Exception("Recipe not found");
                    }


                    var recipeDetail = new GetRecipeForStaffByIdResponseDTO
                    {
                        RecipeId = recipe.RecipeId,
                        RecipeName = recipe.RecipeName,
                        Description = recipe.Description,
                        Instructions = recipe.Instructions,
                        PrepTime = recipe.PrepTime,
                        CookTime = recipe.CookTime,
                        Servings = recipe.Servings,
                        RecipeIngredients = recipe.RecipeIngredients.Select(ri => new RecipeIngredientRequestDTO
                        {
                            IngredientId = ri.IngredientId,
                            // IngredientName = ri.Ingredient.Name,
                            Quantity = ri.Quantity,
                            Unit = ri.Unit,
                            //CaloriesPerUnit = ri.CaloriesPerUnit
                        }).ToList()
                    };

                    return recipeDetail;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving recipe details: {ex.Message}", ex);
            }
        }*//*

        public async Task<Recipe> DeleteRecipeAsync(int id)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    var recipe = await context.Recipes.FindAsync(id);

                    if (recipe == null)
                    {
                        return null;
                    }


                    recipe.Status = false;

                    await context.SaveChangesAsync();

                    return recipe;

                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<UpdateRecipeRequestDTO?> UpdateRecipeAsync(UpdateRecipeRequestDTO recipeUpdate)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var recipe = await context.Recipes
                        .Include(r => r.RecipeIngredients) // Include RecipeIngredients for updating
                        .FirstOrDefaultAsync(r => r.RecipeId == recipeUpdate.RecipeId);

                    if (recipe == null)
                    {
                        return null;
                    }

                    // Update the main recipe properties
                    recipe.FoodId = recipeUpdate.FoodId;
                    recipe.RecipeImage = recipeUpdate.RecipeImage;
                    //recipe.ChangeBy = recipeUpdate.ChangeBy;
                    recipe.ChangeDate = recipeUpdate.ChangeDate;
                    recipe.RecipeName = recipeUpdate.RecipeName;
                    recipe.Description = recipeUpdate.Description;
                    recipe.Instructions = recipeUpdate.Instructions;
                    recipe.PrepTime = recipeUpdate.PrepTime;
                    recipe.CookTime = recipeUpdate.CookTime;
                    recipe.Servings = recipeUpdate.Servings;

                    
                    var existingIngredientIds = recipe.RecipeIngredients.Select(ri => ri.IngredientId).ToList();
                    var updatedIngredientIds = recipeUpdate.RecipeIngredients.Select(ri => ri.IngredientId).ToList();

                   
                   // recipe.RecipeIngredients.RemoveAll(ri => !updatedIngredientIds.Contains(ri.IngredientId));

                   
                    foreach (var updatedIngredient in recipeUpdate.RecipeIngredients)
                    {
                        var ingredient = recipe.RecipeIngredients.FirstOrDefault(ri => ri.IngredientId == updatedIngredient.IngredientId);
                        if (ingredient != null)
                        {
                            
                            ingredient.Quantity = updatedIngredient.Quantity;
                            ingredient.Unit = updatedIngredient.Unit;
                        }
                        else
                        {
                           
                            recipe.RecipeIngredients.Add(new RecipeIngredient
                            {
                                IngredientId = updatedIngredient.IngredientId,
                                Quantity = updatedIngredient.Quantity,
                                Unit = updatedIngredient.Unit,
                                RecipeId = recipe.RecipeId 
                            });
                        }
                    }

                    await context.SaveChangesAsync();

                    
                    var updatedRecipeDto = new UpdateRecipeRequestDTO
                    {
                        RecipeId = recipe.RecipeId,
                        FoodId = recipe.FoodId,
                        RecipeImage = recipe.RecipeImage,
                        ChangeBy = recipe.CreateByNavigation.FullName,
                        ChangeDate = recipe.ChangeDate,
                        RecipeName = recipe.RecipeName,
                        Description = recipe.Description,
                        Instructions = recipe.Instructions,
                        PrepTime = recipe.PrepTime,
                        CookTime = recipe.CookTime,
                        Servings = recipe.Servings,
                        RecipeIngredients = recipe.RecipeIngredients.Select(ri => new RecipeIngredientRequestDTO
                        {
                            IngredientId = ri.IngredientId,
                            Quantity = ri.Quantity,
                            Unit = ri.Unit,
                        }).ToList()
                    };

                    return updatedRecipeDto;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
*/