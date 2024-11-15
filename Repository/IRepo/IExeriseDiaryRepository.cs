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

    }
}
