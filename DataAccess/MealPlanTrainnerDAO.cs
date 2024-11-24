using BusinessObject.Dto.MealMember;
using BusinessObject.Dto.MealPlan;
using BusinessObject.Dto.MealPlanDetail;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MealPlanTrainnerDAO
    {
        private static MealPlanTrainnerDAO instance = null;
        private static readonly object instanceLock = new object();

        public static MealPlanTrainnerDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new MealPlanTrainnerDAO();
                    }
                    return instance;
                }
            }
        }
        public async Task<bool> CreateMealPlanTrainerAsync(MealPlan mealPlanModel)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    context.MealPlans.Add(mealPlanModel);
                    context.SaveChanges();

                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while adding meal plan details with meal type to food diary: {ex.Message}");

            }

        }

        public async Task<int> GetTotalMealPlanAsync()
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    return await context.staffs.CountAsync(s => s.Status == true);

                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error while adding meal plan details with meal type to food diary: {ex.Message}");

            }
        }

        public async Task<IEnumerable<GetAllMealPlanForMemberResponseDTO>> GetAllMealPlanForStaffsAsync(int page, int pageSize)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var mealPlans = await context.MealPlans
                        .Include(s => s.Diet)
                         .Where(s => s.Status == true)
                         .OrderBy(s => s.MealPlanId)
                         .Skip((page - 1) * pageSize)
                         .Take(pageSize)
                         .Select(s => new GetAllMealPlanForMemberResponseDTO
                         {
                             MealPlanId = s.MealPlanId,
                             MealPlanImage = s.MealPlanImage,
                             ShortDescription = s.ShortDescription,
                             Name = s.Name,
                             TotalCalories = s.TotalCalories,
                             DietName = s.Diet.DietName,


                         })
                         .ToListAsync();

                    return mealPlans;

                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error while adding meal plan details with meal type to food diary: {ex.Message}");

            }
        }

        public async Task<bool> UpdateMealPlanTrainerAsync(MealPlan mealPlanModel)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    var mealPlan = await context.MealPlans
                                .Where(s => s.MealPlanId == mealPlanModel.MealPlanId && s.Status == true)
                                 .FirstOrDefaultAsync();


                    if (mealPlan == null)
                    {
                        return false;
                    }


                    mealPlan.ShortDescription = mealPlanModel.ShortDescription;
                    mealPlan.LongDescription = mealPlanModel.LongDescription;
                    mealPlan.Name = mealPlanModel.Name;
                    mealPlan.MealPlanImage = mealPlanModel.MealPlanImage;
                    mealPlan.ChangeBy = mealPlanModel.ChangeBy;
                    mealPlan.ChangeDate = mealPlanModel.ChangeDate;


                    await context.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating staff role: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteMealPlanAsync(int mealPlanId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    var mealPlan = await context.MealPlans
                                .Where(s => s.MealPlanId == mealPlanId && s.Status == true)
                                 .FirstOrDefaultAsync();


                    if (mealPlan == null)
                    {
                        return false;
                    }



                    mealPlan.Status = false;


                    await context.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating staff role: {ex.Message}", ex);
            }
        }

        public async Task<GetMealPlanResponseDTO> GetMealPlanAsync(int mealPlanId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    /*var mealPlans = await context.MealPlans
                         .Where(s => s.MealPlanId == mealPlanId)
                         .Select(s => new GetMealPlanResponseDTO
                         {
                             MealPlanId = s.MealPlanId,
                             MealPlanImage = s.MealPlanImage,
                             ShortDescription = s.ShortDescription,
                             Name = s.Name,
                             TotalCalories = s.TotalCalories,

                         })
                         .FirstOrDefault();*/
                    var mealPlans = await context.MealPlans
                         .Where(s => s.MealPlanId == mealPlanId)
                         .Select(s => new GetMealPlanResponseDTO
                         {
                             MealPlanId = s.MealPlanId,
                             CreateBy = s.CreateByNavigation.FullName,
                             MealPlanImage = s.MealPlanImage,
                             ChangeDate = s.ChangeDate,
                             ChangeBy = s.CreateByNavigation.FullName,

                         })
                        .FirstOrDefaultAsync();
                    return mealPlans;

                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error while adding meal plan details with meal type to food diary: {ex.Message}");

            }
        }

        public async Task<bool> CreateMealPlanDetailAsync(CreateMealPlanDetailRequestDTO request)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    
                    var validFoodIds = await context.Foods.Select(f => f.FoodId).ToListAsync();

                    
                    if (request.ListFoodIdBreakfasts != null && request.ListFoodIdBreakfasts.Any())
                    {
                        foreach (var foodDetail in request.ListFoodIdBreakfasts)
                        {
                            if (!validFoodIds.Contains(foodDetail.FoodId))
                            {
                                throw new Exception($"Invalid FoodId {foodDetail.FoodId} for Breakfast.");
                            }

                            var newMealPlanDetail = new MealPlanDetail
                            {
                                MealPlanId = request.MealPlanId,
                                MealType = 1, 
                                Day = request.Day,
                                MealDate = DateTime.Now,
                                Description = request.DescriptionBreakFast,
                                FoodId = foodDetail.FoodId,
                                Quantity = foodDetail.Quantity
                            };
                            context.MealPlanDetails.Add(newMealPlanDetail);
                        }
                    }

                   
                    if (request.ListFoodIdLunches != null && request.ListFoodIdLunches.Any())
                    {
                        foreach (var foodDetail in request.ListFoodIdLunches)
                        {
                            if (!validFoodIds.Contains(foodDetail.FoodId))
                            {
                                throw new Exception($"Invalid FoodId {foodDetail.FoodId} for Lunch.");
                            }

                            var newMealPlanDetail = new MealPlanDetail
                            {
                                MealPlanId = request.MealPlanId,
                                MealType = 2, // Lunch
                                Day = request.Day,
                                MealDate = DateTime.Now,
                                Description = request.DescriptionLunch,
                                FoodId = foodDetail.FoodId,
                                Quantity = foodDetail.Quantity
                            };
                            context.MealPlanDetails.Add(newMealPlanDetail);
                        }
                    }

                    
                    if (request.ListFoodIdDinners != null && request.ListFoodIdDinners.Any())
                    {
                        foreach (var foodDetail in request.ListFoodIdDinners)
                        {
                            if (!validFoodIds.Contains(foodDetail.FoodId))
                            {
                                throw new Exception($"Invalid FoodId {foodDetail.FoodId} for Dinner.");
                            }

                            var newMealPlanDetail = new MealPlanDetail
                            {
                                MealPlanId = request.MealPlanId,
                                MealType = 3, // Dinner
                                Day = request.Day,
                                MealDate = DateTime.Now,
                                Description = request.DescriptionDinner,
                                FoodId = foodDetail.FoodId,
                                Quantity = foodDetail.Quantity
                            };
                            context.MealPlanDetails.Add(newMealPlanDetail);
                        }
                    }

                 
                    if (request.ListFoodIdSnacks != null && request.ListFoodIdSnacks.Any())
                    {
                        foreach (var foodDetail in request.ListFoodIdSnacks)
                        {
                            if (!validFoodIds.Contains(foodDetail.FoodId))
                            {
                                throw new Exception($"Invalid FoodId {foodDetail.FoodId} for Snack.");
                            }

                            var newMealPlanDetail = new MealPlanDetail
                            {
                                MealPlanId = request.MealPlanId,
                                MealType = 4, // Snack
                                Day = request.Day,
                                MealDate = DateTime.Now,
                                Description = request.DescriptionSnack,
                                FoodId = foodDetail.FoodId,
                                Quantity = foodDetail.Quantity
                            };
                            context.MealPlanDetails.Add(newMealPlanDetail);
                        }
                    }

                    // Save changes to the database
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating meal plan details.", ex);
            }
        }









        public async Task<bool> UpdateMealPlanDetailAsync(CreateMealPlanDetailRequestDTO request)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    // Meal types and associated data
                    var mealTypes = new List<(int MealType, string? Description, List<dynamic>? FoodDetails)>
            {
                (1, request.DescriptionBreakFast, request.ListFoodIdBreakfasts?.Cast<dynamic>().ToList()),
                (2, request.DescriptionLunch, request.ListFoodIdLunches?.Cast<dynamic>().ToList()),
                (3, request.DescriptionDinner, request.ListFoodIdDinners?.Cast<dynamic>().ToList()),
                (4, request.DescriptionSnack, request.ListFoodIdSnacks?.Cast<dynamic>().ToList())
            };

                    foreach (var (mealType, description, foodDetails) in mealTypes)
                    {
                        // Skip if the food details list is null or empty
                        if (foodDetails == null || !foodDetails.Any())
                        {
                            continue;
                        }

                        // Fetch existing meal plan details for the given meal type and day
                        var existingDetails = await context.MealPlanDetails
                            .Where(d => d.MealPlanId == request.MealPlanId && d.MealType == mealType && d.Day == request.Day)
                            .ToListAsync();

                        // Update or add new records
                        foreach (var foodDetail in foodDetails)
                        {
                            int foodId = mealType switch
                            {
                                1 => foodDetail.FoodIdBreakfast,
                                2 => foodDetail.FoodIdLunch,
                                3 => foodDetail.FoodIdDinner,
                                4 => foodDetail.FoodIdSnack,
                                _ => throw new InvalidOperationException("Invalid meal type.")
                            };

                            var detail = existingDetails.FirstOrDefault(d => d.FoodId == foodId);
                            if (detail != null)
                            {
                                // Update existing record
                                detail.Quantity = foodDetail.Quantity;
                                detail.Description = description;
                            }
                            else
                            {
                                // Add new record
                                context.MealPlanDetails.Add(new MealPlanDetail
                                {
                                    MealPlanId = request.MealPlanId,
                                    MealType = mealType,
                                    Day = request.Day,
                                    MealDate = DateTime.Now,
                                    Description = description,
                                    FoodId = foodId,
                                    Quantity = foodDetail.Quantity
                                });
                            }
                        }

                        // Remove records that are no longer part of the updated meal plan
                        var toRemove = existingDetails
                            .Where(d => !foodDetails.Any(f =>
                                (mealType == 1 && f.FoodIdBreakfast == d.FoodId) ||
                                (mealType == 2 && f.FoodIdLunch == d.FoodId) ||
                                (mealType == 3 && f.FoodIdDinner == d.FoodId) ||
                                (mealType == 4 && f.FoodIdSnack == d.FoodId)))
                            .ToList();

                        context.MealPlanDetails.RemoveRange(toRemove);
                    }

                    // Save changes to the database
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the meal plan details.", ex);
            }
        }







        public async Task<GetMealPlanDetaiTrainnerlResponseDTO> GetMealPlanDetailAsync(int MealPlanId , int Day)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    var mealPlanDetail = await context.MealPlanDetails
                        .Where(d => d.MealPlanId == MealPlanId  && d.Day == Day)
                        .GroupBy(d => new { d.MealPlanId, d.Day, d.Description })
                        .Select(g => new GetMealPlanDetaiTrainnerlResponseDTO
                        {
                            MealPlanId = g.Key.MealPlanId,
                           
                            Day = g.Key.Day,
                            Description = g.Key.Description,
                            FoodIds = g.Select(f => new GetFoodInMealPlanResponseDTO
                            {
                                MealType = f.MealType,
                                FoodId = f.FoodId,
                                FoodName = f.Food.FoodName,
                                Quantity = f.Quantity,
                                Calories = f.Food.Calories,
                                FoodImage = f.Food.FoodImage,
                                Protein = f.Food.Protein,
                                Carbs = f.Food.Carbs,
                                Fat = f.Food.Fat
                            }).ToList()
                        })
                        .FirstOrDefaultAsync();

                    return mealPlanDetail;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
