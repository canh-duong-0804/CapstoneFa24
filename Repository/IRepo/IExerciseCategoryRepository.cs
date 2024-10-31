using BusinessObject.Dto.CategoryExerice;
using BusinessObject.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IExerciseCategoryRepository
    {
        Task<ExerciseCategory> CreateExerciseCategoryAsync(ExerciseCategory exercise);
        Task UpdateCategoryExercisesAsync(UpdateCategoryExerciseRequestDTO cate);
        Task DeleteExerciseCategoryAsync(int id);
        Task<List<GetAllCategoryExeriseResponseDTO>> GetAllCategoryExercisesAsync();
    }
}
