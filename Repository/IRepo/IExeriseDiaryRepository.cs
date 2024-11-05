using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
	public interface IExeriseDiaryRepository
	{
		Task AddExerciseDiary(ExerciseDiary exerciseDiary);
		Task<List<ExerciseDiary>> GetExerciseDiaryByMemberId(int memberId);
		Task AddExerciseDiaries(List<ExerciseDiary> exerciseDiaries);
		Task<bool> CheckExercisePlanExistsAsync(int exercisePlanId);
		Task<bool> CheckExerciseExistsAsync(int exerciseId);
		Task<bool> DeleteExerciseDiary(int exerciseDiaryId);
	}
}
