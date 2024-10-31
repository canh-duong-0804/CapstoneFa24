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

       public async Task<Exercise> CreateExerciseAsync(Exercise exercise)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    context.Exercises.Add(exercise);


                    await context.SaveChangesAsync();


                    return exercise;
                }
            }
            catch (Exception ex)
            {

                throw new Exception($"Error creating exercise: {ex.Message}", ex);
            }
        }


        public async Task<IEnumerable<AllExerciseResponseDTO>> GetAllExercisesAsync()
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    var exercises = await context.Exercises
                        .Include(e => e.ExerciseCategory)
                        .Include(e => e.CreateByNavigation)
                        .Select(e => new AllExerciseResponseDTO
                        {
                            ExerciseCategoryId = e.ExerciseCategoryId,
                            ExerciseCategoryName = e.ExerciseCategory.ExerciseCategoryName,
                            CreateBy = e.CreateByNavigation.FullName,
                            ExerciseLevel = e.ExerciseLevel,
                            ExerciseImage = e.ExerciseImage,
                            ExerciseName = e.ExerciseName,
                            Description = e.Description,
                            CaloriesPerHour = e.CaloriesPerHour,

                        })
                        .ToListAsync();

                    return exercises;
                }
            }
            catch (Exception ex)
            {

                throw new Exception($"Error retrieving exercises: {ex.Message}", ex);
            }
        }

        public async Task<ExerciseDetailDTO> GetExerciseByIdAsync(int id)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var exercise = await context.Exercises
                        .Include(e => e.ExerciseCategory)
                        .Include(e => e.CreateByNavigation)
                        .Where(e => e.ExerciseId == id && e.Status == true)
                        .Select(e => new ExerciseDetailDTO
                        {
                            ExerciseCategoryId = e.ExerciseCategoryId,
                            ExerciseCategoryName = e.ExerciseCategory.ExerciseCategoryName,
                            CreateBy = e.CreateByNavigation.FullName,
                            ExerciseLevel = e.ExerciseLevel,
                            ExerciseImage = e.ExerciseImage,
                            ExerciseName = e.ExerciseName,
                            Reps = e.Reps,
                            Minutes = e.Minutes,
                            Sets = e.Sets,
                            Description = e.Description,
                            CaloriesPerHour = e.CaloriesPerHour,
                        })
                        .FirstOrDefaultAsync();

                    if (exercise == null)
                    {
                        throw new Exception("Exercise not found.");
                    }

                    return exercise;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving exercise by ID: {ex.Message}", ex);
            }
        }

        public async Task<ExerciseCategory> CreateExerciseCategoryAsync(ExerciseCategory exerciseCategory)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    await context.ExerciseCategories.AddAsync(exerciseCategory);
                    await context.SaveChangesAsync();
                    return exerciseCategory;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating exercise category: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<AllExerciseResponseDTO>> SearchAndFilterExerciseByIdAsync(SearchFilterObjectDTO search)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                   
                    var query = context.Exercises
                        .Include(e => e.ExerciseCategory)
                        .Where(e => e.Status == true); 

               
                    if (!string.IsNullOrEmpty(search.SearchName) && !string.IsNullOrEmpty(search.FilterName))
                    {
                        query = query.Where(e => e.ExerciseName.Contains(search.SearchName) &&
                                                 e.ExerciseCategory.ExerciseCategoryName == search.FilterName);
                    }
                 
                    else if (!string.IsNullOrEmpty(search.SearchName))
                    {
                        query = query.Where(e => e.ExerciseName.ToLower().Contains(search.SearchName.ToLower()));
                    }
                   
                    else if (!string.IsNullOrEmpty(search.FilterName))
                    {
                        query = query.Where(e => e.ExerciseCategory.ExerciseCategoryName.ToLower() == search.FilterName.ToLower());
                    }

                    
                   

                    var exercises = await query
                        .Select(e => new AllExerciseResponseDTO
                        {
                            ExerciseCategoryId = e.ExerciseCategoryId,
                            ExerciseCategoryName = e.ExerciseCategory.ExerciseCategoryName,
                            CreateBy = e.CreateByNavigation.FullName,
                            ExerciseLevel = e.ExerciseLevel,
                            ExerciseImage = e.ExerciseImage,
                            ExerciseName = e.ExerciseName,
                            Description = e.Description,
                            CaloriesPerHour = e.CaloriesPerHour,

                        })
                        .ToListAsync();


                    return exercises;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error searching exercises: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteExerciseAsync(int id)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var exercise = await context.Exercises.FindAsync(id);
                    if (exercise == null)
                    {
                        return false; 
                    }


                    exercise.Status = false; 
                    context.Exercises.Update(exercise);
                    await context.SaveChangesAsync(); 
                    return true; 
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating status of exercise: {ex.Message}", ex);
            }
        }

        public async Task<UpdateExerciseRequestDTO> UpdateExerciseAsync(UpdateExerciseRequestDTO exerciseDto)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                   
                    var existingExercise = await context.Exercises.FindAsync(exerciseDto.ExerciseId);

                   
                    if (existingExercise == null)
                    {
                        return null;
                    }

                   
                    existingExercise.ExerciseName = exerciseDto.ExerciseName;
                    existingExercise.ExerciseCategoryId = exerciseDto.ExerciseCategoryId;
                    existingExercise.Description = exerciseDto.Description;
                    existingExercise.Minutes = exerciseDto.Minutes;
                    existingExercise.Reps = exerciseDto.Reps;
                    existingExercise.Sets = exerciseDto.Sets;
                    existingExercise.ChangeBy = exerciseDto.ChangeBy;
                    existingExercise.ChangeDate = exerciseDto.ChangeDate;
                    existingExercise.CaloriesPerHour = exerciseDto.CaloriesPerHour;
                    existingExercise.ExerciseLevel = exerciseDto.ExerciseLevel; 
                    existingExercise.ExerciseImage = exerciseDto.ExerciseImage; 
                   

                    await context.SaveChangesAsync();

                    return exerciseDto;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating exercise: {ex.Message}", ex);
            }
        }

       
    }
}
