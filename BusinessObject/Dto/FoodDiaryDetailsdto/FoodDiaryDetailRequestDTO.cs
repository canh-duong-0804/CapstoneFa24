using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.FoodDiaryDetails
{
    public class FoodDiaryDetailRequestDTO
    {
       // public int DiaryDetailId { get; set; }
        public int DiaryId { get; set; }
        public int FoodId { get; set; }
        public double Quantity { get; set; }
        public int MealType { get; set; }
    }
}
