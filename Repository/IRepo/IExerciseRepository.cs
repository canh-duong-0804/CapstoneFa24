using BusinessObject.Dto.CategoryExerice;
using BusinessObject.Dto.ExecriseDiary;
using BusinessObject.Dto.Exericse;
using BusinessObject.Dto.SearchFilter;
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
        Task<List<GetAllExerciseFilterForMember>> GetAllExercisesFilterAsync(string? search, int? isCardioFilter, int memberId);

        /*Task<IEnumerable<AllExerciseResponseDTO>> GetAllExercisesAsync(); 
        Task<ExerciseDetailDTO> GetExerciseByIdAsync(int id);
        Task<IEnumerable<AllExerciseResponseDTO>> SearchAndFilterExerciseByIdAsync(SearchFilterObjectDTO searchName); 
       Task<Exercise> CreateExerciseAsync(Exercise exercise); 
        
        Task<UpdateExerciseRequestDTO> UpdateExerciseAsync(UpdateExerciseRequestDTO exercise); 
        Task<bool> DeleteExerciseAsync(int id);*/
        Task<List<GetAllExerciseForMember>> GetAllExercisesForMemberAsync(string? search, int? isCardioFilter, int memberId);
        Task<GetExerciseDetailOfCardiorResponseDTO> GetExercisesCardioDetailForMemberrAsync(int exerciseId, int memberId);
        Task<GetExerciseDetailOfOtherResponseDTO> GetExercisesOtherDetailForMemberAsync(int exerciseId, int memberId);
        Task<GetExerciseDetailOfResitanceResponseDTO> GetExercisesResistanceDetailForMemberAsync(int exerciseId);
    }
}
