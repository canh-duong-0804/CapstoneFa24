using AutoMapper.Execution;
using BusinessObject.Dto.ExecrisePlan;
using BusinessObject.Dto.ExerciseTrainer;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ExerciseTrainerDAO
    {
        private static ExerciseTrainerDAO instance = null;
        private static readonly object instanceLock = new object();

        public static ExerciseTrainerDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ExerciseTrainerDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<int> CreateExerciseTrainerAsync(CreateExerciseRequestDTO request, int memberId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var exercise = new Exercise
                    {
                        ExerciseName = request.ExerciseName,
                        Description = request.Description,
                        CreateBy = memberId,
                        CreateDate = DateTime.UtcNow,
                        ExerciseImage = request.ExerciseImage,
                        TypeExercise = request.TypeExercise,
                        MetValue = request.MetValue,
                        Status = true
                    };


                    context.Exercises.Add(exercise);
                    await context.SaveChangesAsync();

                    if (request.TypeExercise == 1)
                    {

                        if (request.CardioMetrics == null)
                        {
                            throw new Exception("Cardio metrics are required for Cardio exercises.");
                        }

                        var cardioDetail = new ExerciseCardio
                        {
                            ExerciseId = exercise.ExerciseId,
                            Calories1 = request.CardioMetrics.Calories1,
                            Calories2 = request.CardioMetrics.Calories2,
                            Calories3 = request.CardioMetrics.Calories3,
                            Minutes1 = request.CardioMetrics.Minutes1,
                            Minutes2 = request.CardioMetrics.Minutes2,
                            Minutes3 = request.CardioMetrics.Minutes3,

                            // MetricsCardio = request.CardioMetrics.MetricsCardio,
                            // MetValue = request.CardioMetrics.MetValue
                        };

                        context.ExerciseCardios.Add(cardioDetail);
                    }
                    else if (request.TypeExercise == 2)
                    {

                        if (request.ResistanceMetrics == null)
                        {
                            throw new Exception("Resistance metrics are required for Resistance exercises.");
                        }

                        var resistanceDetail = new ExerciseResistance
                        {
                            ExerciseId = exercise.ExerciseId,
                            Reps1 = request.ResistanceMetrics.Reps1,
                            Reps2 = request.ResistanceMetrics.Reps2,
                            Reps3 = request.ResistanceMetrics.Reps3,
                            Sets1 = request.ResistanceMetrics.Sets1,
                            Sets2 = request.ResistanceMetrics.Sets2,
                            Sets3 = request.ResistanceMetrics.Sets3,
                            Minutes3 = request.ResistanceMetrics.Minutes3,
                            Minutes2 = request.ResistanceMetrics.Minutes2,
                            Minutes1 = request.ResistanceMetrics.Minutes1,
                            //MetricsResistance = request.ResistanceMetrics.MetricsResistance
                        };

                        context.ExerciseResistances.Add(resistanceDetail);
                    }


                    await context.SaveChangesAsync();

                    int idExercise = context.Exercises.OrderByDescending(d => d.ExerciseId).FirstOrDefault().ExerciseId;
                    return idExercise;
                }
            }
            catch (Exception ex)
            {
                return 0;
                throw new Exception($"An error occurred while creating the exercise: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteExerciseAsync(int exerciseId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var exercise = await context.Exercises.FirstOrDefaultAsync(e => e.ExerciseId == exerciseId);

                    if (exercise == null)
                        return false;

                    exercise.Status = false;

                    await context.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception($"An error occurred while creating the exercise: {ex.Message}", ex);
            }
            return false;
        }

        public async Task<ExerciseRequestDTO> GetExerciseDetailAsync(int exerciseId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    var exercise = await context.Exercises
                        .Where(e => e.ExerciseId == exerciseId && e.Status == true)
                        .Include(e => e.ExerciseCardios)
                        .Include(e => e.ExerciseResistances)
                        .FirstOrDefaultAsync();

                    if (exercise == null)
                        throw new Exception("Exercise not found.");


                    var exerciseDto = new ExerciseRequestDTO
                    {
                        ExerciseId = exerciseId,
                        ExerciseName = exercise.ExerciseName,
                        Description = exercise.Description,
                        ExerciseImage = exercise.ExerciseImage,
                        MetValue = exercise.MetValue,


                        TypeExercise = exercise.TypeExercise,
                        CardioMetrics = exercise.TypeExercise == 1
                            ? exercise.ExerciseCardios.Select(c => new UpdateExerciseCardioDTO
                            {
                                //MetricsCardio = c.MetricsCardio,
                                //MetValue = c.MetValue
                                Calories1 = c.Calories1,
                                Calories2 = c.Calories2,
                                Calories3 = c.Calories3,
                                Minutes1 = c.Minutes1,
                                Minutes2 = c.Minutes2,
                                Minutes3 = c.Minutes3,



                            }).FirstOrDefault()
                            : null,
                        ResistanceMetrics = exercise.TypeExercise == 2
                            ? exercise.ExerciseResistances.Select(r => new UpdateExerciseResistanceDTO
                            {

                                Reps1 = r.Reps1,
                                Reps2 = r.Reps2,
                                Reps3 = r.Reps3,
                                Sets1 = r.Sets1,
                                Sets2 = r.Sets2,
                                Sets3 = r.Sets3,
                                Minutes1 = r.Minutes1,
                                Minutes2 = r.Minutes2,
                                Minutes3 = r.Minutes3,

                                //    MetricsResistance = r.MetricsResistance
                            }).FirstOrDefault()
                            : null,

                    };

                    return exerciseDto;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while getting the exercise: {ex.Message}", ex);
            }
        }


        public async Task<ExerciseRequestDTO> UpdateExerciseAsync(int memberId, ExerciseRequestDTO updateRequest)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var exercise = await context.Exercises
                        .Include(e => e.ExerciseCardios)
                        .Include(e => e.ExerciseResistances)
                        .FirstOrDefaultAsync(e => e.ExerciseId == updateRequest.ExerciseId);

                    if (exercise == null || exercise.Status == false)
                        throw new Exception("Exercise not found or inactive.");


                    exercise.ExerciseName = updateRequest.ExerciseName ?? exercise.ExerciseName;
                    exercise.Description = updateRequest.Description ?? exercise.Description;
                    exercise.ExerciseImage = updateRequest.ExerciseImage ?? exercise.ExerciseImage;
                    //exercise.TypeExercise = updateRequest.IsCardio ?? exercise.IsCardio;
                    exercise.ChangeDate = DateTime.UtcNow;
                    exercise.ChangeBy = memberId;
                    exercise.MetValue = updateRequest.MetValue;

                    if (exercise.TypeExercise == 1 && updateRequest.CardioMetrics != null)
                    {

                        var cardioDetail = exercise.ExerciseCardios.FirstOrDefault();
                        if (cardioDetail != null)
                        {
                            cardioDetail.Minutes1 = updateRequest.CardioMetrics.Minutes1;
                            cardioDetail.Minutes2 = updateRequest.CardioMetrics.Minutes2;
                            cardioDetail.Minutes3 = updateRequest.CardioMetrics.Minutes3;
                            cardioDetail.Calories1 = updateRequest.CardioMetrics.Calories1;
                            cardioDetail.Calories2 = updateRequest.CardioMetrics.Calories2;
                            cardioDetail.Calories3 = updateRequest.CardioMetrics.Calories3;

                            //  cardioDetail.MetricsCardio = updateRequest.CardioMetrics.MetricsCardio ?? cardioDetail.MetricsCardio;
                            //   cardioDetail.MetValue = updateRequest.CardioMetrics.MetValue ?? cardioDetail.MetValue;
                        }
                    }
                    else if (exercise.TypeExercise == 2 && updateRequest.ResistanceMetrics != null)
                    {

                        var resistanceDetail = exercise.ExerciseResistances.FirstOrDefault();
                        if (resistanceDetail != null)
                        {
                            resistanceDetail.Reps1 = updateRequest.ResistanceMetrics.Reps1;
                            resistanceDetail.Reps2 = updateRequest.ResistanceMetrics.Reps2;
                            resistanceDetail.Reps3 = updateRequest.ResistanceMetrics.Reps3;
                            resistanceDetail.Sets1 = updateRequest.ResistanceMetrics.Sets1;
                            resistanceDetail.Sets2 = updateRequest.ResistanceMetrics.Sets2;
                            resistanceDetail.Sets3 = updateRequest.ResistanceMetrics.Sets3;
                            resistanceDetail.Minutes1 = updateRequest.ResistanceMetrics.Minutes1;
                            resistanceDetail.Minutes2 = updateRequest.ResistanceMetrics.Minutes2;
                            resistanceDetail.Minutes3 = updateRequest.ResistanceMetrics.Minutes3;

                            //  resistanceDetail.MetricsResistance = updateRequest.ResistanceMetrics.MetricsResistance ?? resistanceDetail.MetricsResistance;
                        }
                    }


                    await context.SaveChangesAsync();

                    return updateRequest;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the exercise: {ex.Message}", ex);
            }
        }

        public async Task<bool> UploadImageForMealMember(string urlImage, int exerciseId)
        {
            try
            {
                using var context = new HealthTrackingDBContext();


                var exercise = await context.Exercises
                    .FirstOrDefaultAsync(m => m.ExerciseId == exerciseId);

                if (exercise == null)
                {
                    throw new Exception("MealMember not found.");
                }


                exercise.ExerciseImage = urlImage;


                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error uploading image for meal member: {ex.Message}", ex);
            }
        }

        public async Task<GetExercisePlanResponseForTrainerDTO> GetAllExercisePlansAsync(int page, int pageSize)
        {
            try
            {
                using var context = new HealthTrackingDBContext();

                // Tính tổng số bản ghi
                var totalRecords = await context.ExercisePlans.CountAsync();

                // Lấy danh sách bài tập theo trang
                var exercisePlans = await context.ExercisePlans
                   
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(ep => new ExercisePlanDTO
                    {
                        ExercisePlanId = ep.ExercisePlanId,
                        Name = ep.Name,
                      TotalCaloriesBurned = ep.TotalCaloriesBurned,
                        ExercisePlanImage=ep.ExercisePlanImage,
                    })
                    .ToListAsync();

                // Tạo phản hồi
                return new GetExercisePlanResponseForTrainerDTO
                {
                    Data = exercisePlans, // Đây là List<ExercisePlanDTO>
                    TotalRecords = totalRecords,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize)
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching exercise plans: {ex.Message}", ex);
            }
        }

    }
}


