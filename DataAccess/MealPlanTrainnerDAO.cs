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
                        .Include(s=>s.Diet)
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
                             DietName=s.Diet.DietName

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
                   
                    foreach (var foodDetail in request.FoodIds)
                    {
                        var mealPlanDetail = new MealPlanDetail
                        {
                            MealPlanId = request.MealPlanId,
                            MealType = request.MealType,
                            Day = request.Day,
                            Description = request.Description,
                            FoodId = foodDetail.FoodId, 
                            Quantity = foodDetail.Quantity, 
                          
                        };

                        context.MealPlanDetails.Add(mealPlanDetail);
                        await context.SaveChangesAsync();
                    }

                    
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<bool> UpdateMealPlanDetailAsync(CreateMealPlanDetailRequestDTO request)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    
                    var existingDetails = await context.MealPlanDetails
                        .Where(d => d.MealPlanId == request.MealPlanId &&
                                    d.MealType == request.MealType &&
                                    d.Day == request.Day)
                        .ToListAsync();

                    
                    foreach (var foodDetail in request.FoodIds)
                    {
                        var detail = existingDetails.FirstOrDefault(d => d.FoodId == foodDetail.FoodId);
                        if (detail != null)
                        {
                            
                            detail.Quantity = foodDetail.Quantity;
                            detail.Description = request.Description;
                        }
                        else
                        {
                           
                            context.MealPlanDetails.Add(new MealPlanDetail
                            {
                                MealPlanId = request.MealPlanId,
                                MealType = request.MealType,
                                Day = request.Day,
                                Description = request.Description,
                                FoodId = foodDetail.FoodId,
                                Quantity = foodDetail.Quantity
                            });
                        }
                    }

                 
                    var toRemove = existingDetails
                        .Where(d => !request.FoodIds.Any(f => f.FoodId == d.FoodId))
                        .ToList();

                  
                    context.MealPlanDetails.RemoveRange(toRemove);

                   
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<GetMealPlanDetaiTrainnerlResponseDTO> GetMealPlanDetailAsync(int MealPlanId, int MealType, int Day)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    
                    var mealPlanDetail = await context.MealPlanDetails
                        .Where(d => d.MealPlanId == MealPlanId && d.MealType == MealType && d.Day ==Day)
                        .GroupBy(d => new { d.MealPlanId, d.MealType, d.Day, d.Description })
                        .Select(g => new GetMealPlanDetaiTrainnerlResponseDTO
                        {
                            MealPlanId = g.Key.MealPlanId,
                            MealType = g.Key.MealType,
                            Day = g.Key.Day,
                            Description = g.Key.Description,
                            FoodIds = g.Select(f => new GetFoodInMealPlanResponseDTO
                            {
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
