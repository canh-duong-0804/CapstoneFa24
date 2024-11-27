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
                        IsCardio = request.IsCardio,
                        Status = true
                    };


                    context.Exercises.Add(exercise);
                    await context.SaveChangesAsync();

                    if (request.IsCardio)
                    {

                        if (request.CardioMetrics == null)
                        {
                            throw new Exception("Cardio metrics are required for Cardio exercises.");
                        }

                        var cardioDetail = new ExerciseCardio
                        {
                            ExerciseId = exercise.ExerciseId,
                            MetricsCardio = request.CardioMetrics.MetricsCardio,
                            MetValue = request.CardioMetrics.MetValue
                        };

                        context.ExerciseCardios.Add(cardioDetail);
                    }
                    else
                    {

                        if (request.ResistanceMetrics == null)
                        {
                            throw new Exception("Resistance metrics are required for Resistance exercises.");
                        }

                        var resistanceDetail = new ExerciseResistance
                        {
                            ExerciseId = exercise.ExerciseId,
                            MetricsResistance = request.ResistanceMetrics.MetricsResistance
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
                        ExerciseId=exerciseId,
                        ExerciseName = exercise.ExerciseName,
                        Description = exercise.Description,
                        ExerciseImage = exercise.ExerciseImage,
                        IsCardio = exercise.IsCardio ?? false,
                        CardioMetrics = exercise.IsCardio == true
                            ? exercise.ExerciseCardios.Select(c => new UpdateExerciseCardioDTO
                            {
                                MetricsCardio = c.MetricsCardio,
                                MetValue = c.MetValue
                            }).FirstOrDefault()
                            : null,
                        ResistanceMetrics = exercise.IsCardio == false
                            ? exercise.ExerciseResistances.Select(r => new UpdateExerciseResistanceDTO
                            {
                                MetricsResistance = r.MetricsResistance
                            }).FirstOrDefault()
                            : null
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
                    exercise.IsCardio = updateRequest.IsCardio ?? exercise.IsCardio;
                    exercise.ChangeDate = DateTime.UtcNow;
                    exercise.ChangeBy = memberId;

                    if (exercise.IsCardio == true && updateRequest.CardioMetrics != null)
                    {
                       
                        var cardioDetail = exercise.ExerciseCardios.FirstOrDefault();
                        if (cardioDetail != null)
                        {
                            cardioDetail.MetricsCardio = updateRequest.CardioMetrics.MetricsCardio ?? cardioDetail.MetricsCardio;
                            cardioDetail.MetValue = updateRequest.CardioMetrics.MetValue ?? cardioDetail.MetValue;
                        }
                    }
                    else if (exercise.IsCardio == false && updateRequest.ResistanceMetrics != null)
                    {
                        
                        var resistanceDetail = exercise.ExerciseResistances.FirstOrDefault();
                        if (resistanceDetail != null)
                        {
                            resistanceDetail.MetricsResistance = updateRequest.ResistanceMetrics.MetricsResistance ?? resistanceDetail.MetricsResistance;
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

    }
}


