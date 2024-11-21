using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.FoodDiary
{
    public class GetFoodDiaryDateResponseDTO
    {
        public int DiaryId { get; set; }
        
        public DateTime Date { get; set; }
    }
}
