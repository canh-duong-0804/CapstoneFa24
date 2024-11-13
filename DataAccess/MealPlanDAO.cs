using BusinessObject.Dto.MealPlan;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MealPlanDAO
    {
        private static MealPlanDAO instance = null;
        private static readonly object instanceLock = new object();

        public static MealPlanDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new MealPlanDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<IEnumerable<GetAllMealPlanForMemberResponseDTO>> GetAllMealPlansForMemberAsync()
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    return await context.MealPlans
                    .Select(mp => new GetAllMealPlanForMemberResponseDTO
                    {
                        MealPlanId = mp.MealPlanId,
                        Name = mp.Name,
                        MealPlanImage = mp.MealPlanImage,
                        TotalCalories = mp.TotalCalories,

                    })
                    .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<bool> AddMealPlanToFoodDiaryAsync(int mealPlanId, int memberId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var today = DateTime.Today;

                    var mealPlan = await context.MealPlans.FirstOrDefaultAsync(mp => mp.MealPlanId == mealPlanId);
                    if (mealPlan == null)
                    {
                        throw new Exception("Meal plan not found.");
                    }

                    var mealPlanDetails = await context.MealPlanDetails
                        .Where(mpd => mpd.MealPlanId == mealPlanId)
                        .ToListAsync();


                    foreach (var detail in mealPlanDetails)
                    {

                        var targetDate = today.AddDays(detail.Day - 1);


                        var targetDiary = await context.FoodDiaries
                            .FirstOrDefaultAsync(fd => fd.MemberId == memberId && fd.Date == targetDate);

                        if (targetDiary == null)
                        {
                            targetDiary = new FoodDiary
                            {
                                MemberId = memberId,
                                Date = targetDate,
                                MealPlanId = mealPlanId,
                                GoalCalories = mealPlan.TotalCalories,
                                Calories = 0,
                                Protein = 0,
                                Fat = 0,
                                Carbs = 0
                            };
                            context.FoodDiaries.Add(targetDiary);
                            await context.SaveChangesAsync();
                        }
                        else
                        {

                            targetDiary.MealPlanId = mealPlanId;


                            var existingDetails = context.FoodDiaryDetails
                                .Where(fdd => fdd.DiaryId == targetDiary.DiaryId);
                            context.FoodDiaryDetails.RemoveRange(existingDetails);
                        }

                        var foodDiaryDetail = new FoodDiaryDetail
                        {
                            DiaryId = targetDiary.DiaryId,
                            FoodId = detail.FoodId,
                            Quantity = detail.Quantity,
                            MealType = detail.MealType,
                            StatusFoodDiary = true
                        };
                        context.FoodDiaryDetails.Add(foodDiaryDetail);
                    }


                    await context.SaveChangesAsync();


                    var diariesToUpdate = await context.FoodDiaries
                        .Where(fd => fd.MemberId == memberId && fd.Date >= today && fd.Date < today.AddDays(7))
                        .ToListAsync();

                    foreach (var diary in diariesToUpdate)
                    {
                        var diaryDetails = await context.FoodDiaryDetails
                            .Where(fdd => fdd.DiaryId == diary.DiaryId)
                            .ToListAsync();

                        diary.Calories = diaryDetails.Sum(dd => dd.Quantity * (context.Foods.FirstOrDefault(f => f.FoodId == dd.FoodId)?.Calories ?? 0));
                        diary.Protein = diaryDetails.Sum(dd => dd.Quantity * (context.Foods.FirstOrDefault(f => f.FoodId == dd.FoodId)?.Protein ?? 0));
                        diary.Fat = diaryDetails.Sum(dd => dd.Quantity * (context.Foods.FirstOrDefault(f => f.FoodId == dd.FoodId)?.Fat ?? 0));
                        diary.Carbs = diaryDetails.Sum(dd => dd.Quantity * (context.Foods.FirstOrDefault(f => f.FoodId == dd.FoodId)?.Carbs ?? 0));


                        context.FoodDiaries.Update(diary);
                    }


                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating diary with meal plan: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> GetMealPlanDetailForMemberAsync(int mealPlanId, int day)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var mealPlanDetail= context.MealPlanDetails.Where(mp=>mp.MealPlanId==mealPlanId).ToListAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating diary with meal plan: {ex.Message}");
                return false;
            }
        }
    }
}
