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
	public class ExecriseDiaryRepository : IExeriseDiaryRepository
	{
		public Task AddExerciseDiary(ExerciseDiary exerciseDiary) => ExerciseDiaryDAO.Instance.AddExerciseDiary(exerciseDiary);

		public Task<List<ExerciseDiary>> GetExerciseDiaryByMemberId(int memberId) => ExerciseDiaryDAO.Instance.GetExerciseDiaryByMemberId(memberId);

		public Task AddExerciseDiaries(List<ExerciseDiary> exerciseDiaries) => ExerciseDiaryDAO.Instance.AddExerciseDiaries(exerciseDiaries);
		public Task<bool> CheckExercisePlanExistsAsync(int exercisePlanId) => ExerciseDiaryDAO.Instance.CheckExercisePlanExistsAsync(exercisePlanId);

		public Task<bool> CheckExerciseExistsAsync(int exerciseId) => ExerciseDiaryDAO.Instance.CheckExerciseExistsAsync(exerciseId);

		public Task<bool> DeleteExerciseDiary(int exerciseDiaryId) => ExerciseDiaryDAO.Instance.DeleteExerciseDiary(exerciseDiaryId);
	}
	
}
