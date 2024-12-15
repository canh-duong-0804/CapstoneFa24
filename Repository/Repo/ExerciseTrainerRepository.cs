using BusinessObject.Dto.ExecrisePlan;
using BusinessObject.Dto.ExerciseTrainer;
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
    public class ExerciseTrainerRepository : IExerciseTrainerRepository
    {
        public Task<int> CreateExerciseTrainerAsync(CreateExerciseRequestDTO request, int memberId) => ExerciseTrainerDAO.Instance.CreateExerciseTrainerAsync(request,memberId);

        public Task<bool> DeleteExerciseAsync(int exerciseId) => ExerciseTrainerDAO.Instance.DeleteExerciseAsync(exerciseId);

        public Task<GetExerciseResponseForTrainerDTO> GetAllExerciseAsync(int page, int pageSize, string? searchExercise) => ExerciseTrainerDAO.Instance.GetAllExerciseAsync(page, pageSize,searchExercise);

        public Task<ExerciseRequestDTO> GetExerciseDetailAsync(int exerciseId) => ExerciseTrainerDAO.Instance.GetExerciseDetailAsync(exerciseId);

        public Task<int> GetTotalExercisesAsync(string? SearchExericse) => ExerciseTrainerDAO.Instance.GetTotalExercisesAsync(SearchExericse);
       

        public Task<ExerciseRequestDTO> UpdateExerciseAsync(int exerciseId, ExerciseRequestDTO updateRequest) => ExerciseTrainerDAO.Instance.UpdateExerciseAsync(exerciseId, updateRequest);

        public Task<bool> UploadImageForMealMember(string urlImage, int exerciseId) => ExerciseTrainerDAO.Instance.UploadImageForMealMember(urlImage, exerciseId);

    }
}
