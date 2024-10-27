using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Food
{
    public class AllFoodForMemberResponseDTO
    {
        public string FoodName { get; set; }
        public string? FoodImage { get; set; }
        public double Calories { get; set; }
        public string DietName { get; set; } // Chỉ lấy tên chế độ ăn
    }
}
