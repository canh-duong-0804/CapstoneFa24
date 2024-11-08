using AutoMapper.Execution;
using BusinessObject.Dto.FoodDiary;
using BusinessObject.Dto.FoodDiaryDetails;
using BusinessObject.Dto.MainDashBoardMobile;
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
                        StatusFoodDiary = true
                    };
                    await context.FoodDiaryDetails.AddAsync(foodDiaryDetail);
                    await context.SaveChangesAsync();


                    var foodDiaryDetails = await context.FoodDiaryDetails
               .Include(fdd => fdd.Food)
               .Where(fdd => fdd.DiaryId == foodDetails.DiaryId && fdd.StatusFoodDiary == true)
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
                        .Where(fdd => fdd.DiaryId == foodItem.DiaryId && fdd.StatusFoodDiary == true)
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
                    var getIdDiary = await context.FoodDiaries
                        .FirstOrDefaultAsync(fd => fd.MemberId == memberId && fd.Date.Date == date.Date);

                    if (getIdDiary == null)
                    {
                        GetOrCreateFoodDiaryAsync(memberId, date);
                    }


                    var foodDiary = await context.FoodDiaries
               .Include(fd => fd.FoodDiaryDetails)
               .FirstOrDefaultAsync(fd => fd.MemberId == getIdDiary.MemberId && fd.Date.Date == date.Date);
                    if (getIdDiary == null)
                    {
                        throw new Exception("Diary not found for the given member and date.");
                    }


                    async Task<List<FoodDiaryForMealResponseDTO>> GetFoodDiaryDetailsByMealType(int mealType)
                    {
                        return await context.FoodDiaryDetails.Include(e => e.Food)
                            .Where(e => e.DiaryId == getIdDiary.DiaryId && e.MealType == mealType)
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
                                MealType = e.MealType
                            })
                            .ToListAsync();
                    }

                    var mealTypes = new Dictionary<int, List<FoodDiaryForMealResponseDTO>>
            {
                { 1, await GetFoodDiaryDetailsByMealType(1) },
                { 2, await GetFoodDiaryDetailsByMealType(2) },
                { 3, await GetFoodDiaryDetailsByMealType(3) },
                { 4, await GetFoodDiaryDetailsByMealType(4) }
            };


                    var totalCalories = foodDiary.Calories;
                    var totalProtein = foodDiary.Protein;
                    var totalFat = foodDiary.Fat;
                    var totalCarbs = foodDiary.Carbs;

                    foreach (var mealList in mealTypes.Values)
                    {
                        totalCalories += mealList.Sum(item => item.Calories * item.Quantity);
                        totalProtein += mealList.Sum(item => item.Protein * item.Quantity);
                        totalFat += mealList.Sum(item => item.Fat * item.Quantity);
                        totalCarbs += mealList.Sum(item => item.Carbs * item.Quantity);
                    }


                    return new MainDashResponseDTO
                    {
                        foodDiaryInforMember = new FoodDiaryResponseDTO
                        {
                            DiaryId = getIdDiary.DiaryId,
                            MemberId = getIdDiary.MemberId,
                            Date = getIdDiary.Date,
                            GoalCalories = getIdDiary.GoalCalories,
                            Calories = totalCalories,
                            Protein = totalProtein,
                            Fat = totalFat,
                            Carbs = totalCarbs
                        },
                        foodDiaryForMealBreakfast = mealTypes[1],
                        foodDiaryForMealLunch = mealTypes[2],
                        foodDiaryForMealDinner = mealTypes[3],
                        foodDiaryForMealSnack = mealTypes[4]
                    };
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
                    .OrderByDescending(b => b.DateChange)
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
                    Calories = 0,
                    Protein = 0,
                    Fat = 0,
                    Carbs = 0
                };
                context.FoodDiaries.Add(foodDiary);
                await context.SaveChangesAsync();



            }
        }
    }
}