using BusinessObject.Dto.Ingredient;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class IngredientDAO
    {
        private static IngredientDAO instance = null;
        private static readonly object instanceLock = new object();

        public static IngredientDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new IngredientDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<IEnumerable<IngredientResponseDTO>> GetAllingredientsAsync()
        {
            try
            {
                using(var context = new HealthTrackingDBContext())
                {
                    var ingredients = await context.Ingredients
                                    .Select(i => new IngredientResponseDTO
                                    {
                                        IngredientId = i.IngredientId,
                                        Name = i.Name,
                                      
                                    })
                                    .ToListAsync();

                    return ingredients;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
