using BusinessObject.Dto.CategoryExerice;
using BusinessObject.Dto.Exericse;
using BusinessObject.Dto.SearchFilter;
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
        /* public Task<Exercise> CreateExerciseAsync(Exercise exercise) => ExerciseDAO.Instance.CreateExerciseAsync(exercise);


          //
          //public Task<ExerciseCategory> CreateExerciseCategoryAsync(ExerciseCategory exerciseCategory) => ExerciseDAO.Instance.CreateExerciseCategoryAsync(exerciseCategory);



          public Task<bool> DeleteExerciseAsync(int id) => ExerciseDAO.Instance.DeleteExerciseAsync(id);
          //
         // public Task<GetAllCategoryExeriseResponseDTO> GetAllCategoryExercisesAsync() => ExerciseDAO.Instance.GetAllCategoryExercisesAsync();


          public Task<IEnumerable<AllExerciseResponseDTO>> GetAllExercisesAsync() => ExerciseDAO.Instance.GetAllExercisesAsync();

          public Task<ExerciseDetailDTO> GetExerciseByIdAsync(int id) => ExerciseDAO.Instance.GetExerciseByIdAsync(id);


          public Task<IEnumerable<AllExerciseResponseDTO >> SearchAndFilterExerciseByIdAsync(SearchFilterObjectDTO searchName) => ExerciseDAO.Instance.SearchAndFilterExerciseByIdAsync(searchName);


          public Task<UpdateExerciseRequestDTO> UpdateExerciseAsync(UpdateExerciseRequestDTO exercise) => ExerciseDAO.Instance.UpdateExerciseAsync(exercise);*/
        public Task<List<GetAllExerciseForMember>> GetAllExercisesForMemberAsync(string? search, int? isCardioFilter) => ExerciseDAO.Instance.GetAllExercisesForMemberAsync(search,isCardioFilter);

        public Task<GetExerciseDetailOfCardiorResponseDTO> GetExercisesCardioDetailForMemberrAsync(int exerciseId) => ExerciseDAO.Instance.GetExercisesCardioDetailForMemberrAsync(exerciseId);

        public Task<GetExerciseDetailOfOtherResponseDTO> GetExercisesOtherDetailForMemberAsync(int exerciseId) => ExerciseDAO.Instance.GetExercisesOtherDetailForMemberAsync(exerciseId);
       

        public Task<GetExerciseDetailOfResitanceResponseDTO> GetExercisesResistanceDetailForMemberAsync(int exerciseId) => ExerciseDAO.Instance.GetExercisesResistanceDetailForMemberAsync(exerciseId);



    }
}
