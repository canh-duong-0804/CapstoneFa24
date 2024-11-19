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

        public async Task<List<GetAllExerciseForMember>> GetAllExercisesForMemberAsync(string? search, bool? isCardioFilter)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var query = context.Exercises.AsQueryable();


                    if (!string.IsNullOrEmpty(search))
                    {
                        query = query.Where((e => EF.Functions.Collate(e.ExerciseName.ToLower(), "Vietnamese_CI_AI").Contains(search.ToLower())));
                    }


                    if (isCardioFilter.HasValue)
                    {
                        query = query.Where(e => e.IsCardio == isCardioFilter.Value);
                    }

                    var exercises = await query
                        .Select(e => new GetAllExerciseForMember
                        {
                            ExerciseId = e.ExerciseId,

                            ExerciseImage = e.ExerciseImage,
                            ExerciseName = e.ExerciseName,
                            CategoryExercise = e.IsCardio == true ? "Cardio" : "Kháng lực"
                        })
                        .ToListAsync();

                    return exercises;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }
    }
}
