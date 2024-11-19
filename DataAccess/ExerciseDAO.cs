using BusinessObject.Dto.CategoryExerice;
using BusinessObject.Dto.Exericse;
using BusinessObject.Dto.SearchFilter;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ExerciseDAO
    {
        private static ExerciseDAO instance = null;
        private static readonly object instanceLock = new object();

        public static ExerciseDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ExerciseDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<List<GetAllExerciseForMember>> GetAllExercisesForMemberAsync()
        {
            using (var context = new HealthTrackingDBContext())
            {
                try
                {
                    var exercises = await context.Exercises
                    .Select(e => new GetAllExerciseForMember
                    {
                        ExerciseId = e.ExerciseId,
                        CategoryExercise = e.IsCardio == true ? "Cardio" : "Kháng lực",
                        ExerciseImage = e.ExerciseImage,
                        ExerciseName = e.ExerciseName
                    })
                    .ToListAsync();

                    return exercises;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error creating food: {ex.Message}", ex);
                }
            }
        }

    }
}
