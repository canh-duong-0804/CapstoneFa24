using AutoMapper.Execution;
using BusinessObject.Dto.Goal;
using BusinessObject.Dto.MealMember;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML.Messaging;

namespace DataAccess
{
    public class GoalDAO
    {
        private static GoalDAO instance = null;
        private static readonly object instanceLock = new object();

        public static GoalDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new GoalDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task AddGoalAsync(Goal goal, double weight)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var addNewWeightCurrent = new BodyMeasureChange
                    {
                        MemberId = goal.MemberId,
                        Weight = weight,
                        DateChange = DateTime.Now,
                        BodyFat = 0,
                        Muscles = 0,
                    };

                    context.BodyMeasureChanges.Add(addNewWeightCurrent);
                    context.SaveChanges();
                    await context.Goals.AddAsync(goal);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<GoalResponseDTO> GetGoalByIdAsync(int id)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    var result = await context.Goals.Include(g => g.Member).ThenInclude(g => g.BodyMeasureChanges).FirstOrDefaultAsync(g => g.MemberId == id);
                    var getCurrentWeight = await context.BodyMeasureChanges.OrderByDescending(g => g.BodyMeasureId).FirstOrDefaultAsync(g => g.MemberId == id);
                    if (result == null)
                    {
                       
                        return new GoalResponseDTO
                        {
                            CurrentWeight = getCurrentWeight?.Weight ?? 0 
                        };
                    }



                    var response = new GoalResponseDTO()
                    {
                        GoalId = result.GoalId,
                        WeightGoal = result.TargetValue,
                        ExerciseLevel = result.Member.ExerciseLevel,

                        
                        
                        DateInitial = result.Member.BodyMeasureChanges.FirstOrDefault()?.DateChange?.ToString("dd/MM/yyyy"),

                        GoalType = float.Parse(result.GoalType),
                        TargetDate = result.TargetDate.ToString("dd/MM/yyyy"),
                        startWeight = result.Member.BodyMeasureChanges.FirstOrDefault()?.Weight,
                    };

                    response.CurrentWeight = getCurrentWeight.Weight;

