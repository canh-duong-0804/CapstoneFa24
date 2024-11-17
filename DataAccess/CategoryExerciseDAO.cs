using BusinessObject.Dto.CategoryExerice;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CategoryExerciseDAO
    {
        private static CategoryExerciseDAO instance = null;
        private static readonly object instanceLock = new object();

        public static CategoryExerciseDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CategoryExerciseDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<ExerciseCategory> CreateExerciseCategoryAsync(ExerciseCategory cate)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    //context.ExerciseCategories.Add(cate);


                    await context.SaveChangesAsync();


                    return cate;
                }
            }
            catch (Exception ex)
            {

                throw new Exception($"Error creating exercise: {ex.Message}", ex);
            }
        }

        public async Task<List<GetAllCategoryExeriseResponseDTO>> GetAllCategoryExercisesAsync()
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    /*var cateExercises = await context.ExerciseCategories
                        .Where(e => e.Status == true)
                        .Select(e => new GetAllCategoryExeriseResponseDTO
                        {
                            ExerciseCategoryId = e.ExerciseCategoryId,
                            ExerciseCategoryName = e.ExerciseCategoryName,
                            Status = e.Status,      
                            //Value = e.ExerciseCategoryId,
                        })
                        .ToListAsync();

                    return cateExercises;*/
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw new Exception($"Error retrieving exercises: {ex.Message}", ex);
            }
        }

        public async Task DeleteExerciseCategoryAsync(int id)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    /* var category = await context.ExerciseCategories.FindAsync(id);
                     if (category != null)
                     {
                         category.Status = false;
                         await context.SaveChangesAsync();
                     }
                     else
                     {
                         throw new Exception("Exercise category not found.");
                     }*/

                    
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting exercise category: {ex.Message}", ex);
            }
        }

        public async Task UpdateCategoryExercisesAsync(UpdateCategoryExerciseRequestDTO cate)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                   /* var category = await context.ExerciseCategories.FindAsync(cate.ExerciseCategoryId);
                    if (category != null)
                    {
                        category.ExerciseCategoryName = cate.ExerciseCategoryName;
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        throw new Exception("Exercise category not found.");
                    }*/
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting exercise category: {ex.Message}", ex);
            }
        }

        public async Task<List<ListBoxResponseDTO>> GetlistboxAsync()
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    /*var cateExercises = await context.ExerciseCategories
                        .Where(e => e.Status == true)
                        .Select(e => new ListBoxResponseDTO
                        {
                            Key = e.ExerciseCategoryId.ToString(),
                            Label = e.ExerciseCategoryName,
                            Value = e.ExerciseCategoryId
                        })
                        .ToListAsync();

                    return cateExercises;*/
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw new Exception($"Error retrieving exercises: {ex.Message}", ex);
            }
        }
	}
}
