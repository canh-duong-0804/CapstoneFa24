using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ExercisePlanTrainerDAO
    {
        private static ExercisePlanTrainerDAO instance = null;
        private static readonly object instanceLock = new object();

        public static ExercisePlanTrainerDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ExercisePlanTrainerDAO();
                    }
                    return instance;
                }
            }
        }

        // Add Exercise Plan
        public async Task<bool> AddExercisePlanAsync(ExercisePlan exercisePlan)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    await context.ExercisePlans.AddAsync(exercisePlan);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Get Exercise Plan by ID (Active Only)
        public async Task<ExercisePlan?> GetExercisePlanByIdAsync(int id)
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
            catch (Exception)
            {
                return null;
            }
        }

        // Get All Active Exercise Plans
        public async Task<List<ExercisePlan>> GetExercisePlansAsync()
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
            catch (Exception)
            {
                return new List<ExercisePlan>();
            }
        }

        // Update Exercise Plan
        public async Task<bool> UpdateExercisePlanAsync(ExercisePlan exercisePlan)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    context.ExercisePlans.Update(exercisePlan);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Soft Delete Exercise Plan
        public async Task<bool> SoftDeleteExercisePlanAsync(int planId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var exercisePlan = await context.ExercisePlans
                        .Include(p => p.ExercisePlanDetails)
                        .FirstOrDefaultAsync(p => p.ExercisePlanId == planId);

                    if (exercisePlan == null)
                        return false;

                    // Mark Exercise Plan and its details as inactive
                    exercisePlan.Status = false;
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Get Exercise Plan Details by Plan ID
        public async Task<List<ExercisePlanDetail>> GetExercisePlanDetailsByPlanIdAsync(int planId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    return await context.ExercisePlanDetails
                        .Where(d => d.ExercisePlanId == planId)
                        .ToListAsync();
                }
            }
            catch (Exception)
            {
                return new List<ExercisePlanDetail>();
            }
        }

        // Add Exercise Plan Detail
        public async Task<bool> AddExercisePlanDetailAsync(ExercisePlanDetail detail)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    await context.ExercisePlanDetails.AddAsync(detail);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Update Exercise Plan Detail
        public async Task<bool> UpdateExercisePlanDetailAsync(ExercisePlanDetail detail)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    context.ExercisePlanDetails.Update(detail);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Delete Exercise Plan Detail
        public async Task<bool> DeleteExercisePlanDetailAsync(int detailId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var detail = await context.ExercisePlanDetails.FindAsync(detailId);
                    if (detail == null)
                        return false;

                    context.ExercisePlanDetails.Remove(detail);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Get Exercise Plan Detail by ID
        public async Task<ExercisePlanDetail?> GetExercisePlanDetailByIdAsync(int detailId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    return await context.ExercisePlanDetails
                        .FirstOrDefaultAsync(d => d.ExercisePlanDetailId == detailId);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