                    return response;

                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the goal.", ex);
            }
        }

        

        public async Task<bool> updateGoal(int memberId, GoalRequestDTO updatedGoal)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    var getGoal = await context.Goals.FindAsync(memberId);
                    getGoal.TargetValue = updatedGoal.TargetWeight;
                    //getGoal.TargetDate = updatedGoal.TargetDate;
                   // getGoal.GoalType = updatedGoal.GoalType;
                    context.SaveChanges();
                    return true;
                    /* context.Goals.Update(updatedGoal);
                     await context.SaveChangesAsync();*/
                }
            }
            catch (Exception ex)
            {
                
                //throw new Exception("An error occurred while updating the goal.", ex);
            }
            return false;
        }

        public async Task<bool> AddCurrentWeightAsync(int memberId, double weightCurrent)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var addNewWeightCurrent = new BodyMeasureChange
                    {
                        MemberId = memberId,
                        Weight = weightCurrent,
                        DateChange = DateTime.Now,
                        BodyFat=0,
                        Muscles=0,
                    };

                        context.BodyMeasureChanges.Add(addNewWeightCurrent);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                //throw new Exception("An error occurred while updating the goal.", ex);
            }
            return false;
        }

        public async Task<bool> AddGoalLevelExercise(int memberId, string goalWeekDaily)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var getGoal = await context.Goals.OrderByDescending(b=>b.GoalId).Where(b=>b.MemberId==memberId).FirstOrDefaultAsync();


                    var addNewWeightCurrent = new Goal
                    {
                        MemberId = memberId,
                        GoalType = goalWeekDaily,
                        TargetDate =getGoal.TargetDate,
                        TargetValue=getGoal.TargetValue,

                    };

                    context.Goals.Add(addNewWeightCurrent);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                //throw new Exception("An error occurred while updating the goal.", ex);
            }
            return false;
        }

        public async Task<bool> AddGoalWeekDaily(int memberId, string goalWeekDaily)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var getGoal = await context.Goals.OrderByDescending(b => b.GoalId).Where(b => b.MemberId == memberId).FirstOrDefaultAsync();


                    var addNewWeightCurrent = new Goal
                    {
                        MemberId = memberId,
                        GoalType = goalWeekDaily,
                        TargetDate = getGoal.TargetDate,
                        TargetValue = getGoal.TargetValue,

                    };

                    context.Goals.Add(addNewWeightCurrent);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                //throw new Exception("An error occurred while updating the goal.", ex);
            }
            return false;
        }

        public async Task<bool> AddGoalWeightAsync(int memberId, double weightCurrent)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var getGoal = await context.Goals.OrderByDescending(b => b.GoalId).Where(b => b.MemberId == memberId).FirstOrDefaultAsync();


                    var addNewWeightCurrent = new Goal
                    {
                        MemberId = memberId,
                        //GoalType = goalWeekDaily,
                        TargetDate = getGoal.TargetDate,
                        TargetValue = getGoal.TargetValue,

                    };

                    context.Goals.Add(addNewWeightCurrent);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                //throw new Exception("An error occurred while updating the goal.", ex);
            }
            return false;
        }

        public async Task<GetInforGoalWeightMemberForGraphResponseDTO> GetInforGoalWeightMemberForGraph(int memberId, DateTime date)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var startOfWeek = date.AddDays(-(int)date.DayOfWeek + (int)DayOfWeek.Monday); // Thứ Hai đầu tuần
                    var endOfWeek = startOfWeek.AddDays(6); // Chủ nhật cuối tuần

                    // Lấy thông tin cân nặng trong tuần
                    var currentWeight = await context.BodyMeasureChanges
                        .Where(b => b.MemberId == memberId && b.DateChange >= startOfWeek && b.DateChange <= endOfWeek)
                        .OrderBy(b => b.DateChange)
                        .Select(b => new CurrentWeightMemberResponseDTO
                        {
                            DateChange = b.DateChange,
                            Weight = b.Weight
                        })
                        .ToListAsync();

                    // Lấy thông tin mục tiêu cân nặng trong tuần
                    var rawGoals = await context.Goals
                        .Where(g => g.MemberId == memberId && g.TargetDate >= startOfWeek && g.TargetDate <= endOfWeek)
                        .OrderBy(g => g.TargetDate)
                        .ToListAsync();

                    var goalWeight = new List<GoalWeightMemberResponseDTO>();
                    double? lastTargetWeight = null;

                    for (var day = startOfWeek; day <= endOfWeek; day = day.AddDays(1))
                    {
                        // Tìm mục tiêu gần nhất trước hoặc đúng ngày hiện tại
                        var applicableGoal = rawGoals
                            .Where(g => g.TargetDate.Date == day.Date)
                            .OrderBy(g => g.TargetDate)
                            .FirstOrDefault();

                        // Tìm mục tiêu tiếp theo sau ngày hiện tại
                        var nextGoal = rawGoals
                            .Where(g => g.TargetDate.Date > day.Date)
                            .OrderBy(g => g.TargetDate)
                            .FirstOrDefault();

                        // Sử dụng mục tiêu tiếp theo nếu không có mục tiêu trước đó
                        if (applicableGoal == null && nextGoal != null)
                        {
                            lastTargetWeight = nextGoal.TargetValue;
                        }
                        else if (applicableGoal != null && nextGoal != null)
                        {
                            lastTargetWeight = applicableGoal.TargetValue;
                        }
                        else if (nextGoal == null && applicableGoal == null)
                        {
                            lastTargetWeight = null;
                        }

                            // Thêm mục tiêu cho từng ngày
                            goalWeight.Add(new GoalWeightMemberResponseDTO
                        {
                            TargetWeight = lastTargetWeight,
                            TargetDate = day
                        });
                    }

                    // Tạo phản hồi
                    var response = new GetInforGoalWeightMemberForGraphResponseDTO
                    {
                        currentWeight = currentWeight,
                        goalWeight = goalWeight
                    };

                    return response;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the goal.", ex);
            }
        }

        public async Task<GetInforGoalWeightMemberForGraphResponseDTO> GetInforGoalWeightMemberForGraphInMonth(int memberId, DateTime date)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    // Xác định ngày đầu tháng và cuối tháng
                    var startOfMonth = new DateTime(date.Year, date.Month, 1);
                    var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                    // Lấy thông tin cân nặng trong tháng
                    var currentWeight = await context.BodyMeasureChanges
                        .Where(b => b.MemberId == memberId && b.DateChange >= startOfMonth && b.DateChange <= endOfMonth)
                        .OrderBy(b => b.DateChange)
                        .Select(b => new CurrentWeightMemberResponseDTO
                        {
                            DateChange = b.DateChange,
                            Weight = b.Weight
                        })
                        .ToListAsync();

                    // Lấy thông tin mục tiêu cân nặng trong tháng
                    var rawGoals = await context.Goals
                        .Where(g => g.MemberId == memberId && g.TargetDate >= startOfMonth && g.TargetDate <= endOfMonth)
                        .OrderBy(g => g.TargetDate)
                        .ToListAsync();

                    var goalWeight = new List<GoalWeightMemberResponseDTO>();
                    double? lastTargetWeight = null;

                    for (var day = startOfMonth; day <= endOfMonth; day = day.AddDays(1))
                    {
                        // Tìm mục tiêu gần nhất trước hoặc đúng ngày hiện tại
                        var applicableGoal = rawGoals
                            .Where(g => g.TargetDate.Date == day.Date)
                            .OrderBy(g => g.TargetDate)
                            .FirstOrDefault();

                        // Tìm mục tiêu tiếp theo sau ngày hiện tại
                        var nextGoal = rawGoals
                            .Where(g => g.TargetDate.Date > day.Date)
                            .OrderBy(g => g.TargetDate)
                            .FirstOrDefault();

                        // Sử dụng mục tiêu tiếp theo nếu không có mục tiêu trước đó
                        if (applicableGoal == null && nextGoal != null)
                        {
                            lastTargetWeight = nextGoal.TargetValue;
                        }
                        else if (applicableGoal != null && nextGoal != null)
                        {
                            lastTargetWeight = applicableGoal.TargetValue;
                        }
                        else if (nextGoal == null && applicableGoal == null)
                        {
                            lastTargetWeight = null;
                        }

                        // Thêm mục tiêu cho từng ngày
                        goalWeight.Add(new GoalWeightMemberResponseDTO
                        {
                            TargetWeight = lastTargetWeight,
                            TargetDate = day
                        });
                    }                    // Tạo phản hồi
                    var response = new GetInforGoalWeightMemberForGraphResponseDTO
                    {
                        currentWeight = currentWeight,
                        goalWeight = goalWeight
                    };

                    return response;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the goal.", ex);
            }
        }



    }
}
