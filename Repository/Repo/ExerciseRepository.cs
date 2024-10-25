using BusinessObject.Dto.Exericse;
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
    public class ExerciseRepository : IExerciseRepository
    {
        public Task<Exercise> CreateExerciseAsync(Exercise exercise) => ExerciseDAO.Instance.CreateExerciseAsync(exercise);

        public Task<ExerciseCategory> CreateExerciseCategoryAsync(ExerciseCategory exerciseCategory) => ExerciseDAO.Instance.CreateExerciseCategoryAsync(exerciseCategory);
        

        public Task<bool> DeleteExerciseAsync(int id) => ExerciseDAO.Instance.DeleteExerciseAsync(id);
       

        public Task<IEnumerable<AllExerciseResponseDTO>> GetAllExercisesAsync() => ExerciseDAO.Instance.GetAllExercisesAsync();

        public Task<ExerciseDetailDTO> GetExerciseByIdAsync(int id) => ExerciseDAO.Instance.GetExerciseByIdAsync(id);
       

        public Task<Exercise> SearchAndFilterExerciseByIdAsync(string searchName, string categortExerciseName) => ExerciseDAO.Instance.SearchAndFilterExerciseByIdAsync(searchName, categortExerciseName);
       

        public Task<Exercise> UpdateExerciseAsync(Exercise exercise)
        {
            throw new NotImplementedException();
        }
    }
}
