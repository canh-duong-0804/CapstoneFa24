using BusinessObject.Dto.ExecrisePlan;
using BusinessObject.Dto.ExerciseTrainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IExerciseTrainerRepository
    {
        Task<int> CreateExerciseTrainerAsync(CreateExerciseRequestDTO request, int memberId);
        Task<bool> DeleteExerciseAsync(int exerciseId);
        Task<GetExerciseResponseForTrainerDTO> GetAllExercisePlansAsync(int page, int pageSize);
        Task<ExerciseRequestDTO> GetExerciseDetailAsync(int exerciseId);
        Task<ExerciseRequestDTO> UpdateExerciseAsync(int exerciseId, ExerciseRequestDTO updateRequest);
        Task<bool> UploadImageForMealMember(string v, int exerciseId);
    }
}
