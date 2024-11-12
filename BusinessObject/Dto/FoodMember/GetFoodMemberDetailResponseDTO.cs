using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.FoodMember
{
    public class GetFoodMemberDetailResponseDTO
    {
        public int FoodId { get; set; }
        public string FoodName { get; set; } = null!;
        public string? Portion { get; set; }
        public double? Calories { get; set; }
        public double? Protein { get; set; }
        public double? Carbs { get; set; }
        public double? Fat { get; set; }
        public double? VitaminA { get; set; }
        public double? VitaminC { get; set; }
        public double? VitaminD { get; set; }
        public double? VitaminE { get; set; }
        public double? VitaminB1 { get; set; }
        public double? VitaminB2 { get; set; }
        public double? VitaminB3 { get; set; }
        public string? FoodImage { get; set; }
       
    }
}
