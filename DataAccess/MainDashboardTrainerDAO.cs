using BusinessObject.Dto.MainDashBoardTrainer;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MainDashboardTrainerDAO
    {
        private static MainDashboardTrainerDAO instance = null;
        private static readonly object instanceLock = new object();

        public static MainDashboardTrainerDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new MainDashboardTrainerDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<MainDashBoardInfoForTrainerDTO> GetAllInformationForMainTrainer(DateTime selectDate)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    // Total foods
                    var totalFoods = await context.Foods.CountAsync(f => f.Status == true);

                    // Total exercises
                    var totalExercises = await context.Exercises.CountAsync();

                    // Total users
                    var totalUsers = await context.Members.CountAsync();

                    // New users registered this month
                    var newUsersThisMonth = await context.Members
                        .CountAsync(m => m.CreatedAt.Month == selectDate.Month && m.CreatedAt.Year == selectDate.Year);

                    // User registrations grouped by month
                    var monthsInYear = Enumerable.Range(1, 12); // Generate months 1 to 12

                    var userRegistrations = await context.Members
                        .Where(m => m.CreatedAt.Year == selectDate.Year)
                        .GroupBy(m => m.CreatedAt.Month)
                        .Select(g => new
                        {
                            Month = g.Key, // Month number (1-12)
                            UserCount = g.Count()
                        })
                        .ToListAsync();

                    // Ensure all months are present, filling in 0 for missing months
                    var completeUserRegistrations = monthsInYear
                        .GroupJoin(
                            userRegistrations,
                            month => month,
                            reg => reg.Month,
                            (month, regGroup) => new UserRegistrationStatisticsDTO
                            {
                                Month = month,
                                UserCount = regGroup.FirstOrDefault()?.UserCount ?? 0 // Use 0 if no data for the month
                            })
                        .OrderBy(stat => stat.Month)
                        .ToList();

                    // Total food diary entries
                    var totalEntries = await context.FoodDiaryDetails.CountAsync();

                    if (totalEntries == 0)
                    {
                        throw new Exception("No food diary details found.");
                    }

                    // Group and order foods by usage
                    var groupedFoods = await context.FoodDiaryDetails
                        .Include(fdd => fdd.Food) // Include the related Food entity
                        .GroupBy(fdd => new { fdd.FoodId, fdd.Food.FoodName }) // Group by FoodId and FoodName
                        .Select(group => new
                        {
                            FoodId = group.Key.FoodId,
                            FoodName = group.Key.FoodName, // Include FoodName in the result
                            Count = group.Count()
                        })
                        .OrderByDescending(f => f.Count)
                        .ToListAsync();

                    // Get the top 5 foods
                    var topFoods = groupedFoods.Take(5).ToList();

                    // Calculate total usage of top 5 foods
                    var totalTop5Usage = topFoods.Sum(f => f.Count);

                    // Calculate the usage percentage relative to the top 5 foods
                    var topFoodStatistics = topFoods
                        .Select(f => new TopFoodStatisticsDTO
                        {
                            FoodId = f.FoodId,
                            FoodName = f.FoodName, // Map the FoodName
                            UsageCount = f.Count,
                            UsagePercentage = Math.Round((f.Count / (double)totalTop5Usage) * 100, 2)
                        })
                        .ToList();



                    // Return aggregated results
                    return new MainDashBoardInfoForTrainerDTO
                    {
                        TotalFoods = totalFoods,
                        TotalExercises = totalExercises,
                        TotalUsers = totalUsers,
                        TopFoodStatistics = topFoodStatistics,
                        UserRegistrationStatistics = completeUserRegistrations,
                        NewUsersThisMonth = newUsersThisMonth
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching dashboard info: {ex.Message}", ex);
            }
        }


    }
}
