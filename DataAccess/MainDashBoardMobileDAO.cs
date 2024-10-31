using AutoMapper.Execution;
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
    public class MainDashBoardMobileDAO
    {
        private static MainDashBoardMobileDAO instance = null;
        private static readonly object instanceLock = new object();

        public static MainDashBoardMobileDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new MainDashBoardMobileDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<MainDashBoardMobileForMemberResponseDTO> GetMainDashBoardForMemberById(int memberId)
        {
          
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var member = await context.Members
                        .Include(m => m.Goals)
                        .FirstOrDefaultAsync(m => m.MemberId == memberId);

                    if (member == null)
                        throw new Exception("Member not found");

                    // Calculate age
                    var age = DateTime.Now.Year - member.Dob.Year;


                    var latestMeasurement = await context.BodyMeasureChanges
                        .Where(b => b.MemberId == memberId)
                        .OrderByDescending(b => b.DateChange)
                        .FirstOrDefaultAsync();

                    double currentWeight = latestMeasurement.Weight ?? 0;


                    double bmr;
                    if (member.Gender == true) 
                    {
                        bmr = 88.362 + (13.397 * currentWeight) + (4.799 * (member.Height ?? 0)) - (5.677 * age);
                    }
                    else
                    {
                        bmr = 447.593 + (9.247 * currentWeight) + (3.098 * (member.Height ?? 0)) - (4.330 * age);
                    }

                    
                    double activityMultiplier = member.ExerciseLevel switch
                    {
                        1 => 1.2,
                        2 => 1.375,
                        3 => 1.725,
                        
                    };

                    var maintenanceCalories = bmr * activityMultiplier;

                   
                    var weightGoal = member.Goals.FirstOrDefault(g => g.GoalType.Contains("cân"));
                    var calorieAdjustment = 0.0;
                    DateTime? targetDate = null;
                    string goalType = "Maintain";
                    double weightDifference = 0;

                    if (weightGoal != null)
                    {
                        targetDate = weightGoal.TargetDate;
                        weightDifference = weightGoal.TargetValue - currentWeight;

                       
                        var daysUntilTarget = (weightGoal.TargetDate - DateTime.Now).Days;
                        if (daysUntilTarget < 1) daysUntilTarget = 1;

                        if (weightDifference < 0) 
                        {
                          
                            var totalDeficitNeeded = Math.Abs(weightDifference) * 7700;
                            calorieAdjustment = -1 * (totalDeficitNeeded / daysUntilTarget);
                            goalType = "Weight Loss";
                        }
                        else if (weightDifference > 0) 
                        {
                           
                            var totalSurplusNeeded = weightDifference * 7700;
                            calorieAdjustment = totalSurplusNeeded / daysUntilTarget;
                            goalType = "Weight Gain";
                        }
                    }

                    var dailyCalories = maintenanceCalories + calorieAdjustment;

                    
                    if (dailyCalories < 1200) dailyCalories = 1200;

                    
                    return new MainDashBoardMobileForMemberResponseDTO
                    {
                        DailyCalories = Math.Round(dailyCalories, 0),
                        ProteinInGrams = Math.Round((dailyCalories * 0.3) / 4, 1),  // 30% protein
                        CarbsInGrams = Math.Round((dailyCalories * 0.45) / 4, 1),   // 45% carbs
                        FatInGrams = Math.Round((dailyCalories * 0.25) / 9, 1),     // 25% fat
                        TargetDate = targetDate,
                        GoalType = goalType,
                        WeightDifference = Math.Round(weightDifference, 1)
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /*public  Task<MainDashBoardMobileForMemberResponseDTO> GetMainDashBoardForMemberById(int id)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                   
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }*/
    }

}
