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
        Task<List<ExerciseDiary>> GetExerciseDiaryByMemberId(int memberId);
		Task<ExerciseDiary> GetTodayExerciseDiaryByMemberId(int memberId, DateTime today);
		Task AddExerciseDiaryAsync(ExerciseDiary exerciseDiary);

		Task<ExerciseDiary> GetExerciseDiaryByDate(int memberId, DateTime date);
		Task UpdateTotalDurationAndCaloriesAsync(int exerciseDiaryId);

		Task<ExerciseDiary> GetExerciseDiaryById(int exerciseDiaryId);
		Task<(int StreakCount, List<DateTime> StreakDates)> GetExerciseDiaryStreakWithDates(int memberId);

    }
}
