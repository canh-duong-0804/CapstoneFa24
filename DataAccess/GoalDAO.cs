using BusinessObject.Dto.Goal;
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

        public async Task AddGoalAsync(Goal goal)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
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
                    var response = new GoalResponseDTO()
                    {
                        GoalId = result.GoalId,
                        WeightGoal = result.TargetValue,
                        ExerciseLevel = MapExerciseLevel(result.Member.ExerciseLevel),

                        DateInitial = result.Member.BodyMeasureChanges.FirstOrDefault()?.DateChange,
                        GoalType = result.GoalType,
                        TargetDate = result.TargetDate,
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

        private string MapExerciseLevel(int? exerciseLevel)
        {

            return exerciseLevel switch
            {
                1 => "Nhe",
                2 => "Trung binh",
                3 => "cao",
                _ => "Khong ro"
            };

        }

        public async Task<bool> updateGoal(int memberId, GoalRequestDTO updatedGoal)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    var getGoal = await context.Goals.FindAsync(memberId);
                    getGoal.TargetValue = updatedGoal.TargetValue;
                    getGoal.TargetDate = updatedGoal.TargetDate;
                    getGoal.GoalType = updatedGoal.GoalType;
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

        /*public async Task UpdateGoalAsync(Goal goal,int memberid)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    context.Goals.Update(goal);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the goal.", ex);
            }
        }*/
    }
}
