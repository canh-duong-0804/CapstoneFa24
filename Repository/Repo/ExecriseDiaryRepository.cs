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

    }

}
