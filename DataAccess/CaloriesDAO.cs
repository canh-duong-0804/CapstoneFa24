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
    public class CaloriesDAO
    {
        private static CaloriesDAO instance = null;
        private static readonly object instanceLock = new object();

        public static CaloriesDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CaloriesDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<DailyCaloriesDto> CalculateDailyCalories(int memberId, DateTime date)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var query = context.FoodDiaries
                        .Where(d => d.MemberId == memberId && d.Date.Date == date.Date)
                        .Include(d => d.FoodDiaryDetails)
                        .SelectMany(d => d.FoodDiaryDetails);

                    // Calories by Meal
                    var caloriesByMeal = await query
                        .GroupBy(fd => fd.MealType)
                        .Select(g => new
                        {
                            MealType = g.Key,
                            Calories = g.Sum(fd => fd.Quantity * fd.Food.Calories)
                        })
                        .ToDictionaryAsync(g => g.MealType, g => g.Calories);

                    // Ensure all meal types are included with default 0 for missing ones
                    var caloriesByMealWithNames = Enum.GetValues(typeof(MealTypeEnum))
                        .Cast<int>()
                        .ToDictionary(
                            mealType => ((MealTypeEnum)mealType).ToString(),
                            mealType => caloriesByMeal.ContainsKey(mealType) ? caloriesByMeal[mealType] : 0
                        );

                    // Total Calories
                    var totalCalories = caloriesByMeal.Values.Sum();

                    // Goal Calories
                    var goalCalories = await context.FoodDiaries
                        .Where(d => d.MemberId == memberId && d.Date.Date == date.Date)
                        .Select(d => d.GoalCalories)
                        .FirstOrDefaultAsync() ?? 0;

                    // Net Calories
                    var caloriesBurned = await context.ExerciseDiaries
                        .Where(e => e.MemberId == memberId && e.Date.Value.Date == date.Date)
                        .Select(e => e.TotalCaloriesBurned)
                        .SumAsync() ?? 0; // Default to 0 if no exercise data is found

                    double netCalories = totalCalories - caloriesBurned;

                    // Foods with Highest Calories
                    var foodsWithHighestCalories = await query
                        .Select(fd => new FoodCaloriesDto
                        {
                            FoodDiaryDetailId = fd.DiaryDetailId,
                            FoodName = fd.Food.FoodName,
                            Calories = fd.Quantity * fd.Food.Calories
                        })
                        .OrderByDescending(f => f.Calories)
                        .ToListAsync();

                    var highestCalorieFood = foodsWithHighestCalories.OrderByDescending(fd => fd.FoodDiaryDetailId).FirstOrDefault();

                    return new DailyCaloriesDto
                    {
                        CaloriesByMeal = caloriesByMealWithNames,
                        TotalCalories = totalCalories,
                        NetCalories = netCalories,
                        GoalCalories = goalCalories,
                        FoodsWithHighestCalories = highestCalorieFood
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating daily calories: {ex.Message}");
                return null;
            }
        }


    }
}
