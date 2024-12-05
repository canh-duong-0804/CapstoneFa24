using AutoMapper.Execution;
using BusinessObject.Dto.ExecriseDiary;
using BusinessObject.Dto.Exericse;
using BusinessObject.Models;
using DataAccess;
using Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Http;

namespace Repository.Repo
{
	public class ExecriseDiaryDetailRepository : IExecriseDiaryDetailRepository
	{
		public Task<double> CalculateCaloriesBurnedAsync(int exerciseId, double bodyWeightKg, int durationInMinutes) => ExecriseDiaryDetailDAO.Instance.CalculateCaloriesBurnedAsync(exerciseId, bodyWeightKg, durationInMinutes);
		public Task AddDiaryDetailAsync(ExerciseDiaryDetail diaryDetail) => ExecriseDiaryDetailDAO.Instance.AddDiaryDetailAsync(diaryDetail);
		public Task<double?> GetLatestBodyWeightAsync(string memberId) => ExecriseDiaryDetailDAO.Instance.GetLatestBodyWeightAsync(memberId);
		public Task<double?> GetExerciseMetValueAsync(int exerciseId) => ExecriseDiaryDetailDAO.Instance.GetExerciseMetValueAsync(exerciseId);
		public Task<Exercise?> GetExerciseAsync(int exerciseId) => ExecriseDiaryDetailDAO.Instance.GetExerciseAsync(exerciseId);
       public Task UpdateExerciseDiaryDetailAsync(ExerciseDiaryDetail detail) => ExecriseDiaryDetailDAO.Instance.UpdateExerciseDiaryDetailAsync(detail);
       public Task<ExerciseDiaryDetail?> GetExerciseDiaryDetailById(int exerciseDiaryDetailId) => ExecriseDiaryDetailDAO.Instance.GetExerciseDiaryDetailById(exerciseDiaryDetailId);
		public Task DeleteDiaryDetailAsync(int diaryDetailId) => ExecriseDiaryDetailDAO.Instance.DeleteDiaryDetailAsync(diaryDetailId);

        public Task<ExerciseDiaryDetail?> GetDiaryDetailByIdAsync(int diaryDetailId) => ExecriseDiaryDetailDAO.Instance.GetDiaryDetailByIdAsync(diaryDetailId);
		public Task UpdateDiaryDetailAsync(ExerciseDiaryDetail diaryDetail) => ExecriseDiaryDetailDAO.Instance.UpdateDiaryDetailAsync(diaryDetail);
		
		public Task AssignExercisePlanToUserAsync(int memberId, int planId, DateTime startDate) => ExecriseDiaryDetailDAO.Instance.AssignExercisePlanToUserAsync(memberId, planId, startDate);

        public Task<List<ExerciseDiaryForAllMonthDTO>> GetAllDiariesForMonthOfExercise(DateTime date, int memberId) => ExecriseDiaryDetailDAO.Instance.GetAllDiariesForMonthOfExercise(date, memberId);

        public Task<bool> addExerciseListToDiaryForWebsite(AddExerciseDiaryDetailForWebsiteRequestDTO request, int memberId) => ExecriseDiaryDetailDAO.Instance.addExerciseListToDiaryForWebsite(request, memberId);

        public Task<AddExerciseDiaryDetailForWebsiteRequestDTO> GetExerciseDairyDetailWebsite(int memberId, DateTime selectDate) => ExecriseDiaryDetailDAO.Instance.GetExerciseDairyDetailWebsite(memberId, selectDate);

        public Task<List<ExerciseListBoxResponseDTO>> GetListBoxExerciseForStaffAsync() => ExecriseDiaryDetailDAO.Instance.GetListBoxExerciseForStaffAsync();
        
    }


}
