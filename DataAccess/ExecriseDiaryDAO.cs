/*using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ExerciseDiaryDAO
    {
        private static ExerciseDiaryDAO instance = null;
        private static readonly object instanceLock = new object();

        public static ExerciseDiaryDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ExerciseDiaryDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task AddExerciseDiary(ExerciseDiary exerciseDiary)
        {
            if (exerciseDiary == null)
                throw new ArgumentNullException(nameof(exerciseDiary), "Exercise diary entry cannot be null");

            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    await context.ExerciseDiaries.AddAsync(exerciseDiary);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating exercise diary entry: {ex.Message}", ex);
            }
        }

        public async Task<List<ExerciseDiary>> GetExerciseDiaryByMemberId(int memberId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    return await context.ExerciseDiaries
                        .Include(e => e.Exercise)
                        .Include(e => e.ExercisePlan)
                        .Include(e => e.Member)
                        .Where(e => e.MemberId == memberId)
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting exercise diary entries: {ex.Message}", ex);
            }
        }

        public async Task AddExerciseDiaries(List<ExerciseDiary> exerciseDiaries)
        {
            if (exerciseDiaries == null || !exerciseDiaries.Any())
                throw new ArgumentNullException(nameof(exerciseDiaries), "Exercise diary entries cannot be null or empty");

            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    await context.ExerciseDiaries.AddRangeAsync(exerciseDiaries);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating exercise diary entries: {ex.Message}", ex);
            }
        }

        // New method to check if ExercisePlan exists
        public async Task<bool> CheckExercisePlanExistsAsync(int exercisePlanId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    return await context.ExercisePlans.AnyAsync(ep => ep.ExercisePlanId == exercisePlanId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking ExercisePlan existence: {ex.Message}", ex);
            }
        }

        // New method to check if Exercise exists
        public async Task<bool> CheckExerciseExistsAsync(int exerciseId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    return await context.Exercises.AnyAsync(e => e.ExerciseId == exerciseId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking Exercise existence: {ex.Message}", ex);
            }
        }


        //create method to delete exercise diary by id	
        public async Task<bool> DeleteExerciseDiary(int exerciseDiaryId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var exerciseDiary = await context.ExerciseDiaries.FindAsync(exerciseDiaryId);
                    // delete exercise diary
                    context.ExerciseDiaries.Remove(exerciseDiary);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public async Task<List<(int ExerciseId, int Duration, float CaloriesPerHour)>> GetExercisesByPlanIdAsync(int exercisePlanId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var result = await context.ExercisePlanDetails
                        .Where(epd => epd.ExercisePlanId == exercisePlanId)
                        .Join(context.Exercises,
                            epd => epd.ExerciseId,
                            e => e.ExerciseId,
                            (epd, e) => new { epd.ExerciseId, epd.Duration, CaloriesPerHour = (float)e.CaloriesPerHour })
                        .ToListAsync();

                    return result.Select(x => (x.ExerciseId, x.Duration, x.CaloriesPerHour)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching exercises for plan {exercisePlanId}: {ex.Message}", ex);
            }
        }


        public async Task<List<(int ExerciseId, int Duration, byte Day, float CaloriesPerHour)>> GetExercisePlanDetailsByPlanIdAsync(int exercisePlanId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var result = await context.ExercisePlanDetails
                        .Where(epd => epd.ExercisePlanId == exercisePlanId)
                        .Join(context.Exercises,
                            epd => epd.ExerciseId,
                            e => e.ExerciseId,
                            (epd, e) => new
                            {
                                epd.ExerciseId,
                                epd.Duration,
                                epd.Day, // Fetching the Day field
                                CaloriesPerHour = (float)e.CaloriesPerHour // Assuming this is available in Exercise
                            })
                        .ToListAsync();

                    return result.Select(x => (x.ExerciseId, x.Duration, x.Day, x.CaloriesPerHour)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching exercise details for plan {exercisePlanId}: {ex.Message}", ex);
            }
        }

        public async Task GetOrCreateExerciseDiaryAsync(int memberId, DateTime date)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var exerciseDiary = new ExerciseDiary
                    {
                        MemberId = memberId,
                        Date = date,
                        CaloriesBurned = 0
                      
                    };

                    context.ExerciseDiaries.Add(exerciseDiary);
                    await context.SaveChangesAsync(); 
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching exercise details for plan: {ex.Message}", ex);
            }
        }
    }
}
*/