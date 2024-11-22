﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IWaterLogRepository
    {
        Task<bool> AddOneLiterAsync(int memberId, DateTime date);
        Task<bool> SubtractWaterIntakeAsync(int memberId, DateTime date);
    }
}