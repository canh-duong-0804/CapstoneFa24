using BusinessObject.Dto.Nutrition;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MacroDAO
    {
        private static MacroDAO instance = null;
        private static readonly object instanceLock = new object();

        public static MacroDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new MacroDAO();
                    }
                    return instance;
                }
            }
        }


        public async Task<MacroNutrientDto> GetMacroNutrientsByDate(int memberId, DateTime date)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    // Query food diary details for the given date and member
                    var query = context.FoodDiaries
                        .Where(d => d.MemberId == memberId && d.Date.Date == date.Date)
                        .Include(d => d.FoodDiaryDetails)
                        .SelectMany(d => d.FoodDiaryDetails);

                    // Calculate macronutrient totals
                    var totalCarbs = await query.SumAsync(fd => fd.Quantity * fd.Food.Carbs);
                    var totalFat = await query.SumAsync(fd => fd.Quantity * fd.Food.Fat);
                    var totalProtein = await query.SumAsync(fd => fd.Quantity * fd.Food.Protein);

                    // Find the food highest in carbs, fat, and protein
                    var highestCarbFood = await query
                        .OrderByDescending(fd => fd.Quantity * fd.Food.Carbs) 
                        .Select(fd => new FoodMacroDto
                        {
                            FoodName = fd.Food.FoodName,
                            Quantity = fd.Quantity,
                            MacroValue = fd.Quantity * fd.Food.Carbs
                        })
                        .FirstOrDefaultAsync();

                    var highestFatFood = await query
                        .OrderByDescending(fd => fd.Quantity * fd.Food.Fat)
                        .Select(fd => new FoodMacroDto
                        {
                            FoodName = fd.Food.FoodName,
                            Quantity = fd.Quantity,
                            MacroValue = fd.Quantity * fd.Food.Fat
                        })
                        .FirstOrDefaultAsync();

                    var highestProteinFood = await query
                        .OrderByDescending(fd => fd.Quantity * fd.Food.Protein)
                        .Select(fd => new FoodMacroDto
                        {
                            FoodName = fd.Food.FoodName,
                            Quantity = fd.Quantity,
                            MacroValue = fd.Quantity * fd.Food.Protein
                        })
                        .FirstOrDefaultAsync();

                    // Return the DTO with calculated values
                    return new MacroNutrientDto
                    {
                        TotalCarbs = totalCarbs,
                        TotalFat = totalFat,
                        TotalProtein = totalProtein,
                        HighestCarbFood = highestCarbFood,
                        HighestFatFood = highestFatFood,
                        HighestProteinFood = highestProteinFood
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating macronutrients: {ex.Message}");
                return null;
            }
        }

    }
}
