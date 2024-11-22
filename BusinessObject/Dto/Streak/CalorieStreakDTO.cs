using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Streak
{
    public class CalorieStreakDTO
    {
        public List<DateTime> Dates { get; set; } = new List<DateTime>(); 
        public int StreakNumber { get; set; } 
    }

}
