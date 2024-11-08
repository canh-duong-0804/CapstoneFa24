using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.FoodDiary
{
    public class FoodDiaryForMemberMobileResponse
    {
        public int DiaryId { get; set; }
      
        
        public DateTime Date { get; set; }
        public double? Calories { get; set; }
        public double? Protein { get; set; }
        public double? Fat { get; set; }
        public double? Carbs { get; set; }
    }
}
