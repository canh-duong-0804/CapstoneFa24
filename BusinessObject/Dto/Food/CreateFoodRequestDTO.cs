using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Food
{
    public class CreateFoodRequestDTO
    {
      
        public string FoodName { get; set; } = null!;
        public string Portion { get; set; } = null!;
        public string Serving { get; set; } = null!;
        public string dietname { get; set; } = null!;
        public double Calories { get; set; }
       // public int CreateBy { get; set; }    
        public string? FoodImage { get; set; }
        public double Protein { get; set; }
        public double Carbs { get; set; }
        public double Fat { get; set; }
        public double VitaminA { get; set; }
        public double VitaminC { get; set; }
        public double VitaminD { get; set; }
        public double VitaminE { get; set; }
        public double VitaminB1 { get; set; }
        public double VitaminB2 { get; set; }
        public double VitaminB3 { get; set; }
        public bool? Status { get; set; }
        public int? DietId { get; set; }
    }
}
