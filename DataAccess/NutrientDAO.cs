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
    public class NutrientDAO
    {
        private static NutrientDAO instance = null;
        private static readonly object instanceLock = new object();

        public static NutrientDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new NutrientDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<DailyNutritionDto> CalculateDailyNutrition(int memberId, DateTime date)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    var dailyNutrition = await context.FoodDiaries
                                    .Where(d => d.MemberId == memberId && d.Date.Date == date.Date)
                                    .SelectMany(d => d.FoodDiaryDetails)
                                    .GroupBy(fd => 1)
                                    .Select(g => new DailyNutritionDto
                                    {
                                        TotalCalories = g.Sum(fd => fd.Quantity * fd.Food.Calories),
                                        TotalProtein = g.Sum(fd => fd.Quantity * fd.Food.Protein),
                                        TotalCarbs = g.Sum(fd => fd.Quantity * fd.Food.Carbs),
                                        TotalFat = g.Sum(fd => fd.Quantity * fd.Food.Fat),
                                        TotalVitaminA = g.Sum(fd => fd.Quantity * fd.Food.VitaminA),
                                        TotalVitaminC = g.Sum(fd => fd.Quantity * fd.Food.VitaminC),
                                        TotalVitaminD = g.Sum(fd => fd.Quantity * fd.Food.VitaminD),
                                        TotalVitaminE = g.Sum(fd => fd.Quantity * fd.Food.VitaminE),
                                        TotalVitaminB1 = g.Sum(fd => fd.Quantity * fd.Food.VitaminB1),
                                        TotalVitaminB2 = g.Sum(fd => fd.Quantity * fd.Food.VitaminB2),
                                        TotalVitaminB3 = g.Sum(fd => fd.Quantity * fd.Food.VitaminB3)
                                    })
                                    .FirstOrDefaultAsync();

                    return dailyNutrition ?? new DailyNutritionDto();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating daily nutrition: {ex.Message}");
                return null; // Default return if error
            }
        }
    }
}
