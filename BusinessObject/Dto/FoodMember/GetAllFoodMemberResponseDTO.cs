using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.FoodMember
{
    public class GetAllFoodMemberResponseDTO
    {
        public int FoodId { get; set; }
        public string FoodName { get; set; } = null!;
   
        public double? Calories { get; set; }
        public double? Protein { get; set; }
        public double? Carbs { get; set; }
        public double? Fat { get; set; }
        public string? FoodImage { get; set; }

    }
}
