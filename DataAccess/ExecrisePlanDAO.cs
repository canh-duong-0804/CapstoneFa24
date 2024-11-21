using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ExecrisePlanDAO
    {
        private static ExecrisePlanDAO instance = null;
        private static readonly object instanceLock = new object();

        public static ExecrisePlanDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ExecrisePlanDAO();
                    }
                    return instance;
                }
            }
        }

        // Add Exercise Plan
        public async Task AddExecrisePlanAsync(ExercisePlan exercisePlan)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    await context.ExercisePlans.AddAsync(exercisePlan);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding Exercise Plan: {ex.Message}");
            }
        }

        // Get Exercise Plan by ID (Active Only)
        public async Task<ExercisePlan> GetExecrisePlanByIdAsync(int id)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    return await context.ExercisePlans
                        .Include(p => p.ExercisePlanDetails)
                        .ThenInclude(d => d.Exercise)
                        .FirstOrDefaultAsync(p => p.ExercisePlanId == id && p.Status == true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching Exercise Plan: {ex.Message}");
            }
        }

        // Get All Active Exercise Plans
        public async Task<List<ExercisePlan>> GetExecrisePlansAsync()
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    return await context.ExercisePlans
                        .Include(p => p.ExercisePlanDetails)
                        .ThenInclude(d => d.Exercise)
                        .Where(p => p.Status == true)
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching Exercise Plans: {ex.Message}");
            }
        }

        // Update Exercise Plan
        public async Task UpdateExecrisePlanAsync(ExercisePlan exercisePlan)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    context.ExercisePlans.Update(exercisePlan);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating Exercise Plan: {ex.Message}");
            }
        }

        // Soft Delete Exercise Plan
        public async Task SoftDeleteExecrisePlanAsync(int planid)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var exercisePlan = await context.ExercisePlans
                        .Include(p => p.ExercisePlanDetails)
                        .FirstOrDefaultAsync(p => p.ExercisePlanId == planid);

                    if (exercisePlan == null)
                    {
                        throw new Exception("Exercise Plan not found.");
                    }

                    // Mark Exercise Plan and its details as inactive
                    exercisePlan.Status = false;

                   

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting Exercise Plan: {ex.Message}");
            }
        }
    }
}
