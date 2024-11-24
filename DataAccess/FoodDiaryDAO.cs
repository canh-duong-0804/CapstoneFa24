using AutoMapper.Execution;
using BusinessObject.Dto.Food;
using BusinessObject.Dto.FoodDiary;
using BusinessObject.Dto.FoodDiaryDetails;
using BusinessObject.Dto.MainDashBoardMobile;
using BusinessObject.Dto.Streak;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class FoodDiaryDAO
    {
        private static FoodDiaryDAO instance = null;
        private static readonly object instanceLock = new object();

        public static FoodDiaryDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new FoodDiaryDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<FoodDiary> GetOrCreateFoodDiaryByDate(int memberId, DateTime date)
        {

            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    // Kiểm tra xem có FoodDiary nào đã tồn tại không
                    var foodDiary = await context.FoodDiaries
                    .Include(fd => fd.FoodDiaryDetails)
                    .FirstOrDefaultAsync(fd => fd.MemberId == memberId && fd.Date.Date == date.Date);


                    /*if (foodDiary == null)
                    {
                        var mainDashboard = await MainDashBoardMobileDAO.Instance.GetMainDashBoardForMemberById(memberId);
                        double dailyCalories = mainDashboard.DailyCalories;
                        foodDiary = new FoodDiary
                        {
                            MemberId = memberId,
                            Date = date,
                            GoalCalories = dailyCalories,
                            Calories = 0,
                            Protein = 0,
                            Fat = 0,
                            Carbs = 0,
                            // FoodDiaryDetails = new List<FoodDiaryDetail>()
                        };


                        context.FoodDiaries.Add(foodDiary);
                        await context.SaveChangesAsync();
                    }*/
                    return foodDiary;
                }


            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while retrieving or creating the food diary.", ex);
            }
        }

        public async Task<UpdateFoodDiaryRequestDTO> UpdateFoodDiary(UpdateFoodDiaryRequestDTO updatedFoodDiary)
        {
            try
            {

                using (var context = new HealthTrackingDBContext())
                {

                    var foodDiary = await context.FoodDiaries

                        .FirstOrDefaultAsync(fd => fd.DiaryId == updatedFoodDiary.DiaryId);

                    if (foodDiary == null)
                    {

                        return null;
                    }


                    foodDiary.GoalCalories = updatedFoodDiary.GoalCalories;
                    foodDiary.Calories = updatedFoodDiary.Calories;
                    foodDiary.Protein = updatedFoodDiary.Protein;
                    foodDiary.Fat = updatedFoodDiary.Fat;
                    foodDiary.Carbs = updatedFoodDiary.Carbs;


                    await context.SaveChangesAsync();
                    return updatedFoodDiary;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);

            }
        }


        public async Task<bool> AddFoodListToDiaryAsync(FoodDiaryDetailRequestDTO foodDetails)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {


                    var foodDiaryDetail = new FoodDiaryDetail
                    {
                        DiaryId = foodDetails.DiaryId,
                        FoodId = foodDetails.FoodId,
                        Quantity = foodDetails.Quantity,
                        MealType = foodDetails.MealType,

                    };
                    await context.FoodDiaryDetails.AddAsync(foodDiaryDetail);
                    await context.SaveChangesAsync();


                    var foodDiaryDetails = await context.FoodDiaryDetails
                                                        .Include(fdd => fdd.Food)
                                                        .Where(fdd => fdd.DiaryId == foodDetails.DiaryId)
                                                        .ToListAsync();


                    double totalCalories = foodDiaryDetails.Sum(detail => detail.Food.Calories * detail.Quantity);
                    double totalProtein = foodDiaryDetails.Sum(detail => detail.Food.Protein * detail.Quantity);
                    double totalFat = foodDiaryDetails.Sum(detail => detail.Food.Fat * detail.Quantity);
                    double totalCarbs = foodDiaryDetails.Sum(detail => detail.Food.Carbs * detail.Quantity);


                    var foodDiary = await context.FoodDiaries.FirstOrDefaultAsync(fd => fd.DiaryId == foodDetails.DiaryId);
                    if (foodDiary != null)
                    {
                        foodDiary.Calories = Math.Round(totalCalories, 1);
                        foodDiary.Protein = Math.Round(totalProtein, 1);
                        foodDiary.Fat = Math.Round(totalFat, 1);
                        foodDiary.Carbs = Math.Round(totalCarbs, 1);

                        await context.SaveChangesAsync();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while adding food items to diary: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteFoodListToDiaryAsync(int DiaryDetailId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    var foodItem = await context.FoodDiaryDetails
                        .FirstOrDefaultAsync(fd => fd.DiaryDetailId == DiaryDetailId);

                    if (foodItem == null)
                    {

                        return false;
                    }


                    var foodDiary = await context.FoodDiaries
                        .FirstOrDefaultAsync(fd => fd.DiaryId == foodItem.DiaryId);

                    if (foodDiary == null)
                    {
                        return false;
                    }


                    context.FoodDiaryDetails.Remove(foodItem);
                    await context.SaveChangesAsync();


                    var foodDiaryDetails = await context.FoodDiaryDetails
                        .Include(fdd => fdd.Food)
                        .Where(fdd => fdd.DiaryId == foodItem.DiaryId)
                        .ToListAsync();

                    double totalCalories = foodDiaryDetails.Sum(detail => detail.Food.Calories * detail.Quantity);
                    double totalProtein = foodDiaryDetails.Sum(detail => detail.Food.Protein * detail.Quantity);
                    double totalFat = foodDiaryDetails.Sum(detail => detail.Food.Fat * detail.Quantity);
                    double totalCarbs = foodDiaryDetails.Sum(detail => detail.Food.Carbs * detail.Quantity);


                    foodDiary.Calories = Math.Round(totalCalories, 1);
                    foodDiary.Protein = Math.Round(totalProtein, 1);
                    foodDiary.Fat = Math.Round(totalFat, 1);
                    foodDiary.Carbs = Math.Round(totalCarbs, 1);

                    await context.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting food item: {ex.Message}");
                return false;
            }
        }


        public async Task getDailyFoodDiaryFollowMeal(int dairyID, int mealType)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {


                    var foodDiary = await context.FoodDiaryDetails
                     .Include(fd => fd.Food)
                    .FirstOrDefaultAsync(fd => fd.DiaryId == dairyID && fd.MealType == mealType);

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);

            }
        }
        public async Task<MainDashResponseDTO> GetFoodDairyDetailById(int memberId, DateTime date)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var foodDiary = await context.FoodDiaries
                        .FirstOrDefaultAsync(fd => fd.MemberId == memberId && fd.Date.Date == date.Date);

                    if (foodDiary == null)
                    {
                        await GetOrCreateFoodDiaryAsync(memberId, date);
                        foodDiary = await context.FoodDiaries
                            .FirstOrDefaultAsync(fd => fd.MemberId == memberId && fd.Date.Date == date.Date);
                    }

                    async Task<List<FoodDiaryForMealResponseDTO>> GetFoodDiaryDetailsByMealType(int mealType)
                    {
                        return await context.FoodDiaryDetails
                            .Include(e => e.Food)
                            .Where(e => e.DiaryId == foodDiary.DiaryId && e.MealType == mealType)
                            .Select(e => new FoodDiaryForMealResponseDTO
                            {
                                DiaryDetailId = e.DiaryDetailId,
                                DiaryId = e.DiaryId,
                                FoodId = e.FoodId,
                                FoodName = e.Food.FoodName,
                                Calories = e.Food.Calories,
                                Protein = e.Food.Protein,
                                Carbs = e.Food.Carbs,
                                Fat = e.Food.Fat,
                                Quantity = e.Quantity,
                                MealType = e.MealType,
                                Portion = e.Food.Portion,

                            })
                            .ToListAsync();
                    }

                    var hasDetails = await context.FoodDiaryDetails
                        .AnyAsync(fd => fd.DiaryId == foodDiary.DiaryId);

                    var response = new MainDashResponseDTO
                    {
                        foodDiaryInforMember = new FoodDiaryResponseDTO
                        {
                            DiaryId = foodDiary.DiaryId,
                            MemberId = foodDiary.MemberId,
                            Date = foodDiary.Date,
                            GoalCalories = foodDiary.GoalCalories,
                            Calories = foodDiary.Calories,
                            Protein = foodDiary.Protein,
                            Fat = foodDiary.Fat,
                            Carbs = foodDiary.Carbs
                        }
                    };

                    if (hasDetails)
                    {
                        response.foodDiaryForMealBreakfast = await GetFoodDiaryDetailsByMealType(1);
                        response.foodDiaryForMealLunch = await GetFoodDiaryDetailsByMealType(2);
                        response.foodDiaryForMealDinner = await GetFoodDiaryDetailsByMealType(3);
                        response.foodDiaryForMealSnack = await GetFoodDiaryDetailsByMealType(4);
                    }
                    else
                    {
                        response.foodDiaryForMealBreakfast = new List<FoodDiaryForMealResponseDTO>();
                        response.foodDiaryForMealLunch = new List<FoodDiaryForMealResponseDTO>();
                        response.foodDiaryForMealDinner = new List<FoodDiaryForMealResponseDTO>();
                        response.foodDiaryForMealSnack = new List<FoodDiaryForMealResponseDTO>();
                    }

                    return response;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving food diary details.", ex);
            }
        }


        public async Task<FoodDiaryForMemberMobileResponse> GetFoodDairyByDate(int memberId, DateTime date)
        {
            try
            {

                using (var context = new HealthTrackingDBContext())
                {

                    var getDiary = await context.FoodDiaries
                        .FirstOrDefaultAsync(fd => fd.MemberId == memberId && fd.Date.Date == date.Date);
                    if (getDiary == null) return null;
                    return new FoodDiaryForMemberMobileResponse
                    {
                        DiaryId = getDiary.DiaryId,
                        Date = getDiary.Date,
                        Calories = getDiary.Calories,
                        Protein = getDiary.Protein,
                        Fat = getDiary.Fat,
                        Carbs = getDiary.Carbs,


                    };

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task GetOrCreateFoodDiaryAsync(int memberId, DateTime date)
        {
            using (var context = new HealthTrackingDBContext())
            {
                /*var foodDiary = await context.FoodDiaries
            .Include(fd => fd.FoodDiaryDetails)
            .FirstOrDefaultAsync(fd => fd.MemberId == memberId && fd.Date.Date == date.Date);
*/
                /* if (foodDiary == null)
                 {*/
                var member = await context.Members
                    .Include(m => m.Goals)
                    .FirstOrDefaultAsync(m => m.MemberId == memberId);

                if (member == null)
                    throw new Exception("Member not found");


                var age = DateTime.Now.Year - member.Dob.Year;


                var latestMeasurement = await context.BodyMeasureChanges
                        .Where(b => b.MemberId == memberId)
                        .OrderByDescending(b => b.DateChange.Value.Date)
                        .ThenBy(b => b.DateChange.Value.TimeOfDay)
                        .FirstOrDefaultAsync();

                double currentWeight = latestMeasurement.Weight ?? 0;


                double bmr;
                if (member.Gender == true)
                {
                    bmr = (10 * currentWeight) + (6.25 * (member.Height ?? 0)) - (5 * age) + 5;

                }
                else
                {
                    bmr = (10 * currentWeight) + (6.25 * (member.Height ?? 0)) - (5 * age) - 161;
                }


                double activityMultiplier = member.ExerciseLevel switch
                {
                    1 => 1.2,
                    2 => 1.375,
                    3 => 1.725,

                };

                var maintenanceCalories = bmr * activityMultiplier;
                var foodDiary = new FoodDiary
                {
                    MemberId = memberId,
                    Date = date,
                    GoalCalories = Math.Round(maintenanceCalories, 0),
                    //Calories = Math.Round((maintenanceCalories * 0.3) / 4, 1),
                    Protein = Math.Round((maintenanceCalories * 0.3) / 4, 1),
                    Fat = Math.Round((maintenanceCalories * 0.25) / 9, 1),
                    Carbs = Math.Round((maintenanceCalories * 0.45) / 4, 1)
                };
                context.FoodDiaries.Add(foodDiary);
                await context.SaveChangesAsync();



            }
        }

        public async Task<IEnumerable<AllFoodForMemberResponseDTO>> GetFoodHistoryAsync(int memberId)
        {
            try
            {
                using var context = new HealthTrackingDBContext();

                const int maxAttempts = 3;
                int attempts = 0;
                var foodDetails = new List<AllFoodForMemberResponseDTO>();
                var lastDate = DateTime.Now;

                do
                {
                    attempts++;


                    var foodDiary = await context.FoodDiaries
                        .OrderByDescending(fd => fd.Date)
                        .FirstOrDefaultAsync(fd => fd.MemberId == memberId && fd.Date < lastDate);

                    if (foodDiary == null)
                    {
                        break;
                    }

                    lastDate = foodDiary.Date;


                    var foodIds = await context.FoodDiaryDetails
                        .Where(fdd => fdd.DiaryId == foodDiary.DiaryId)
                        .Select(fdd => fdd.FoodId)
                        .Distinct()
                        .ToListAsync();


                    var currentFoodDetails = await context.Foods
                        .Where(f => foodIds.Contains(f.FoodId))
                        .Include(f => f.Diet)
                        .Select(f => new AllFoodForMemberResponseDTO
                        {
                            FoodId = f.FoodId,
                            FoodName = f.FoodName,
                            FoodImage = f.FoodImage,
                            Calories = f.Calories,
                            Protein = f.Protein,
                            Carbs = f.Carbs,
                            Fat = f.Fat,
                            DietName = f.Diet.DietName
                        })
                        .Take(3)
                        .ToListAsync();


                    foodDetails.AddRange(currentFoodDetails);


                    if (foodDetails.Count >= 3 || attempts >= maxAttempts)
                    {
                        break;
                    }
                }
                while (true);

                return foodDetails;
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error fetching food history: {ex.Message}");
            }
        }


        public async Task<IEnumerable<AllFoodForMemberResponseDTO>> GetFoodSuggestAsync(int memberId)
        {
            try
            {
                using var context = new HealthTrackingDBContext();


                var member = await context.Members
                    .Include(m => m.Diet)
                    .FirstOrDefaultAsync(m => m.MemberId == memberId);


                if (member == null)
                {
                    throw new Exception("Member not found");
                }


                var dietId = member.DietId;


                var foodSuggest = await context.Foods
                    .Where(f => f.DietId == dietId)
                    .Select(f => new AllFoodForMemberResponseDTO
                    {
                        FoodId = f.FoodId,
                        FoodName = f.FoodName,
                        FoodImage = f.FoodImage,
                        Calories = f.Calories,
                        Protein = f.Protein,
                        Carbs = f.Carbs,
                        Fat = f.Fat,
                        DietName = f.Diet.DietName
                    })
                    .Take(3).ToListAsync();

                return foodSuggest;
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error fetching food suggestions: {ex.Message}");
            }
        }

        public async Task<List<GetFoodDiaryDateResponseDTO>> GetFoodDairyDateAsync(int memberId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    var today = DateTime.Now.Date;


                    var diaries = await context.FoodDiaries
                        .Where(fd => fd.MemberId == memberId && fd.Date >= startOfMonth && fd.Date <= today)
                        .OrderByDescending(fd => fd.Date)
                        .Select(getDiary => new GetFoodDiaryDateResponseDTO
                        {
                            DiaryId = getDiary.DiaryId,
                            Date = getDiary.Date,

                        })
                        .ToListAsync();

                    return diaries;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching food diaries: {ex.Message}");
            }
        }

        public async Task<CalorieStreakDTO> GetCalorieStreakAsync(int memberId, DateTime date)
        {
            using (var context = new HealthTrackingDBContext())
            {
                var today = DateTime.Now.Date;

                var startOfMonth = new DateTime(date.Year, date.Month, 1);


                var endDate = today;


                var foodDiaries = await context.FoodDiaries
                    .Where(fd => fd.MemberId == memberId &&
                                 fd.Date.Month == startOfMonth.Month &&
                                 fd.Date.Day <= endDate.Day &&
                                 fd.Date.Year == startOfMonth.Year)  
                    .OrderByDescending(fd => fd.Date)
                    .Select(fd => new { fd.Date, fd.Calories })
                    .ToListAsync();


                var streakDTO = new CalorieStreakDTO();
                var currentStreak = 0;


                var hasEntryForDate = foodDiaries.Any() && foodDiaries.First().Date == endDate;


                DateTime startDate = hasEntryForDate ? endDate : endDate.AddDays(-1);
                DateTime? expectedDate = startDate;

                foreach (var diary in foodDiaries)
                {

                    if (diary.Calories > 0)
                    {
                        streakDTO.Dates.Add(diary.Date.Date);
                    }

                    if (diary.Date.Date == expectedDate)
                    {
                        if (diary.Calories > 0)
                        {
                            currentStreak++;
                        }
                        else
                        {

                            currentStreak = 0;
                        }
                        streakDTO.StreakNumber = Math.Max(streakDTO.StreakNumber, currentStreak);
                        expectedDate = expectedDate.Value.AddDays(-1);


                        if (expectedDate < startOfMonth)
                        {
                            break;
                        }
                    }
                    else if (diary.Date.Date < expectedDate)
                    {

                        currentStreak = 0;
                        if (diary.Calories > 0)
                        {
                            currentStreak = 1;
                        }
                        expectedDate = diary.Date.AddDays(-1);
                    }
                }

                streakDTO.Dates = streakDTO.Dates
                    .Where(d => d >= startOfMonth && d <= endDate)
                    .OrderByDescending(d => d)
                    .ToList();

                return streakDTO;
            }
        }


    }
}