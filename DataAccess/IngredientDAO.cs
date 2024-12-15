/*using BusinessObject.Dto.Ingredient;
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
                using (var context = new HealthTrackingDBContext())
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

        public async Task<List<ListBoxIngredientResponseDTO>> GetListBoxIngredientForStaffAsync()
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var ingredients = await context.Ingredients
                                    .Select(i => new ListBoxIngredientResponseDTO
                                    {
                                        Value = i.IngredientId,
                                        Label = i.Name,

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

        public async Task<Ingredient> CreateIngredientModelAsync(Ingredient ingredientModel)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    await context.Ingredients.AddAsync(ingredientModel);


                    await context.SaveChangesAsync();

                    return ingredientModel;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating food: {ex.Message}", ex);
            }
        }

        public async Task<Ingredient> UpdateIngredientAsync(Ingredient ingredientModel)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    var existingIngredient = await context.Ingredients.FindAsync(ingredientModel.IngredientId);

                    if (existingIngredient == null) return null;



                    existingIngredient.Name = ingredientModel.Name;
                    existingIngredient.Description = ingredientModel.Description;



                    await context.SaveChangesAsync();


                    return existingIngredient;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating food: {ex.Message}", ex);
            }
        }
    }
}*/