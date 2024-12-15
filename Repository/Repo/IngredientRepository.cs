/*using BusinessObject.Dto.Ingredient;
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
    public class IngredientRepository : IIngredientRepository
    {
        public Task<Ingredient> CreateIngredientModelAsync(Ingredient ingredientModel) => IngredientDAO.Instance.CreateIngredientModelAsync(ingredientModel);


        public Task<IEnumerable<IngredientResponseDTO>> GetAllingredientsAsync() => IngredientDAO.Instance.GetAllingredientsAsync();

        public Task<List<ListBoxIngredientResponseDTO>> GetListBoxIngredientForStaffAsync() => IngredientDAO.Instance.GetListBoxIngredientForStaffAsync();

        public Task<Ingredient> UpdateIngredientAsync(Ingredient ingredientModel) => IngredientDAO.Instance.UpdateIngredientAsync(ingredientModel);

    }
}
*/