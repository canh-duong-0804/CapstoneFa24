using AutoMapper.Execution;
using BusinessObject.Dto.FoodDiary;
using BusinessObject.Dto.FoodDiaryDetails;
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
                }
                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error while adding food items to diary: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteFoodListToDiaryAsync(int id)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    // Tìm kiếm mục thực phẩm theo ID
                    var foodItem = await context.FoodItems.FindAsync(id);
                    if (foodItem == null)
                    {
                        // Nếu không tìm thấy mục thực phẩm, trả về false
                        return false;
                    }

                    // Xóa mục thực phẩm
                    context.FoodItems.Remove(foodItem);
                    await context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu

                    return true; // Xóa thành công
                }
            }
            catch (Exception ex)
            {
                // Ghi log hoặc xử lý lỗi ở đây nếu cần
                Console.WriteLine($"Error deleting food item: {ex.Message}");
                return false; // Trả về false trong trường hợp xảy ra lỗi
            }
        }


        public async Task getDailyFoodDiaryFollowMeal(int dairyID, int mealType)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {


                    var foodDiary = await context.FoodDiaryDetails
                     .Include(fd=>fd.Food)
                    .FirstOrDefaultAsync(fd => fd.DiaryId == dairyID && fd.MealType == mealType);

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);

            }
        }
    }
}