using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.FoodDiary
{
    public class FoodDiaryForMealResponseDTO
    {
        public int DiaryDetailId { get; set; }
        public int DiaryId { get; set; }
        public int FoodId { get; set; }
        public string Portion { get; set; } = null!;
        public string FoodName {  get; set; }
        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Carbs { get; set; }
        public double Fat { get; set; }
        public double Quantity { get; set; }
        public int MealType { get; set; }
    }
}
