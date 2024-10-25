using BusinessObject.Dto.Exericse;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IExerciseRepository
    {

        Task<IEnumerable<AllExerciseResponseDTO>> GetAllExercisesAsync(); 
        Task<ExerciseDetailDTO> GetExerciseByIdAsync(int id); 
        Task<Exercise> SearchAndFilterExerciseByIdAsync(string searchName, string categortExerciseName); 
        Task<Exercise> CreateExerciseAsync(Exercise exercise); 
        Task<ExerciseCategory> CreateExerciseCategoryAsync(ExerciseCategory exercise); 
        Task<Exercise> UpdateExerciseAsync(Exercise exercise); 
        Task<bool> DeleteExerciseAsync(int id); 


    }
}
