using BusinessObject.Dto.Recipe;
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
    public class RecipeRepository : IRecipeRepository
    {
        public Task<Recipe> CreateRecipeAsync(Recipe recipeModel) => RecipeDAO.Instance.CreateRecipeAsync(recipeModel);
       

        public Task<IEnumerable<AllRecipeForMemberResponseDTO>> GetAllRecipesForMemberAsync() => RecipeDAO.Instance.GetAllRecipesForMemberAsync();


        public Task<IEnumerable<AllRecipeForStaffResponseDTO>> GetAllRecipesForStaffAsync() => RecipeDAO.Instance.GetAllRecipesForStaffAsync();


    }
}
