﻿using AutoMapper.Execution;
using BusinessObject;
using BusinessObject.Dto.FoodDiary;
using BusinessObject.Dto.MainDashBoardMobile;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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
        public async Task<MainDashBoardMobileForMemberResponseDTO> GetMainDashBoardForMemberById(int memberId, DateTime date)
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

                    var heightInMeters = member.Height / 100.0;
                    var bmi = currentWeight / (heightInMeters * heightInMeters);
                    var weightGoal = member.Goals.FirstOrDefault(g => g.GoalType.Contains("cân"));
                    var calorieAdjustment = 0.0;
                    //DateTime? targetDate = member.Goals.FirstOrDefault(g => g.GoalType.Contains("cân")).TargetDate;
                    string goalType = "duy trì";
                    double weightDifference = 0;

                    if (weightGoal != null)
                    {
                        //targetDate = weightGoal.TargetDate;
                        weightDifference = weightGoal.TargetValue - currentWeight;


                        var timeSpan = weightGoal.TargetDate - latestMeasurement.DateChange;
                        var daysUntilTarget = (timeSpan ?? TimeSpan.Zero).Days;
                        if (daysUntilTarget < 1) daysUntilTarget = 1;

                        if (weightDifference < 0)
                        {

                            /*  var totalDeficitNeeded = Math.Abs(weightDifference) * 7700;
                              calorieAdjustment = -1 * (totalDeficitNeeded / daysUntilTarget);*/
                            goalType = "giảm cân";
                        }
                        else if (weightDifference > 0)
                        {

                            /*  var totalSurplusNeeded = weightDifference * 7700;
                              calorieAdjustment = totalSurplusNeeded / daysUntilTarget;*/
                            goalType = "tăng cân";
                        }
                    }

                    var dailyCalories = maintenanceCalories + calorieAdjustment;


                    if (dailyCalories < 1200) dailyCalories = 1200;

                    // Kiểm tra xem có FoodDiary nào đã tồn tại không
                    var foodDiary = await context.FoodDiaries
                    .Include(fd => fd.FoodDiaryDetails)
                    .FirstOrDefaultAsync(fd => fd.MemberId == memberId && fd.Date.Date == date.Date);


                    if (foodDiary == null)
                    {

                        foodDiary = new FoodDiary
                        {
                            MemberId = memberId,
                            Date = date,
                            GoalCalories = Math.Round(dailyCalories, 0),
                            Calories = 0,
                            Protein = 0,
                            Fat = 0,
                            Carbs = 0,
                            // FoodDiaryDetails = new List<FoodDiaryDetail>()
                        };


                        context.FoodDiaries.Add(foodDiary);
                        await context.SaveChangesAsync();
                    }


                    var getIdDiary = await context.FoodDiaries
                   .FirstOrDefaultAsync(fd => fd.MemberId == memberId && fd.Date.Date == date.Date);


                    return new MainDashBoardMobileForMemberResponseDTO
                    {
                        DailyCalories = Math.Round(dailyCalories, 0),
                        ProteinInGrams = Math.Round((dailyCalories * 0.3) / 4, 1),  // 30% protein
                        CarbsInGrams = Math.Round((dailyCalories * 0.45) / 4, 1),   // 45% carbs
                        FatInGrams = Math.Round((dailyCalories * 0.25) / 9, 1),     // 25% fat
                        //TargetDate = targetDate,
                        GoalType = goalType,
                        WeightDifference = Math.Round(weightDifference, 1),
                        BMI = Math.Round(bmi.Value, 0)
                    };



                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

       
    }
}