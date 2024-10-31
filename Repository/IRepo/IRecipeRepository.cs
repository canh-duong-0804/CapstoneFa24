using BusinessObject.Dto.Diet;
using BusinessObject.Dto.Food;
using BusinessObject.Dto.Recipe;
using BusinessObject.Dto.Recipe.UpdateDTO;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IRecipeRepository
    {
        Task<Recipe> CreateRecipeAsync(Recipe recipeModel);
        Task<Recipe> DeleteRecipeAsync(int id);
        Task<IEnumerable<AllRecipeForMemberResponseDTO>> GetAllRecipesForMemberAsync();

        /* Task<Recipe> CreateRecipeAsync(Recipe recipe);*/


        Task<IEnumerable<AllRecipeForStaffResponseDTO>> GetAllRecipesForStaffAsync();
        Task<GetRecipeForStaffByIdResponseDTO> GetRecipeForStaffByIdAsync(int id);
        Task<UpdateRecipeRequestDTO> UpdateRecipeAsync(UpdateRecipeRequestDTO recipeUpdate);


        /*  Task<GetRecipeByIdResponseDTO> GetRecipeByIdAsync(int id);


          Task<Recipe> UpdateRecipeAsync(Food blog);


          Task<bool> DeleteFoodAsync(int id);*/

    }
}
