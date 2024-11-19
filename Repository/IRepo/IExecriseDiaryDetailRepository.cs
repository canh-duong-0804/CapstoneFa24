using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
	public interface IExecriseDiaryDetailRepository
	{
		Task<double> CalculateCaloriesBurnedAsync(int exerciseId, double bodyWeightKg, int durationInMinutes);
		Task AddDiaryDetailAsync(ExerciseDiaryDetail diaryDetail);
		Task<double?> GetLatestBodyWeightAsync(string memberId);
		Task<double?> GetExerciseMetValueAsync(int exerciseId);
		Task<Exercise?> GetExerciseAsync(int exerciseId);

		Task UpdateExerciseDiaryDetailAsync(ExerciseDiaryDetail detail);
		Task<ExerciseDiaryDetail?> GetExerciseDiaryDetailById(int exerciseDiaryDetailId);

		Task DeleteDiaryDetailAsync(int diaryDetailId);

        Task<ExerciseDiaryDetail?> GetDiaryDetailByIdAsync(int diaryDetailId);

        Task UpdateDiaryDetailAsync(ExerciseDiaryDetail diaryDetail);

		Task AssignExercisePlanToUserAsync(int memberId, int planId, DateTime startDate);

    }
}
