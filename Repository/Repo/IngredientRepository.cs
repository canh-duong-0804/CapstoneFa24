using BusinessObject.Dto.Ingredient;
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
        public Task<IEnumerable<IngredientResponseDTO>> GetAllingredientsAsync() => IngredientDAO.Instance.GetAllingredientsAsync();


    }
}
