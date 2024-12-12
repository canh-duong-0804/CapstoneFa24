using BusinessObject.Dto.ExecriseDiary;
using BusinessObject.Dto.Exericse;
using BusinessObject.Dto.Food;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Http;

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
                    var exercise = await context.Exercises
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
                    var exercise = await context.Exercises
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
        public async Task<List<ExerciseDiaryForAllMonthDTO>> GetAllDiariesForMonthOfExercise(DateTime date, int memberId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    var startOfMonth = new DateTime(date.Year, date.Month, 1);
                    var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);


                    var diaries = await context.ExerciseDiaries
                        .Where(fd => fd.Date >= startOfMonth && fd.Date <= endOfMonth && fd.MemberId == memberId )
                        .Select(fd => new ExerciseDiaryForAllMonthDTO
                        {
                            Date = fd.Date.Value,
                            exerciseDiaryId = fd.ExerciseDiaryId,
                            HasExercise = fd.ExerciseDiaryDetails.Any(detail => detail.IsPractice == true)

                        })
                        .ToListAsync();

                    return diaries;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching food diaries for the month: {ex.Message}", ex);
            }
        }

        public async Task<bool> addExerciseListToDiaryForWebsite(AddExerciseDiaryDetailForWebsiteRequestDTO request, int memberId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    // Retrieve the exercise diary for the specified member and date.
                    var exerciseDiary = await context.ExerciseDiaries
                        .FirstOrDefaultAsync(ed => ed.MemberId == memberId && ed.Date.Value.Date == request.selectDate.Date);

                    if (exerciseDiary == null)
                    {
                        // If no exercise diary exists, create a new one.
                        await GetOrCreateExerciseDiaryAsync(memberId, request.selectDate.Date);
                        exerciseDiary = await context.ExerciseDiaries
                            .FirstOrDefaultAsync(ed => ed.MemberId == memberId && ed.Date.Value.Date == request.selectDate.Date);
                    }

                    // Delete all existing exercise diary details for the given exercise diary.
                    var existingDetails = await context.ExerciseDiaryDetails
                        .Where(edd => edd.ExerciseDiaryId == exerciseDiary.ExerciseDiaryId)
                        .ToListAsync();

                    context.ExerciseDiaryDetails.RemoveRange(existingDetails);

                    // Add the new exercise items to the exercise diary.
                    foreach (var exerciseItem in request.ListExerciseIdToAdd)
                    {
                        var newDetail = new ExerciseDiaryDetail
                        {
                            ExerciseDiaryId = exerciseDiary.ExerciseDiaryId,
                            ExerciseId = exerciseItem.ExerciseId,
                            Duration = exerciseItem.DurationInMinutes,
                            IsPractice = exerciseItem.IsPractice,
                            CaloriesBurned = exerciseItem.CaloriesBurned
                        };
                        await context.ExerciseDiaryDetails.AddAsync(newDetail);
                    }

                    // Save the changes to the database.
                    await context.SaveChangesAsync();

                    // Recalculate the exercise diary's total calories burned based on the new details.
                    var exerciseDiaryDetails = await context.ExerciseDiaryDetails
                        .Where(edd => edd.ExerciseDiaryId == exerciseDiary.ExerciseDiaryId)
                        .ToListAsync();

                    exerciseDiary.TotalCaloriesBurned = Math.Round((double)exerciseDiaryDetails.Sum(d => d.CaloriesBurned), 1);
                    exerciseDiary.TotalDuration = (int?)Math.Round((double)exerciseDiaryDetails.Sum(d => d.Duration), 1);

                    // Save the updated exercise diary.
                    await context.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding exercise list to diary: {ex.Message}", ex);
            }
        }

        private async Task GetOrCreateExerciseDiaryAsync(int memberId, DateTime date)
        {
            using (var context = new HealthTrackingDBContext())
            {
                // Check if an exercise diary already exists for the given memberId and date
                var existingDiary = await context.ExerciseDiaries
                    .FirstOrDefaultAsync(ed => ed.MemberId == memberId && ed.Date.Value.Date == date.Date);

                if (existingDiary == null)
                {
                    // If the diary doesn't exist, create a new exercise diary
                    var newDiary = new ExerciseDiary
                    {
                        MemberId = memberId,
                        Date = date,
                        TotalCaloriesBurned = 0 // Initialize TotalCaloriesBurned to 0 or any default value
                    };

                    await context.ExerciseDiaries.AddAsync(newDiary);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task<AddExerciseDiaryDetailForWebsiteRequestDTO> GetExerciseDairyDetailWebsite(int memberId, DateTime selectDate)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    // Retrieve the exercise diary for the given memberId and selectDate
                    var exerciseDiary = await context.ExerciseDiaries
                        .FirstOrDefaultAsync(ed => ed.MemberId == memberId && ed.Date.Value.Date == selectDate.Date);

                    // If the diary doesn't exist, return null or handle as needed
                    if (exerciseDiary == null)
                    {
                        return null;
                    }

                    // Retrieve the exercise diary details associated with this exercise diary
                    var exerciseDiaryDetails = await context.ExerciseDiaryDetails
                        .Where(edd => edd.ExerciseDiaryId == exerciseDiary.ExerciseDiaryId)
                        .Select(edd => new ExerciseDiaryDetailForWebisteRequestDTO
                        {
                            ExerciseId = edd.ExerciseId,
                            DurationInMinutes = edd.Duration,
                            IsPractice = (bool)edd.IsPractice,
                            CaloriesBurned = (float)edd.CaloriesBurned
                        })
                        .ToListAsync();

                    // Map the retrieved data to AddExerciseDiaryDetailForWebsiteRequestDTO
                    var result = new AddExerciseDiaryDetailForWebsiteRequestDTO
                    {
                        //ExerciseDiaryId = exerciseDiary.ExerciseDiaryId,
                        selectDate = exerciseDiary.Date.Value.Date,
                        ListExerciseIdToAdd = exerciseDiaryDetails
                    };

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching exercise diary details: {ex.Message}", ex);
            }
        }

        public async Task<List<ExerciseListBoxResponseDTO>> GetListBoxExerciseForStaffAsync()
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    // Fetch the exercises that are active
                    var exerciseslist = await context.Exercises
                        .Where(e => e.Status == true)
                        .ToListAsync();

                    // Create dictionaries for quick lookup of cardio and resistance details by ExerciseId
                    var cardioDetails = await context.ExerciseCardios
                        .Where(c => exerciseslist.Select(e => e.ExerciseId).Contains(c.ExerciseId))
                        .ToDictionaryAsync(c => c.ExerciseId);

                    var resistanceDetails = await context.ExerciseResistances
                        .Where(r => exerciseslist.Select(e => e.ExerciseId).Contains(r.ExerciseId))
                        .ToDictionaryAsync(r => r.ExerciseId);

                    // Project exercises to DTOs
                    var exercises = exerciseslist.Select(exercise => new ExerciseListBoxResponseDTO
                    {
                        Value = exercise.ExerciseId,
                        Label = exercise.ExerciseName,
                        ExerciseName = exercise.ExerciseName,
                        MetValue = exercise.MetValue,
                        TypeExercise = exercise.TypeExercise,
                        NameTypeExercise = exercise.TypeExercise == 1 ? "Cardio" :
                                           exercise.TypeExercise == 2 ? "Kháng lực" :
                                           exercise.TypeExercise == 3 ? "Các bài tập khác" :
                                           "Unknown",

                        // Using dictionary lookups to fetch cardio and resistance details
                        CaloriesCadior1 = cardioDetails.ContainsKey(exercise.ExerciseId) ? cardioDetails[exercise.ExerciseId].Calories1 : null,
                        CaloriesCadior2 = cardioDetails.ContainsKey(exercise.ExerciseId) ? cardioDetails[exercise.ExerciseId].Calories2 : null,
                        CaloriesCadior3 = cardioDetails.ContainsKey(exercise.ExerciseId) ? cardioDetails[exercise.ExerciseId].Calories3 : null,
                        MinutesCadior1 = cardioDetails.ContainsKey(exercise.ExerciseId) ? cardioDetails[exercise.ExerciseId].Minutes1 : null,
                        MinutesCadior2 = cardioDetails.ContainsKey(exercise.ExerciseId) ? cardioDetails[exercise.ExerciseId].Minutes2 : null,
                        MinutesCadior3 = cardioDetails.ContainsKey(exercise.ExerciseId) ? cardioDetails[exercise.ExerciseId].Minutes3 : null,

                        MinutesResitance1 = resistanceDetails.ContainsKey(exercise.ExerciseId) ? resistanceDetails[exercise.ExerciseId].Minutes1 : null,
                        MinutesResitance2 = resistanceDetails.ContainsKey(exercise.ExerciseId) ? resistanceDetails[exercise.ExerciseId].Minutes2 : null,
                        MinutesResitance3 = resistanceDetails.ContainsKey(exercise.ExerciseId) ? resistanceDetails[exercise.ExerciseId].Minutes3 : null,
                        RepsResitance1 = resistanceDetails.ContainsKey(exercise.ExerciseId) ? resistanceDetails[exercise.ExerciseId].Reps1 : null,
                        RepsResitance2 = resistanceDetails.ContainsKey(exercise.ExerciseId) ? resistanceDetails[exercise.ExerciseId].Reps2 : null,
                        RepsResitance3 = resistanceDetails.ContainsKey(exercise.ExerciseId) ? resistanceDetails[exercise.ExerciseId].Reps3 : null,
                        SetsResitance1 = resistanceDetails.ContainsKey(exercise.ExerciseId) ? resistanceDetails[exercise.ExerciseId].Sets1 : null,
                        SetsResitance2 = resistanceDetails.ContainsKey(exercise.ExerciseId) ? resistanceDetails[exercise.ExerciseId].Sets2 : null,
                        SetsResitance3 = resistanceDetails.ContainsKey(exercise.ExerciseId) ? resistanceDetails[exercise.ExerciseId].Sets3 : null,
                    }).ToList();

                    return exercises;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving exercises: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteExerciseDiaryDetailWebsite(DateTime selectDate, int memberId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    // Retrieve the exercise diary for the specified member and date.
                    var exerciseDiary = await context.ExerciseDiaries
                        .FirstOrDefaultAsync(ed => ed.MemberId == memberId && ed.Date.Value.Date == selectDate.Date);

                    if (exerciseDiary == null)
                    {
                        // If no exercise diary exists, create a new one.
                        await GetOrCreateExerciseDiaryAsync(memberId, selectDate.Date);
                        exerciseDiary = await context.ExerciseDiaries
                            .FirstOrDefaultAsync(ed => ed.MemberId == memberId && ed.Date.Value.Date == selectDate.Date);
                    }

                    // Delete all existing exercise diary details for the given exercise diary.
                    var existingDetails = await context.ExerciseDiaryDetails
                        .Where(edd => edd.ExerciseDiaryId == exerciseDiary.ExerciseDiaryId)
                        .ToListAsync();

                    context.ExerciseDiaryDetails.RemoveRange(existingDetails);

                    
                    await context.SaveChangesAsync();

                    // Recalculate the exercise diary's total calories burned based on the new details.
                    var exerciseDiaryDetails = await context.ExerciseDiaryDetails
                        .Where(edd => edd.ExerciseDiaryId == exerciseDiary.ExerciseDiaryId)
                        .ToListAsync();

                    exerciseDiary.TotalCaloriesBurned = Math.Round((double)exerciseDiaryDetails.Sum(d => d.CaloriesBurned), 1);
                    exerciseDiary.TotalDuration = (int?)Math.Round((double)exerciseDiaryDetails.Sum(d => d.Duration), 1);

                    // Save the updated exercise diary.
                    await context.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding exercise list to diary: {ex.Message}", ex);
            }
        }
    }
}
