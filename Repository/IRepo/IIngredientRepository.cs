using BusinessObject.Dto.Food;
using BusinessObject.Dto.Ingredient;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IIngredientRepository
    {
        Task<Ingredient> CreateIngredientModelAsync(Ingredient ingredientModel);
        Task<IEnumerable<IngredientResponseDTO>> GetAllingredientsAsync();
        Task<List<ListBoxIngredientResponseDTO>> GetListBoxIngredientForStaffAsync();
        Task<Ingredient> UpdateIngredientAsync(Ingredient ingredientModel);
    }
}
