using BusinessObject.Dto.CategoryExerice;
using BusinessObject.Models;
using DataAccess;
using Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repo
{
    public class ExerciseCategoryRepository : IExerciseCategoryRepository
    {
        public Task<ExerciseCategory> CreateExerciseCategoryAsync(ExerciseCategory exerciseCategory) => CategoryExerciseDAO.Instance.CreateExerciseCategoryAsync(exerciseCategory);

        public Task DeleteExerciseCategoryAsync(int id) => CategoryExerciseDAO.Instance.DeleteExerciseCategoryAsync(id);
       

        public Task<List<GetAllCategoryExeriseResponseDTO>> GetAllCategoryExercisesAsync() => CategoryExerciseDAO.Instance.GetAllCategoryExercisesAsync();

        public Task UpdateCategoryExercisesAsync(UpdateCategoryExerciseRequestDTO cate) => CategoryExerciseDAO.Instance.UpdateCategoryExercisesAsync(cate);
        
    }
}
