using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ExecrisePlanDetailDAO
    {
        private static ExecrisePlanDetailDAO instance = null;
        private static readonly object instanceLock = new object();

        public static ExecrisePlanDetailDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ExecrisePlanDetailDAO();
                    }
                    return instance;
                }
            }
        }

        // Add Exercise Plan Details to an Exercise Plan
        public async Task AddExercisePlanDetailsByPlanIdAsync(int planId, List<ExercisePlanDetail> details)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    // Check if the Exercise Plan exists
                    var exercisePlan = await context.ExercisePlans.FindAsync(planId);
                    if (exercisePlan == null)
                    {
                        throw new Exception($"Exercise Plan with ID {planId} does not exist.");
                    }

                    // Ensure the provided details are associated with the specified plan
                    foreach (var detail in details)
                    {
                        detail.ExercisePlanId = planId;
                    }

                    // Add the new details to the database
                    await context.ExercisePlanDetails.AddRangeAsync(details);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding Exercise Plan Details: {ex.Message}");
            }
        }

        // Get Exercise Plan Details by Plan ID
       
        // Delete Exercise Plan Details by Plan ID
       
    }
}
