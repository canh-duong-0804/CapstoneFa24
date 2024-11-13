using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.MealMember
{
    public class GetAllMealMemberResonseDTO
    {
        public int MealMemberId { get; set; }

        public string? Image { get; set; }
        public string NameMealPlanMember { get; set; } = null!;
        public int? TotalCalories { get; set; }
        public double? TotalProtein { get; set; }
        public double? TotalCarb { get; set; }
        public double? TotalFat { get; set; }
        public DateTime? MealDate { get; set; }
    }
}
