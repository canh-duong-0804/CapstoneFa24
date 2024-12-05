using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.MainDashBoardAdmin
{
    public class TopExeriseStatisticsDTO
    {
        public int ExerciseId { get; set; }
        public string ExerciseName { get; set; }
        public int UsageCount { get; set; }
        public double UsagePercentage { get; set; }
    }
}
