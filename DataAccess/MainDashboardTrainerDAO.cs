using BusinessObject.Dto.MainDashBoardAdmin;
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



                    /////
                    // Total food diary entries
                    var totalEntriesExercise = await context.ExerciseDiaryDetails.CountAsync();

                    if (totalEntriesExercise == 0)
                    {
                        throw new Exception("No food diary details found.");
                    }

                    // Group and order foods by usage
                    var groupedExercises = await context.ExerciseDiaryDetails
                        .Include(fdd => fdd.Exercise) // Include the related Food entity
                        .GroupBy(fdd => new { fdd.ExerciseId, fdd.Exercise.ExerciseName }) // Group by FoodId and FoodName
                        .Select(group => new
                        {
                            ExerciseId = group.Key.ExerciseId,
                            ExerciseName = group.Key.ExerciseName, // Include FoodName in the result
                            Count = group.Count()
                        })
                        .OrderByDescending(f => f.Count)
                        .ToListAsync();

                    // Get the top 5 foods
                    var topExericses = groupedExercises.Take(5).ToList();

                    // Calculate total usage of top 5 foods
                    var totalTop5ExerciseUsage = topExericses.Sum(f => f.Count);

                    // Calculate the usage percentage relative to the top 5 foods
                    var topExeriseStatistics = topExericses
                        .Select(f => new TopExeriseStatisticsDTO
                        {
                            ExerciseId = f.ExerciseId,
                            ExerciseName = f.ExerciseName, // Map the FoodName
                            UsageCount = f.Count,
                            UsagePercentage = Math.Round((f.Count / (double)totalTop5ExerciseUsage) * 100, 2)
                        })
                        .ToList();

                    ////

                    // Calculate counts for each diet type
                    var totalMemberUseEatClean = await context.Members.CountAsync(m => m.DietId == 4);
                    var totalMemberUseVegetarian = await context.Members.CountAsync(m => m.DietId == 3);
                    var totalMemberUseLowCarb = await context.Members.CountAsync(m => m.DietId == 2);
                    var totalMemberUseNormal = await context.Members.CountAsync(m => m.DietId == 1);

                    // Calculate total members
                    var totalMembers = totalMemberUseEatClean + totalMemberUseVegetarian + totalMemberUseLowCarb + totalMemberUseNormal;

                    // Avoid division by zero
                    if (totalMembers == 0)
                    {
                        return new MainDashBoardInfoForTrainerDTO
                        {
                            TotalFoods = totalFoods,
                            TotalExercises = totalExercises,
                            TotalUsers = totalUsers,
                            TopFoodStatistics = topFoodStatistics,
                            TopExerciseStatistics= topExeriseStatistics,
                            UserRegistrationStatistics = completeUserRegistrations,
                            NewUsersThisMonth = newUsersThisMonth,

                            TotalMemberUseEatClean = 0,
                            TotalMemberUseVegetarian = 0,
                            TotalMemberUseLowCarb = 0,
                            TotalMemberUseNormal = 0,

                            PercentageEatClean = 0,
                            PercentageVegetarian = 0,
                            PercentageLowCarb = 0,
                            PercentageNormal = 0
                        };
                    }

                    // Calculate percentages
                    var percentageEatClean = (double)totalMemberUseEatClean / totalMembers * 100;
                    var percentageVegetarian = (double)totalMemberUseVegetarian / totalMembers * 100;
                    var percentageLowCarb = (double)totalMemberUseLowCarb / totalMembers * 100;
                    var percentageNormal = (double)totalMemberUseNormal / totalMembers * 100;

                    // Return aggregated results with percentages
                    return new MainDashBoardInfoForTrainerDTO
                    {
                        TotalFoods = totalFoods,
                        TotalExercises = totalExercises,
                        TotalUsers = totalUsers,
                       TopFoodStatistics = topFoodStatistics,
                       TopExerciseStatistics=topExeriseStatistics,
                        UserRegistrationStatistics = completeUserRegistrations,
                        NewUsersThisMonth = newUsersThisMonth,

                        TotalMemberUseEatClean = totalMemberUseEatClean,
                        TotalMemberUseVegetarian = totalMemberUseVegetarian,
                        TotalMemberUseLowCarb = totalMemberUseLowCarb,
                        TotalMemberUseNormal = totalMemberUseNormal,

                        PercentageEatClean = percentageEatClean,
                        PercentageVegetarian = percentageVegetarian,
                        PercentageLowCarb = percentageLowCarb,
                        PercentageNormal = percentageNormal
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
