using BusinessObject.Dto.Food;
using BusinessObject.Dto.Ingredient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IIngredientRepository
    {
        Task<IEnumerable<IngredientResponseDTO>> GetAllingredientsAsync();
    }
}
