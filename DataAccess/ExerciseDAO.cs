using AutoMapper.Execution;
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

        public async Task<List<GetAllExerciseForMember>> GetAllExercisesForMemberAsync(string? search, int? isCardioFilter)
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
                        query = query.Where(e => e.TypeExercise == isCardioFilter.Value);
                    }

                    var exercises = await query
                        .Select(e => new GetAllExerciseForMember
                        {
                            ExerciseId = e.ExerciseId,

                            ExerciseImage = e.ExerciseImage,
                            ExerciseName = e.ExerciseName,
                            CategoryExercise = e.TypeExercise == 1 ? "Cardio" :
                                               e.TypeExercise == 2 ? "Kháng lực" :
                                               e.TypeExercise == 3 ? "Các bài tập khác": "khong xac dinh"

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

        public async Task<GetExerciseDetailOfCardiorResponseDTO> GetExercisesCardioDetailForMemberrAsync(int exerciseId,int memberId)
        {

            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var exercise = await context.Exercises
                    .Where(e => e.ExerciseId == exerciseId)
                    .FirstOrDefaultAsync();
                    var cardio = await context.ExerciseCardios
                           .Where(c => c.ExerciseId == exerciseId)
                           .FirstOrDefaultAsync();
                    if (exercise == null ) return null;
                    var latestMeasurement = await context.BodyMeasureChanges
                        .Where(b => b.MemberId == memberId)
                        .OrderByDescending(b => b.DateChange.Value.Date)
                        .ThenBy(b => b.DateChange.Value.TimeOfDay)
                        .FirstOrDefaultAsync();

                    if (exercise.TypeExercise == 1)
                    {
                        var result = new GetExerciseDetailOfCardiorResponseDTO
                        {
                            ExerciseId = exercise.ExerciseId,
                            TypeExercise = exercise.TypeExercise,
                            CategoryExercise = exercise.TypeExercise switch
                            {
                                1 => "Cardio",
                                2 => "Kháng lực",
                                3 => "Các bài tập khác",
                                _ => "Không xác định"
                            },
                            ExerciseImage = exercise.ExerciseImage,
                            ExerciseName = exercise.ExerciseName,
                            Description = exercise.Description,
                            MetValue = exercise.MetValue,
                            Calories1 = cardio.Calories1,
                            Calories2 = cardio.Calories2,
                            Calories3 = cardio.Calories3,
                            Minutes1 = cardio.Minutes1,
                            Minutes2 = cardio.Minutes2,
                            Minutes3 = cardio.Minutes3,
                            Weight=latestMeasurement.Weight
                            

                            //MetricsCardio=cardio.MetricsCardio
                        };







                        return result;
                    }
                    return null;
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
                    var resistance = await context.ExerciseResistances
                                                .Where(r => r.ExerciseId == exerciseId)
                                                .FirstOrDefaultAsync();
                    if (exercise == null ) return null;

                    if (exercise.TypeExercise == 2)
                    {
                        var result = new GetExerciseDetailOfResitanceResponseDTO
                        {
                            ExerciseId = exercise.ExerciseId,
                            TypeExercise = exercise.TypeExercise,
                            CategoryExercise = exercise.TypeExercise switch
                            {
                                1 => "Cardio",
                                2 => "Kháng lực",
                                3 => "Các bài tập khác",
                                _ => "Không xác định"
                            },
                            ExerciseImage = exercise.ExerciseImage,
                            ExerciseName = exercise.ExerciseName,
                            Description = exercise.Description,
                           
                            Minutes1 = resistance.Minutes1,
                            Minutes2 = resistance.Minutes2,
                            Minutes3 = resistance.Minutes3,
                            Reps1 = resistance.Reps1,
                            Reps2 = resistance.Reps2,
                            Reps3 = resistance.Reps3,
                            Sets1 = resistance.Sets1,
                            Sets2 = resistance.Sets2,
                            Sets3 = resistance.Sets3,
                            MetValue=exercise.MetValue,

                            //MetricsResistance= resistance.MetricsResistance
                        };
                        return result;
                    }
                    return null;
                }
            }

            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while fetching exercise details.", ex);
            }
        }

        public async Task<GetExerciseDetailOfOtherResponseDTO> GetExercisesOtherDetailForMemberAsync(int exerciseId, int memberId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var exercise = await context.Exercises
                .Where(e => e.ExerciseId == exerciseId)
                .FirstOrDefaultAsync();
                    var resistance = await context.ExerciseResistances
                                                .Where(r => r.ExerciseId == exerciseId)
                                                .FirstOrDefaultAsync();
                    if (exercise == null ) return null;
                    var latestMeasurement = await context.BodyMeasureChanges
                        .Where(b => b.MemberId == memberId)
                        .OrderByDescending(b => b.DateChange.Value.Date)
                        .ThenBy(b => b.DateChange.Value.TimeOfDay)
                        .FirstOrDefaultAsync();
                    if (exercise.TypeExercise == 3)
                    {
                        var result = new GetExerciseDetailOfOtherResponseDTO
                        {
                            ExerciseId = exercise.ExerciseId,
                            TypeExercise = exercise.TypeExercise,
                            CategoryExercise = exercise.TypeExercise switch
                            {
                                1 => "Cardio",
                                2 => "Kháng lực",
                                3 => "Các bài tập khác",
                                _ => "Không xác định"
                            },
                            ExerciseImage = exercise.ExerciseImage,
                            ExerciseName = exercise.ExerciseName,
                            Description = exercise.Description,
                            MetValue=exercise.MetValue,
                            Weight=latestMeasurement.Weight


                            //MetricsResistance= resistance.MetricsResistance
                        };
                        return result;
                    }
                    return null;
                }
            }

            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while fetching exercise details.", ex);
            }
        }
    }
}
