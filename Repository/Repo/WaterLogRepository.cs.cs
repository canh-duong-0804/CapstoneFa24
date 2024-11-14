using DataAccess;
using Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repo
{
    public class WaterLogRepository : IWaterLogRepository
    {
        public Task<bool> AddOneLiterAsync(int memberId, DateTime date) => WaterLogDAO.Instance.Add200mlWaterIntakeAsync(memberId, date);

        public Task<bool> SubtractWaterIntakeAsync(int memberId, DateTime date) => WaterLogDAO.Instance.Subtract200mlWaterIntakeAsync(memberId, date);

    }
}
