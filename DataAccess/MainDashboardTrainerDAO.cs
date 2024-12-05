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
                    var totalFoods = await context.Foods.CountAsync(f => f.Status == true);
                    var totalExercises = await context.Exercises.CountAsync();
                    var totalUsers = await context.Members.CountAsync();
                    var newUsersThisMonth = await context.Members
                        .CountAsync(m => m.CreatedAt.Month == selectDate.Month && m.CreatedAt.Year == selectDate.Year);

                    var userRegistrations = await context.Members
                        .Where(m => m.CreatedAt.Year == selectDate.Year)
                        .GroupBy(m => m.CreatedAt.Month)
                        .Select(g => new { Month = g.Key, UserCount = g.Count() })
                        .ToListAsync();

                    var completeUserRegistrations = Enumerable.Range(1, 12)
                        .GroupJoin(
                            userRegistrations,
                            month => month,
                            reg => reg.Month,
                            (month, regGroup) => new UserRegistrationStatisticsDTO
                            {
                                Month = month,
                                UserCount = regGroup.FirstOrDefault()?.UserCount ?? 0
                            })
                        .OrderBy(stat => stat.Month)
                        .ToList();

                    var topFoodStatistics = await context.FoodDiaryDetails
                        .Include(fdd => fdd.Food)
                        .GroupBy(fdd => new { fdd.FoodId, fdd.Food.FoodName })
                        .Select(group => new
                        {
                            FoodId = group.Key.FoodId,
                            FoodName = group.Key.FoodName,
                            Count = group.Count()
                        })
                        .OrderByDescending(f => f.Count)
                        .Take(5)
                        .ToListAsync();

                    var totalTop5Usage = topFoodStatistics.Sum(f => f.Count);
                    var processedTopFoods = topFoodStatistics
                        .Select(f => new TopFoodStatisticsDTO
                        {
                            FoodId = f.FoodId,
                            FoodName = f.FoodName,
                            UsageCount = f.Count,
                            UsagePercentage = Math.Round((f.Count / (double)totalTop5Usage) * 100, 2)
                        })
                        .ToList();

                    var topExercises = await context.ExerciseDiaryDetails
                        .Include(fdd => fdd.Exercise)
                        .GroupBy(fdd => new { fdd.ExerciseId, fdd.Exercise.ExerciseName })
                        .Select(group => new
                        {
                            ExerciseId = group.Key.ExerciseId,
                            ExerciseName = group.Key.ExerciseName,
                            Count = group.Count()
                        })
                        .OrderByDescending(f => f.Count)
                        .Take(5)
                        .ToListAsync();

                    var totalTop5ExerciseUsage = topExercises.Sum(f => f.Count);
                    var processedTopExercises = topExercises
                        .Select(f => new TopExeriseStatisticsDTO
                        {
                            ExerciseId = f.ExerciseId,
                            ExerciseName = f.ExerciseName,
                            UsageCount = f.Count,
                            UsagePercentage = Math.Round((f.Count / (double)totalTop5ExerciseUsage) * 100, 2)
                        })
                        .ToList();

                    var dietStats = await context.Members
                        .GroupBy(m => m.DietId)
                        .Select(g => new { DietId = g.Key, Count = g.Count() })
                        .ToListAsync();

                    var totalMembers = dietStats.Sum(d => d.Count);

                    if (totalMembers == 0)
                    {
                        return new MainDashBoardInfoForTrainerDTO
                        {
                            TotalFoods = totalFoods,
                            TotalExercises = totalExercises,
                            TotalUsers = totalUsers,
                            TopFoodStatistics = processedTopFoods,
                            TopExerciseStatistics = processedTopExercises,
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

                    return new MainDashBoardInfoForTrainerDTO
                    {
                        TotalFoods = totalFoods,
                        TotalExercises = totalExercises,
                        TotalUsers = totalUsers,
                        TopFoodStatistics = processedTopFoods,
                        TopExerciseStatistics = processedTopExercises,
                        UserRegistrationStatistics = completeUserRegistrations,
                        NewUsersThisMonth = newUsersThisMonth,
                        TotalMemberUseEatClean = dietStats.FirstOrDefault(d => d.DietId == 4)?.Count ?? 0,
                        TotalMemberUseVegetarian = dietStats.FirstOrDefault(d => d.DietId == 3)?.Count ?? 0,
                        TotalMemberUseLowCarb = dietStats.FirstOrDefault(d => d.DietId == 2)?.Count ?? 0,
                        TotalMemberUseNormal = dietStats.FirstOrDefault(d => d.DietId == 1)?.Count ?? 0,
                        PercentageEatClean = dietStats.FirstOrDefault(d => d.DietId == 4)?.Count * 100.0 / totalMembers ?? 0,
                        PercentageVegetarian = dietStats.FirstOrDefault(d => d.DietId == 3)?.Count * 100.0 / totalMembers ?? 0,
                        PercentageLowCarb = dietStats.FirstOrDefault(d => d.DietId == 2)?.Count * 100.0 / totalMembers ?? 0,
                        PercentageNormal = dietStats.FirstOrDefault(d => d.DietId == 1)?.Count * 100.0 / totalMembers ?? 0
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching dashboard info: {ex.Message}", ex);
            }
        }

        public async Task<MainDashBoardInfoForTrainerFoodDTO> GetMainDashBoardForFoodTrainer(DateTime selectDate)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var totalFoods = await context.Foods.CountAsync(f => f.Status == true);

                    var userRegistrations = await context.Members
                        .Where(m => m.CreatedAt.Year == selectDate.Year)
                        .GroupBy(m => m.CreatedAt.Month)
                        .Select(g => new { Month = g.Key, UserCount = g.Count() })
                        .ToListAsync();

                    var topFoodStatistics = await context.FoodDiaryDetails
                        .Include(fdd => fdd.Food)
                        .GroupBy(fdd => new { fdd.FoodId, fdd.Food.FoodName })
                        .Select(group => new
                        {
                            FoodId = group.Key.FoodId,
                            FoodName = group.Key.FoodName,
                            Count = group.Count()
                        })
                        .OrderByDescending(f => f.Count)
                        .Take(5)
                        .ToListAsync();

                    var totalTop5Usage = topFoodStatistics.Sum(f => f.Count);

                    var processedTopFoods = topFoodStatistics
                        .Select(f => new TopFoodStatisticsDTO
                        {
                            FoodId = f.FoodId,
                            FoodName = f.FoodName,
                            UsageCount = f.Count,
                            UsagePercentage = Math.Round((f.Count / (double)totalTop5Usage) * 100, 2)
                        })
                        .ToList();

                    var topExercises = await context.ExerciseDiaryDetails
                        .Include(fdd => fdd.Exercise)
                        .GroupBy(fdd => new { fdd.ExerciseId, fdd.Exercise.ExerciseName })
                        .Select(group => new
                        {
                            ExerciseId = group.Key.ExerciseId,
                            ExerciseName = group.Key.ExerciseName,
                            Count = group.Count()
                        })
                        .OrderByDescending(f => f.Count)
                        .Take(5)
                        .ToListAsync();

                    var dietStats = await context.Members
                        .GroupBy(m => m.DietId)
                        .Select(g => new
                        {
                            DietId = g.Key,
                            Count = g.Count()
                        })
                        .ToListAsync();

                    var totalMembers = dietStats.Sum(d => d.Count);

                    if (totalMembers == 0)
                    {
                        return new MainDashBoardInfoForTrainerFoodDTO
                        {
                            TotalFoods = totalFoods,
                            TopFoodStatistics = processedTopFoods,
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

                    return new MainDashBoardInfoForTrainerFoodDTO
                    {
                        TotalFoods = totalFoods,
                        TopFoodStatistics = processedTopFoods,
                        TotalMemberUseEatClean = dietStats.FirstOrDefault(d => d.DietId == 4)?.Count ?? 0,
                        TotalMemberUseVegetarian = dietStats.FirstOrDefault(d => d.DietId == 3)?.Count ?? 0,
                        TotalMemberUseLowCarb = dietStats.FirstOrDefault(d => d.DietId == 2)?.Count ?? 0,
                        TotalMemberUseNormal = dietStats.FirstOrDefault(d => d.DietId == 1)?.Count ?? 0,
                        PercentageEatClean = dietStats.FirstOrDefault(d => d.DietId == 4)?.Count * 100.0 / totalMembers ?? 0,
                        PercentageVegetarian = dietStats.FirstOrDefault(d => d.DietId == 3)?.Count * 100.0 / totalMembers ?? 0,
                        PercentageLowCarb = dietStats.FirstOrDefault(d => d.DietId == 2)?.Count * 100.0 / totalMembers ?? 0,
                        PercentageNormal = dietStats.FirstOrDefault(d => d.DietId == 1)?.Count * 100.0 / totalMembers ?? 0
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching dashboard info: {ex.Message}", ex);
            }
        }

        public async Task<MainDashBoardInfoForTrainerExerciseDTO> GetMainDashBoardForExerciseTrainer(DateTime selectDate)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                  
                    var totalExercises = await context.Exercises.CountAsync();
                    var topExercises = await context.ExerciseDiaryDetails
                        .Include(fdd => fdd.Exercise)
                        .GroupBy(fdd => new { fdd.ExerciseId, fdd.Exercise.ExerciseName })
                        .Select(group => new
                        {
                            ExerciseId = group.Key.ExerciseId,
                            ExerciseName = group.Key.ExerciseName,
                            Count = group.Count()
                        })
                        .OrderByDescending(f => f.Count)
                        .Take(5)
                        .ToListAsync();

                    var totalTop5ExerciseUsage = topExercises.Sum(f => f.Count);
                    var processedTopExercises = topExercises
                        .Select(f => new TopExeriseStatisticsDTO
                        {
                            ExerciseId = f.ExerciseId,
                            ExerciseName = f.ExerciseName,
                            UsageCount = f.Count,
                            UsagePercentage = Math.Round((f.Count / (double)totalTop5ExerciseUsage) * 100, 2)
                        })
                        .ToList();

                    var dietStats = await context.Members
                        .GroupBy(m => m.DietId)
                        .Select(g => new { DietId = g.Key, Count = g.Count() })
                        .ToListAsync();

                    var totalMembers = dietStats.Sum(d => d.Count);

                    if (totalMembers == 0)
                    {
                        return new MainDashBoardInfoForTrainerExerciseDTO
                        {
                            
                            TotalExercises = totalExercises,
                          
                            TopExerciseStatistics = processedTopExercises,
                           
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

                    return new MainDashBoardInfoForTrainerExerciseDTO
                    {
                       
                        TotalExercises = totalExercises,
                     
                        TopExerciseStatistics = processedTopExercises,
                        
                        TotalMemberUseEatClean = dietStats.FirstOrDefault(d => d.DietId == 4)?.Count ?? 0,
                        TotalMemberUseVegetarian = dietStats.FirstOrDefault(d => d.DietId == 3)?.Count ?? 0,
                        TotalMemberUseLowCarb = dietStats.FirstOrDefault(d => d.DietId == 2)?.Count ?? 0,
                        TotalMemberUseNormal = dietStats.FirstOrDefault(d => d.DietId == 1)?.Count ?? 0,
                        PercentageEatClean = dietStats.FirstOrDefault(d => d.DietId == 4)?.Count * 100.0 / totalMembers ?? 0,
                        PercentageVegetarian = dietStats.FirstOrDefault(d => d.DietId == 3)?.Count * 100.0 / totalMembers ?? 0,
                        PercentageLowCarb = dietStats.FirstOrDefault(d => d.DietId == 2)?.Count * 100.0 / totalMembers ?? 0,
                        PercentageNormal = dietStats.FirstOrDefault(d => d.DietId == 1)?.Count * 100.0 / totalMembers ?? 0
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
