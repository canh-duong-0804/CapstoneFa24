using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.FoodDiary
{
    public class FoodDiaryWithMealTypeDTO
    {
        public DateTime Date { get; set; }
        public int DiaryId { get; set; }
        public bool HasBreakfast { get; set; }
        public bool HasLunch { get; set; }
        public bool HasDinner { get; set; }
        public bool HasSnack { get; set; }
    }

}
