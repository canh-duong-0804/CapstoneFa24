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


    }
}
