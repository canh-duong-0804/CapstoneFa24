using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Nutrition
{
    public class DailyCaloriesDto
    {
        public Dictionary<string, double> CaloriesByMeal { get; set; } = new Dictionary<string, double>(); // Calories phân theo MealType
        public double TotalCalories { get; set; } // Tổng calories
        public double NetCalories { get; set; } // Net calories (Total - tiêu thụ)
        public double GoalCalories { get; set; } // Mục tiêu calories
        public FoodCaloriesDto FoodsWithHighestCalories { get; set; } // Danh sách món ăn
    }

    public class FoodCaloriesDto
    {
        public int FoodDiaryDetailId { get; set; }  
        public string FoodName { get; set; }
        public double Calories { get; set; }
    }

}
