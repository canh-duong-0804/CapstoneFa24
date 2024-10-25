using BusinessObject.Dto.Recipe;
using BusinessObject.Models;
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
        public Task<Recipe> CreateRecipeAsync(Recipe recipe)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteFoodAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AllRecipeForStaffResponseDTO>> GetAllFoodsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<GetRecipeByIdResponseDTO> GetRecipeByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Recipe> UpdateRecipeAsync(Food blog)
        {
            throw new NotImplementedException();
        }
    }
}
