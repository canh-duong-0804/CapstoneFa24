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
    public class NutrientRepository : INutrientRepository
    {
        public Task<DailyNutritionDto> CalculateDailyNutrition(int memberId, DateTime date) => NutrientDAO.Instance.CalculateDailyNutrition(memberId, date);
    }
}
