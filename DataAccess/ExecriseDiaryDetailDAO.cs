using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ExecriseDiaryDetailDAO
    {
        private static ExecriseDiaryDetailDAO instance = null;
        private static readonly object instanceLock = new object();
        public static ExecriseDiaryDetailDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ExecriseDiaryDetailDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<double> CalculateCaloriesBurnedAsync(int exerciseId, double bodyWeightKg, int durationInMinutes)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    // Retrieve the MET value for the exercise
                    var exercise = await context.ExerciseCardios
                        .FirstOrDefaultAsync(e => e.ExerciseId == exerciseId);

                    if (exercise == null)
                        throw new Exception("Exercise not found.");

                    // Get the MET value
                    double metValue = exercise.MetValue ?? 0;

                    // Apply the formula: (MET * 3.5 * BW (kg) * Duration) / 200
                    double caloriesBurned = (metValue * 3.5 * bodyWeightKg * durationInMinutes) / 200;

                    return caloriesBurned;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating calories burned: {ex.Message}");
                return 0; // Default return if error
            }
        }

        public async Task AddDiaryDetailAsync(ExerciseDiaryDetail diaryDetail)
        {

            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    context.ExerciseDiaryDetails.Add(diaryDetail);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding diary detail: {ex.Message}");
            }
        }
        public async Task<double?> GetLatestBodyWeightAsync(string memberId)
        {

            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var latestBodyMeasure = await context.BodyMeasureChanges
                        .Where(bm => bm.MemberId.ToString() == memberId)

                        .FirstOrDefaultAsync();

                    return latestBodyMeasure?.Weight;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting latest body weight: {ex.Message}");
                return null;
            }
        }

        public async Task<double?> GetExerciseMetValueAsync(int exerciseId)
        {

            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var exercise = await context.ExerciseCardios
                        .FirstOrDefaultAsync(e => e.ExerciseId == exerciseId);

                    return exercise?.MetValue;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting exercise MET value: {ex.Message}");
                return null;
            }
        }

        public async Task<Exercise?> GetExerciseAsync(int exerciseId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var exercise = await context.Exercises
                        .FirstOrDefaultAsync(e => e.ExerciseId == exerciseId);

                    return exercise;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task UpdateExerciseDiaryDetailAsync(ExerciseDiaryDetail detail)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())

                {
                    context.ExerciseDiaryDetails.Update(detail);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<ExerciseDiaryDetail?> GetExerciseDiaryDetailById(int exerciseDiaryDetailId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    return await context.ExerciseDiaryDetails
                        .FirstOrDefaultAsync(detail => detail.ExerciseDiaryDetailsId == exerciseDiaryDetailId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving ExerciseDiaryDetail by ID: {ex.Message}");
                throw;
            }
        }

        public async Task<ExerciseDiaryDetail?> GetDiaryDetailByIdAsync(int diaryDetailId)
        {
            using (var context = new HealthTrackingDBContext())
            {
                return await context.ExerciseDiaryDetails
                    .FirstOrDefaultAsync(dd => dd.ExerciseDiaryDetailsId == diaryDetailId);
            }
        }
        public async Task DeleteDiaryDetailAsync(int diaryDetailId)
        {
            using (var context = new HealthTrackingDBContext())
            {
                var diaryDetail = await context.ExerciseDiaryDetails
                    .FirstOrDefaultAsync(dd => dd.ExerciseDiaryDetailsId == diaryDetailId);

                if (diaryDetail != null)
                {
                    context.ExerciseDiaryDetails.Remove(diaryDetail);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task UpdateDiaryDetailAsync(ExerciseDiaryDetail diaryDetail)
        {
            using (var context = new HealthTrackingDBContext())
            {
                context.ExerciseDiaryDetails.Update(diaryDetail);
                await context.SaveChangesAsync();
            }
        }

        public async Task AssignExercisePlanToUserAsync(int memberId, int planId, DateTime startDate)
        {
            using (var context = new HealthTrackingDBContext())
            {
                // Fetch the exercise plan details
                var planDetails = await context.ExercisePlanDetails
                    .Include(d => d.Exercise)
                    .Where(d => d.ExercisePlanId == planId)
                    .ToListAsync();

                if (planDetails == null || !planDetails.Any())
                {
                    throw new InvalidOperationException("No exercises found in the plan.");
                }

                // Get the latest body weight of the user
                var bodyWeightKg = await GetLatestBodyWeightAsync(memberId.ToString());

                if (bodyWeightKg == null || bodyWeightKg <= 0)
                {
                    throw new InvalidOperationException("Cannot calculate calories burned: user body weight is not available.");
                }

                foreach (var detail in planDetails)
                {
                    // Calculate the target date for the exercise
                    var targetDate = startDate.AddDays(detail.Day - 1); // Assuming Day starts at 1 for Day 1 of the plan

                    // Check if a diary exists for the target date
                    var diary = await context.ExerciseDiaries
                        .FirstOrDefaultAsync(d => d.MemberId == memberId && d.Date == targetDate);

                    if (diary == null)
                    {
                        // Create a new diary entry if it doesn't exist
                        diary = new ExerciseDiary
                        {
                            MemberId = memberId,
                            Date = targetDate,
                            TotalDuration = 0,
                            TotalCaloriesBurned = 0,
                            ExercisePlanId = planId
                        };

                        await context.ExerciseDiaries.AddAsync(diary);
                        await context.SaveChangesAsync(); // Save here to generate ExerciseDiaryId
                    }

                    // Calculate calories burned for the exercise
                    var caloriesBurned = await CalculateCaloriesBurnedAsync(
                        detail.ExerciseId,
                        bodyWeightKg.Value,
                        detail.Duration
                    );

                    // Add the exercise detail to the diary
                    var diaryDetail = new ExerciseDiaryDetail
                    {
                        ExerciseDiaryId = diary.ExerciseDiaryId,
                        ExerciseId = detail.ExerciseId,
                        Duration = detail.Duration,
                        CaloriesBurned = caloriesBurned,
                        IsPractice = false
                    };

                    await context.ExerciseDiaryDetails.AddAsync(diaryDetail);

                   
                }

                await context.SaveChangesAsync();
            }
        }


    }
}
