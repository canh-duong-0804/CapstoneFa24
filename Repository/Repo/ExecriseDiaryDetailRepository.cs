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
	public class ExecriseDiaryDetailRepository : IExecriseDiaryDetailRepository
	{
		public Task<double> CalculateCaloriesBurnedAsync(int exerciseId, double bodyWeightKg, int durationInMinutes) => ExecriseDiaryDetailDAO.Instance.CalculateCaloriesBurnedAsync(exerciseId, bodyWeightKg, durationInMinutes);
		public Task AddDiaryDetailAsync(ExerciseDiaryDetail diaryDetail) => ExecriseDiaryDetailDAO.Instance.AddDiaryDetailAsync(diaryDetail);
		public Task<double?> GetLatestBodyWeightAsync(string memberId) => ExecriseDiaryDetailDAO.Instance.GetLatestBodyWeightAsync(memberId);
		public Task<double?> GetExerciseMetValueAsync(int exerciseId) => ExecriseDiaryDetailDAO.Instance.GetExerciseMetValueAsync(exerciseId);
		public Task<Exercise?> GetExerciseAsync(int exerciseId) => ExecriseDiaryDetailDAO.Instance.GetExerciseAsync(exerciseId);
       public Task UpdateExerciseDiaryDetailAsync(ExerciseDiaryDetail detail) => ExecriseDiaryDetailDAO.Instance.UpdateExerciseDiaryDetailAsync(detail);
       public Task<ExerciseDiaryDetail?> GetExerciseDiaryDetailById(int exerciseDiaryDetailId) => ExecriseDiaryDetailDAO.Instance.GetExerciseDiaryDetailById(exerciseDiaryDetailId);

    }
	

}
