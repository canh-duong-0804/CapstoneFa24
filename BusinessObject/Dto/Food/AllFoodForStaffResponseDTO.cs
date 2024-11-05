using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Food
{
    public class AllFoodForStaffResponseDTO
    {
        public int FoodId { get; set; }
        public string FoodName { get; set; }
        public int CreateBy { get; set; }
        public string CreateByName { get; set; }
        public int? ChangeBy { get; set; }
        public string? ChangeByName { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ChangeDate { get; set; }
        public string? FoodImage { get; set; }
        public double Calories { get; set; }
        public string DietName { get; set; } // Chỉ lấy tên chế độ ăn
    }
}
