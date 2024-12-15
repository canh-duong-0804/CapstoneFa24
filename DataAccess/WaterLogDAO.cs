/*using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class WaterLogDAO
    {
        private static WaterLogDAO instance = null;
        private static readonly object instanceLock = new object();

        public static WaterLogDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new WaterLogDAO();
                    }
                    return instance;
                }
            }
        }

        private async Task<WaterIntake> GetOrCreateWaterLogAsync(int memberId, DateTime date)
        {
            using (var context = new HealthTrackingDBContext())
            {
                var waterLog = await context.WaterIntakes
                    .FirstOrDefaultAsync(wl => wl.MemberId == memberId && wl.Date.Date == date.Date);

                if (waterLog == null)
                {
                    waterLog = new WaterIntake
                    {
                        MemberId = memberId,
                        Date = date,
                        Amount = 0
                    };
                    context.WaterIntakes.Add(waterLog);
                    await context.SaveChangesAsync();
                }

                return waterLog;
            }
        }


        public async Task<bool> Add200mlWaterIntakeAsync(int memberId, DateTime date)
        {
            try
            {
                var waterLog = await GetOrCreateWaterLogAsync(memberId, date);


                waterLog.Amount += 200;

                using (var context = new HealthTrackingDBContext())
                {
                    context.WaterIntakes.Update(waterLog);
                    await context.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating water intake.", ex);
            }
        }


        public async Task<bool> Subtract200mlWaterIntakeAsync(int memberId, DateTime date)
        {
            try
            {
                var waterLog = await GetOrCreateWaterLogAsync(memberId, date);


                if (waterLog.Amount > 0)
                {
                    waterLog.Amount = Math.Max(0, waterLog.Amount - 200);

                    using (var context = new HealthTrackingDBContext())
                    {
                        context.WaterIntakes.Update(waterLog);
                        await context.SaveChangesAsync();
                    }

                    return true;
                }
                else
                {

                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating water intake.", ex);
            }
        }
    }
}*/