using BusinessObject.Dto.Food;
using BusinessObject.Dto.MealDetailMember;
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
                        ShortDescription = mp.ShortDescription,
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

        public async Task<MealPlanDetailResponseDTO> GetMealPlanDetailForMemberAsync(int mealPlanId, int day)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var mealPlanData = await context.MealPlans
                        .Where(mp => mp.MealPlanId == mealPlanId)
                        .Select(mp => new
                        {
                            mp.MealPlanId,
                            mp.MealPlanImage,
                            mp.DietId,
                            mp.ShortDescription,
                            mp.LongDescription,
                            mp.Name,
                            mp.TotalCalories,
                            BreakfastFoods = mp.MealPlanDetails
                                .Where(mpd => mpd.Day == day && mpd.MealType == 1)
                                .Select(mpd => new GetAllFoodOfMealMemberResonseDTO
                                {
                                    FoodId = mpd.FoodId,
                                    FoodName = mpd.Food.FoodName,
                                    FoodImage = mpd.Food.FoodImage,
                                    Calories = mpd.Food.Calories,
                                    Protein = mpd.Food.Protein,
                                    Carbs = mpd.Food.Carbs,
                                    Fat = mpd.Food.Fat,
                                    DietName = mpd.Food.Diet.DietName,
                                    Quantity = mpd.Quantity
                                }).ToList(),
                            LunchFoods = mp.MealPlanDetails
                                .Where(mpd => mpd.Day == day && mpd.MealType == 2)
                                .Select(mpd => new GetAllFoodOfMealMemberResonseDTO
                                {
                                    FoodId = mpd.FoodId,
                                    FoodName = mpd.Food.FoodName,
                                    FoodImage = mpd.Food.FoodImage,
                                    Calories = mpd.Food.Calories,
                                    Protein = mpd.Food.Protein,
                                    Carbs = mpd.Food.Carbs,
                                    Fat = mpd.Food.Fat,
                                    DietName = mpd.Food.Diet.DietName,
                                    Quantity = mpd.Quantity
                                }).ToList(),
                            AfternoonSnackFoods = mp.MealPlanDetails
                                .Where(mpd => mpd.Day == day && mpd.MealType == 3)
                                .Select(mpd => new GetAllFoodOfMealMemberResonseDTO
                                {
                                    FoodId = mpd.FoodId,
                                    FoodName = mpd.Food.FoodName,
                                    FoodImage = mpd.Food.FoodImage,
                                    Calories = mpd.Food.Calories,
                                    Protein = mpd.Food.Protein,
                                    Carbs = mpd.Food.Carbs,
                                    Fat = mpd.Food.Fat,
                                    DietName = mpd.Food.Diet.DietName,
                                    Quantity = mpd.Quantity
                                }).ToList(),
                            DinnerFoods = mp.MealPlanDetails
                                .Where(mpd => mpd.Day == day && mpd.MealType == 4)
                                .Select(mpd => new GetAllFoodOfMealMemberResonseDTO
                                {
                                    FoodId = mpd.FoodId,
                                    FoodName = mpd.Food.FoodName,
                                    FoodImage = mpd.Food.FoodImage,
                                    Calories = mpd.Food.Calories,
                                    Protein = mpd.Food.Protein,
                                    Carbs = mpd.Food.Carbs,
                                    Fat = mpd.Food.Fat,
                                    DietName = mpd.Food.Diet.DietName,
                                    Quantity = mpd.Quantity
                                }).ToList()
                        })
                        .FirstOrDefaultAsync();

                    if (mealPlanData == null)
                    {
                        return null;
                    }

                    string? GetMainFoodImage(List<GetAllFoodOfMealMemberResonseDTO> foods) =>
                        foods.OrderByDescending(f => f.Calories).FirstOrDefault()?.FoodImage;

                    var response = new MealPlanDetailResponseDTO
                    {
                        MealPlanId = mealPlanData.MealPlanId,
                        MealPlanImage = mealPlanData.MealPlanImage,
                        DietId = mealPlanData.DietId,
                        ShortDescription = mealPlanData.ShortDescription,
                        LongDescription = mealPlanData.LongDescription,
                        Name = mealPlanData.Name,
                        TotalCalories = mealPlanData.TotalCalories,

                        BreakfastFoods = mealPlanData.BreakfastFoods,
                        MainFoodImageForBreakfast = GetMainFoodImage(mealPlanData.BreakfastFoods),

                        LunchFoods = mealPlanData.LunchFoods,
                        MainFoodImageForLunch = GetMainFoodImage(mealPlanData.LunchFoods),

                        DinnerFoods = mealPlanData.DinnerFoods,
                        MainFoodImageForDinner = GetMainFoodImage(mealPlanData.DinnerFoods),

                        SnackFoods = mealPlanData.AfternoonSnackFoods,
                        MainFoodImageForSnack = GetMainFoodImage(mealPlanData.AfternoonSnackFoods),
                    };

                    return response;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<GetAllMealPlanForMemberResponseDTO>> SearchMealPlanForMemberAsync(string? mealPlanName)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                   
                    if (string.IsNullOrWhiteSpace(mealPlanName))
                    {
                       
                        return await (from mp in context.MealPlans
                                      where mp.Status == true
                                      select new GetAllMealPlanForMemberResponseDTO
                                      {
                                          MealPlanId = mp.MealPlanId,
                                          Name = mp.Name,
                                          ShortDescription = mp.ShortDescription,
                                          MealPlanImage = mp.MealPlanImage,
                                          TotalCalories = mp.TotalCalories
                                      }).ToListAsync();
                    }

                  
                    var query = from mp in context.MealPlans
                                where mp.Status == true && EF.Functions.Collate(mp.Name.ToLower(), "Vietnamese_CI_AI").Contains(mealPlanName.ToLower())
                                select new GetAllMealPlanForMemberResponseDTO
                                {
                                    MealPlanId = mp.MealPlanId,
                                    Name = mp.Name,
                                    ShortDescription = mp.ShortDescription,
                                    MealPlanImage = mp.MealPlanImage,
                                    TotalCalories = mp.TotalCalories
                                };

                    var result = await query.ToListAsync();

                    
                    if (!result.Any())
                    {
                        return await GetAllMealPlansForMemberAsync();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }

}

