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
        
        public Task<List<ExerciseDiary>> GetExerciseDiaryByMemberId(int memberId) => ExerciseDiaryDAO.Instance.GetExerciseDiaryByMemberId(memberId);
        public Task<ExerciseDiary> GetTodayExerciseDiaryByMemberId(int memberId, DateTime today) => ExerciseDiaryDAO.Instance.GetTodayExerciseDiaryByMemberId(memberId, today);
		public Task AddExerciseDiaryAsync(ExerciseDiary exerciseDiary) => ExerciseDiaryDAO.Instance.AddExerciseDiaryAsync(exerciseDiary);
		public Task<ExerciseDiary> GetExerciseDiaryByDate(int memberId, DateTime date) => ExerciseDiaryDAO.Instance.GetExerciseDiaryByDate(memberId, date);
		public Task UpdateTotalDurationAndCaloriesAsync(int exerciseDiaryId) => ExerciseDiaryDAO.Instance.UpdateTotalDurationAndCaloriesAsync(exerciseDiaryId);

		public Task<ExerciseDiary> GetExerciseDiaryById(int exerciseDiaryId) => ExerciseDiaryDAO.Instance.GetExerciseDiaryById(exerciseDiaryId);
	}

}
