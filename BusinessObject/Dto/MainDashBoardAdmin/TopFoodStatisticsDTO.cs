using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.MainDashBoardTrainer
{
    public class TopFoodStatisticsDTO
    {
        public int FoodId { get; set; } 
        public string FoodName { get; set; } 
        public int UsageCount { get; set; } 
        public double UsagePercentage { get; set; } 
    }

}
