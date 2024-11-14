using BusinessObject.Dto.Goal;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<Goal> GetGoalByIdAsync(int id)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    return await context.Goals.FirstOrDefaultAsync(g => g.GoalId == id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the goal.", ex);
            }
        }

        public async Task UpdateGoalAsync(Goal goal)
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
        }
    }
}
