using BusinessObject.Dto.Nutrition;
using DataAccess;
using Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repo
{
    public class CaloriesRepository : ICaloriesRepository
    {
        public Task<DailyCaloriesDto> CalculateDailyCalories(int memberId, DateTime date) => CaloriesDAO.Instance.CalculateDailyCalories(memberId, date);
    }
}
