using BusinessObject.Dto.Nutrition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface ICaloriesRepository
    {
        Task<DailyCaloriesDto> CalculateDailyCalories(int memberId, DateTime date);
    }
}
