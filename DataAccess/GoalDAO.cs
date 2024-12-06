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
                    var latestBodyMeasure = await context.BodyMeasureChanges
                        .Where(b => b.MemberId == goal.MemberId)
                        .OrderByDescending(b => b.DateChange)
                        .FirstOrDefaultAsync();

                    double? currentWeight = latestBodyMeasure.Weight;
                    double targetWeight = goal.TargetValue;
                    double weeklyGoal = Convert.ToDouble(goal.GoalType);

                    int weeksNeeded = (int)Math.Ceiling(Math.Abs((double)((targetWeight - currentWeight) / weeklyGoal)));

                    goal.TargetDate = DateTime.Now.AddDays(weeksNeeded * 7);
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



        
        public async Task<GetInforGoalWeightMemberForGraphResponseDTO> GetInforGoalWeightMemberForGraph(int memberId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                  
                    var bodyMeasureChanges = await context.BodyMeasureChanges
                        .Where(b => b.MemberId == memberId)
                        .ToListAsync(); 

                    var groupedBodyMeasures = bodyMeasureChanges
                        .GroupBy(b => b.DateChange.Value.Date)
                        .Select(g => g.OrderByDescending(b => b.BodyMeasureId).FirstOrDefault())
                        .OrderBy(b => b.BodyMeasureId)
                        .Select(b => new WeightDTO
                        {
                            Date = b.DateChange.Value.ToString("dd-MM-yyyy"),
                            Weight = b.Weight
                        })
                        .ToList();

                    
                    var goals = await context.Goals
                        .Where(g => g.MemberId == memberId)
                        .ToListAsync(); 

                    var groupedGoals = goals
                        .GroupBy(g => g.ChangeDate.Date)
                        .Select(g => g.OrderBy(goal => goal.ChangeDate).FirstOrDefault()) 
                        .OrderBy(g => g.ChangeDate)
                        .Select(g => new WeightDTO
                        {
                            Date = g.ChangeDate.ToString("dd-MM-yyyy"),
                            Weight = g.TargetValue 
                        })
                        .ToList();

                 
                    return new GetInforGoalWeightMemberForGraphResponseDTO
                    {
                        currentWeight = groupedBodyMeasures,
                        goalWeight = groupedGoals
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving daily member data.", ex);
            }
        }

        

        public async Task AddOnlyGoalMember(Goal goalModel)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                   
                    var latestBodyMeasure = await context.BodyMeasureChanges
                        .Where(b => b.MemberId == goalModel.MemberId)
                        .OrderByDescending(b => b.DateChange)
                        .FirstOrDefaultAsync();

                    double? currentWeight = latestBodyMeasure.Weight;
                    double targetWeight = goalModel.TargetValue;
                    double weeklyGoal = Convert.ToDouble(goalModel.GoalType);

                    int weeksNeeded = (int)Math.Ceiling(Math.Abs((double)((targetWeight - currentWeight) / weeklyGoal)));

                    goalModel.TargetDate = DateTime.Now.AddDays(weeksNeeded * 7);
                  
                    await context.Goals.AddAsync(goalModel);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }





        /*public async Task<GetInforGoalWeightMemberForGraphResponseDTO> GetInforGoalWeightMemberForGraph(int memberId, DateTime date)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    var startOfWeek = date.AddDays(-(int)date.DayOfWeek + (int)DayOfWeek.Monday);
                    var endOfWeek = startOfWeek.AddDays(6);


                    var allRecords = await context.BodyMeasureChanges
                        .Where(b => b.MemberId == memberId && b.DateChange >= startOfWeek && b.DateChange <= endOfWeek)
                        .ToListAsync();

                    var currentWeight = allRecords
                        .GroupBy(b => b.DateChange.Value.Date)
                        .Select(g => g.OrderByDescending(b => b.DateChange).First())
                        .Select(b => new CurrentWeightMemberResponseDTO
                        {
                            DateChange = b.DateChange?.ToString("dd-MM-yyyy"),
                            Weight = b.Weight
                        })
                        .ToList();


                    var allGoals = await context.Goals
                        .Where(g => g.MemberId == memberId && g.TargetDate >= startOfWeek && g.TargetDate <= endOfWeek)
                        .ToListAsync();

                    var rawGoals = allGoals
                        .GroupBy(g => g.TargetDate.Date)
                        .Select(g => g.OrderByDescending(x => x.GoalId).First())
                        .OrderBy(g => g.TargetDate)
                        .ToList();

                    var goalWeight = new List<GoalWeightMemberResponseDTO>();
                    double? lastTargetWeight = null;


                    for (var day = startOfWeek; day <= endOfWeek; day = day.AddDays(1))
                    {

                        var applicableGoal = rawGoals
                            .Where(g => g.TargetDate.Date == day.Date)
                            .OrderByDescending(g => g.TargetDate)
                            .FirstOrDefault();


                        var nextGoal = rawGoals
                            .Where(g => g.TargetDate.Date > day.Date)
                            .OrderBy(g => g.TargetDate)
                            .FirstOrDefault();


                        if (applicableGoal != null)
                        {
                            lastTargetWeight = applicableGoal.TargetValue;
                        }
                        else if (nextGoal != null)
                        {
                            lastTargetWeight = nextGoal.TargetValue;
                        }
                        else
                        {
                            lastTargetWeight = null;
                        }


                        goalWeight.Add(new GoalWeightMemberResponseDTO
                        {
                            TargetWeight = lastTargetWeight,
                            TargetDate = day.ToString("dd-MM-yyyy")
                        });
                    }


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
        }*/



        /*public async Task<GetInforGoalWeightMemberForGraphResponseDTO> GetInforGoalWeightMemberForGraphInMonth(int memberId, DateTime date)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var startOfWeek = date.AddDays(-(int)date.DayOfWeek + (int)DayOfWeek.Monday);
                    var endOfWeek = startOfWeek.AddDays(7).AddTicks(-1);
                    var today = DateTime.UtcNow;


                    var allRecords = await context.BodyMeasureChanges
               .Where(b => b.MemberId == memberId && b.DateChange >= startOfWeek && b.DateChange <= today.AddDays(1).AddTicks(-1))
               .ToListAsync();


                    var currentWeight = allRecords
                        .GroupBy(b => b.DateChange.Value.Date)
                        .Select(g => g.OrderByDescending(b => b.DateChange).First())
                        .Select(b => new WeightDTO
                        {
                            Date = b.DateChange?.ToString("dd-MM-yyyy"),
                            Weight = b.Weight
                        })
                        .ToList();


                    var firstDayRecord = allRecords
                        .Where(b => b.DateChange.Value.Date == startOfWeek.Date)
                        .OrderByDescending(b => b.DateChange)
                        .FirstOrDefault();

                    if (firstDayRecord == null)
                    {

                        currentWeight.Add(new WeightDTO
                        {
                            Date = startOfWeek.ToString("dd-MM-yyyy"),
                            Weight = 0
                        });
                    }
                    else
                    {

                        if (!currentWeight.Any(cw => cw.Date == startOfWeek.ToString("dd-MM-yyyy")))
                        {
                            currentWeight.Add(new WeightDTO
                            {
                                Date = startOfWeek.ToString("dd-MM-yyyy"),
                                Weight = firstDayRecord.Weight
                            });
                        }
                    }
                    currentWeight = currentWeight
                        .OrderBy(cw => DateTime.ParseExact(cw.Date, "dd-MM-yyyy", null))
                        .ToList();


                    var allGoals = await context.Goals
                        .Where(g => g.MemberId == memberId && g.ChangeDate >= startOfWeek && g.ChangeDate <= today.AddDays(1).AddTicks(-1))
                        .ToListAsync();

                    var rawGoals = allGoals
                        .GroupBy(g => g.ChangeDate.Date)
                        .Select(g => g.OrderByDescending(x => x.GoalId).First())
                        .OrderBy(g => g.ChangeDate)
                        .ToList();

                    var goalWeight = new List<WeightDTO>();
                    double? lastTargetWeight = null;

                    bool isFirstIteration = true;

                    for (var day = startOfWeek; day <= endOfWeek; day = day.AddDays(1))
                    {
                        if (day <= today)
                        {
                            var applicableGoal = rawGoals
                                .Where(g => g.ChangeDate.Date == day.Date)
                                .OrderByDescending(g => g.ChangeDate)
                                .FirstOrDefault();

                            if (isFirstIteration && applicableGoal == null)
                            {
                                goalWeight.Add(new WeightDTO
                                {
                                    Weight = 0,
                                    Date = day.ToString("dd-MM-yyyy")
                                });
                            }
                            else if (applicableGoal != null)
                            {
                                lastTargetWeight = applicableGoal.TargetValue;
                                goalWeight.Add(new WeightDTO
                                {
                                    Weight = lastTargetWeight,
                                    Date = day.ToString("dd-MM-yyyy")
                                });
                            }

                            isFirstIteration = false;
                        }
                    }

                    var response = new GetInforGoalWeightMemberForGraphResponseDTO
                    {
                        goalWeight = goalWeight,
                        currentWeight = currentWeight
                    };

                    return response;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the goal.", ex);
            }
        }

*/


        /*public async Task<GetInforGoalWeightMemberForGraphResponseDTO> GetInforGoalWeightMemberForGraphInMonth(int memberId, DateTime date)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    var startOfMonth = new DateTime(date.Year, date.Month, 1);
                    var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);


                    var allRecords = await context.BodyMeasureChanges
                        .Where(b => b.MemberId == memberId && b.DateChange >= startOfMonth && b.DateChange <= endOfMonth)
                        .ToListAsync();

                    var currentWeight = allRecords
                        .GroupBy(b => b.DateChange.Value.Date)
                        .Select(g => g.OrderByDescending(b => b.DateChange).First())
                        .Select(b => new CurrentWeightMemberResponseDTO
                        {
                            DateChange = b.DateChange?.ToString("dd-MM-yyyy"),
                            Weight = b.Weight
                        })
                        .ToList();


                    var allGoals = await context.Goals
                        .Where(g => g.MemberId == memberId && g.TargetDate >= startOfMonth && g.TargetDate <= endOfMonth)
                        .ToListAsync();

                    var rawGoals = allGoals
                        .GroupBy(g => g.TargetDate.Date)
                        .Select(g => g.OrderByDescending(x => x.GoalId).First())
                        .OrderBy(g => g.TargetDate)
                        .ToList();

                    var goalWeight = new List<GoalWeightMemberResponseDTO>();
                    double? lastTargetWeight = null;


                    for (var day = startOfMonth; day <= endOfMonth; day = day.AddDays(1))
                    {

                        var applicableGoal = rawGoals
                            .Where(g => g.TargetDate.Date == day.Date)
                            .OrderBy(g => g.TargetDate)
                            .FirstOrDefault();


                        var nextGoal = rawGoals
                            .Where(g => g.TargetDate.Date > day.Date)
                            .OrderBy(g => g.TargetDate)
                            .FirstOrDefault();


                        if (applicableGoal == null && nextGoal != null)
                        {
                            lastTargetWeight = nextGoal.TargetValue;
                        }
                        else if (applicableGoal != null)
                        {
                            lastTargetWeight = applicableGoal.TargetValue;
                        }
                        else
                        {
                            lastTargetWeight = null;
                        }


                        goalWeight.Add(new GoalWeightMemberResponseDTO
                        {
                            TargetWeight = lastTargetWeight,
                            TargetDate = day.ToString("dd-MM-yyyy")
                        });
                    }


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
        }*/
    }
}
