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

        public async Task<GetExerciseDetailOfCardiorResponseDTO> GetExercisesCardioDetailForMemberrAsync(int exerciseId)
        {

            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var exercise = await context.Exercises
                    .Where(e => e.ExerciseId == exerciseId)
                    .FirstOrDefaultAsync();

                    if (exercise == null || exercise.IsCardio == false) return null;



                    var result = new GetExerciseDetailOfCardiorResponseDTO
                    {
                        ExerciseId = exercise.ExerciseId,
                        IsCardio = exercise.IsCardio,
                        CategoryExercise = exercise.IsCardio == true ? "Cardio" : "Kháng lực",
                        ExerciseImage = exercise.ExerciseImage,
                        ExerciseName = exercise.ExerciseName,
                        Description = exercise.Description
                    };


                   
                        var cardio = await context.ExerciseCardios
                            .Where(c => c.ExerciseId == exerciseId)
                            .FirstOrDefaultAsync();

                        if (cardio != null)
                        {
                            result.Minutes1 = cardio.Minutes1 ?? 0;
                            result.Minutes2 = cardio.Minutes2 ?? 0;
                            result.Minutes3 = cardio.Minutes3 ?? 0;
                            result.Calories1 = cardio.Calories1 ?? 0;
                            result.Calories2 = cardio.Calories2 ?? 0;
                            result.Calories3 = cardio.Calories3 ?? 0;
                            result.MetValue = cardio.MetValue ?? 0;
                        }
                    
                    return result;
                }
            }

            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while fetching exercise details.", ex);
            }


        }

        public async Task<GetExerciseDetailOfResitanceResponseDTO> GetExercisesResistanceDetailForMemberAsync(int exerciseId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var exercise = await context.Exercises
                .Where(e => e.ExerciseId == exerciseId)
                .FirstOrDefaultAsync();

                    if (exercise == null || exercise.IsCardio == true) return null;


                    var result = new GetExerciseDetailOfResitanceResponseDTO
                    {
                        ExerciseId = exercise.ExerciseId,
                        IsCardio = exercise.IsCardio,
                        CategoryExercise = exercise.IsCardio == false ? "Kháng lực" : "Cardio",
                        ExerciseImage = exercise.ExerciseImage,
                        ExerciseName = exercise.ExerciseName,
                        Description = exercise.Description
                    };


                   
                        var resistance = await context.ExerciseResistances
                            .Where(r => r.ExerciseId == exerciseId)
                            .FirstOrDefaultAsync();

                        if (resistance != null)
                        {
                            result.Reps1 = resistance.Reps1 ?? 0;
                            result.Reps2 = resistance.Reps2 ?? 0;
                            result.Reps3 = resistance.Reps3 ?? 0;
                            result.Sets1 = resistance.Sets1 ?? 0;
                            result.Sets2 = resistance.Sets2 ?? 0;
                            result.Sets3 = resistance.Sets3 ?? 0;
                            result.Minutes1 = resistance.Minutes1 ?? 0;
                            result.Minutes2 = resistance.Minutes2 ?? 0;
                            result.Minutes3 = resistance.Minutes3 ?? 0;
                        }
                    

                    return result;
                }
            }

            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while fetching exercise details.", ex);
            }
        }
    }
}
