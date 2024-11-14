using AutoMapper.Execution;
using BusinessObject.Dto.Food;
using BusinessObject.Dto.MealDetailMember;
using BusinessObject.Dto.MealMember;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MealMemberDAO
    {
        private static MealMemberDAO instance = null;
        private static readonly object instanceLock = new object();

        public static MealMemberDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new MealMemberDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<bool> CreateMealPlanDetailsOfMemberAsync(MealMember mealMember)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    await context.MealMembers.AddAsync(mealMember);
                    await context.SaveChangesAsync();



                    await context.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating recipe: {ex.Message}", ex);
            }
        }

        public async Task<int> CreateMealMemberAsync(MealMember mealMember)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    context.MealMembers.Add(mealMember);
                    await context.SaveChangesAsync();
                    return mealMember.MealMemberId;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating recipe: {ex.Message}", ex);
            }
        }




        public async Task DeleteMealMemberDetailAsync(int detailId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var detail = await context.MealMemberDetails.FindAsync(detailId);
                    if (detail != null)
                    {
                        context.MealMemberDetails.Remove(detail);
                        await context.SaveChangesAsync();
                    }
                }
            }

            catch (Exception ex)
            {
                throw new Exception($"Error creating recipe: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<MealMember>> GetAllMealMembersAsync(int memberId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    return await context.MealMembers
                //.Include(m => m.MealMemberDetails)
                .Where(m => m.MemberId == memberId)
                .ToListAsync();
                }
            }

            catch (Exception ex)
            {
                throw new Exception($"Error creating recipe: {ex.Message}", ex);
            }
        }

        public async Task CreateMealMemberDetailsAsync(List<MealMemberDetail> mealMemberDetails)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    context.MealMemberDetails.AddRange(mealMemberDetails);
                    await context.SaveChangesAsync();
                }
            }

            catch (Exception ex)
            {
                throw new Exception($"Error creating recipe: {ex.Message}", ex);
            }
        }

        public async Task UpdateMealMemberTotalCaloriesAsync(int mealMemberId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {


                    var mealDetails = await context.MealMemberDetails
.Include(detail => detail.Food)
.Where(detail => detail.MealMemberId == mealMemberId)
.ToListAsync();





                    var mealMember = await context.MealMembers.FindAsync(mealMemberId);
                    if (mealMember != null)
                    {
                        mealMember.TotalCalories = mealDetails
                        .Sum(detail => (detail.Food.Calories) * (detail.Quantity ?? 0));

                        mealMember.TotalFat = mealDetails
                        .Sum(detail => (detail.Food.Fat) * (detail.Quantity ?? 0));

                        mealMember.TotalCarb = mealDetails
                       .Sum(detail => (detail.Food.Carbs) * (detail.Quantity ?? 0));

                        mealMember.TotalProtein = mealDetails
                       .Sum(detail => (detail.Food.Protein) * (detail.Quantity ?? 0));

                        await context.SaveChangesAsync();
                    }
                }
            }

            catch (Exception ex)
            {
                throw new Exception($"Error creating recipe: {ex.Message}", ex);
            }
        }

        public async Task<MealMemberDetailResonseDTO> GetMealMemberDetailAsync(int mealMemberId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    return await context.MealMembers
            .Where(m => m.MealMemberId == mealMemberId)
            .Include(m => m.MealMemberDetails)
            .ThenInclude(d => d.Food).ThenInclude(f => f.Diet)
                        .Select(m => new MealMemberDetailResonseDTO
                        {
                            MealPlanId = m.MealMemberId,
                            ImageMealMember = m.Image,
                            NameMealPlanMember = m.NameMealMember,
                            TotalCalories = m.TotalCalories,
                            TotalCarb = m.TotalCarb,
                            TotalProtein = m.TotalProtein,
                            TotalFat = m.TotalFat,
                            MealDate = m.MealDate,
                            FoodDetails = m.MealMemberDetails.Select(d => new GetAllFoodOfMealMemberResonseDTO
                            {
                                FoodId = d.Food.FoodId,
                                FoodImage = d.Food.FoodImage,
                                FoodName = d.Food.FoodName,
                                Calories = d.Food.Calories,
                                Protein = d.Food.Protein,
                                Fat = d.Food.Fat,
                                Carbs = d.Food.Carbs,
                                DietName = d.Food.Diet.DietName,
                                Quantity=d.Quantity
                               
                            }).ToList()

                            /* FoodImage = m.Food.FoodImage,
                             Calories = m.Food.Calories,
                             Protein=m.Food.Protein,
                             Fat=m.Food.Fat, 
                             Carbs=m.Food.Carbs,*/

                        }).FirstOrDefaultAsync();

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving meal member details: {ex.Message}", ex);
            }
        }

        public async Task DeleteMealMemberAsync(int mealMemberId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var mealMember = await context.MealMembers.FindAsync(mealMemberId);
                    if (mealMember != null)
                    {
                        context.MealMembers.Remove(mealMember);
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving meal member details: {ex.Message}", ex);
            }
        }

        public async Task DeleteMealMemberDetailsByMealMemberIdAsync(int mealMemberId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var mealMemberDetails = context.MealMemberDetails
                                 .Where(detail => detail.MealMemberId == mealMemberId);

                    context.MealMemberDetails.RemoveRange(mealMemberDetails);
                    await context.SaveChangesAsync();
        }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving meal member details: {ex.Message}", ex);
            }
        }

        public async Task<bool> AddMealMemberToDiaryDetailAsync(AddMealMemberToFoodDiaryDetailRequestDTO addMealMemberToFoodDiary,int memberId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    
                    /*var existingDiary = await context.FoodDiaries
                        .FirstOrDefaultAsync(fd => fd.MemberId == memberId && fd.Date.Date == DateTime.Now.Date );*/
                    //addMealMemberToFoodDiary.Date.Date);



                    var listMealMember = await context.MealMemberDetails
                        .Where(fd => fd.MealMemberId == addMealMemberToFoodDiary.MealMemberId && fd.MemberId== memberId).ToListAsync();

                    





                    foreach (var foodItem in listMealMember)
                    {
                        var foodDiaryDetail = new FoodDiaryDetail
                        {
                            DiaryId = addMealMemberToFoodDiary.DiaryId,
                            FoodId = foodItem.FoodId,
                            Quantity = foodItem.Quantity.HasValue ? (double)foodItem.Quantity.Value : 0.0,
                            MealType = addMealMemberToFoodDiary.MealType,
                           
                        };

                        context.FoodDiaryDetails.Add(foodDiaryDetail);
                    }

                   
                    await context.SaveChangesAsync();

                    var foodDiaryDetails = await context.FoodDiaryDetails
               .Include(fdd => fdd.Food)
               .Where(fdd => fdd.DiaryId == addMealMemberToFoodDiary.DiaryId )
               .ToListAsync();


                    double totalCalories = foodDiaryDetails.Sum(detail => detail.Food.Calories * detail.Quantity);
                    double totalProtein = foodDiaryDetails.Sum(detail => detail.Food.Protein * detail.Quantity);
                    double totalFat = foodDiaryDetails.Sum(detail => detail.Food.Fat * detail.Quantity);
                    double totalCarbs = foodDiaryDetails.Sum(detail => detail.Food.Carbs * detail.Quantity);


                    var foodDiary = await context.FoodDiaries.FirstOrDefaultAsync(fd => fd.DiaryId == addMealMemberToFoodDiary.DiaryId);
                    if (foodDiary != null)
                    {
                        foodDiary.Calories = Math.Round(totalCalories, 1);
                        foodDiary.Protein = Math.Round(totalProtein, 1);
                        foodDiary.Fat = Math.Round(totalFat, 1);
                        foodDiary.Carbs = Math.Round(totalCarbs, 1);

                        await context.SaveChangesAsync();
                    }

                    return true; 
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error adding meal to diary: {ex.Message}");
                return false; 
            }
        }
    }
}