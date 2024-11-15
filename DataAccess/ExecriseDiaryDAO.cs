using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ExerciseDiaryDAO
    {
        private static ExerciseDiaryDAO instance = null;
        private static readonly object instanceLock = new object();

        public static ExerciseDiaryDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ExerciseDiaryDAO();
                    }
                    return instance;
                }
            }
        }

        //create method to get exercise diary by member id
        public async Task<List<ExerciseDiary>> GetExerciseDiaryByMemberId(int memberId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    // Include related navigation properties
                    var exerciseDiaries = await context.ExerciseDiaries
                        .Include(ed => ed.ExerciseDiaryDetails) // Assuming Exercise is the related entity
                        .Include(ed => ed.ExercisePlan) // Assuming ExercisePlan is the related entity
                        .Include(ed => ed.Member) // Assuming Member is the related entity
                        .Where(ed => ed.MemberId == memberId) // Filter by MemberId
                        .ToListAsync();

                    return exerciseDiaries;
                }
            } catch (Exception ex)
            {
                throw ex;
            }
        }





    }
}
