using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Food
{
    public class AllFoodForStaffResponseDTO
    {
        public string Name { get; set; }
        public int CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ChangeDate { get; set; }
        public string? FoodImage { get; set; }
        public double Calories { get; set; }
        public string DietName { get; set; } // Chỉ lấy tên chế độ ăn
    }
}
