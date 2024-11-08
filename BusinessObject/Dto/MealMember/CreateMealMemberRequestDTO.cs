using BusinessObject.Dto.MealPlanDetailMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.MealMember
{
    public class CreateMealMemberRequestDTO
    {
        public string? Image { get; set; }
        public string NameMealMember { get; set; } = null!;
        //  public int? TotalCalories { get; set; }
        //public double? TotalProtein { get; set; }
        //public double? TotalCarb { get; set; }
        //public double? TotalFat { get; set; }
        //public DateTime? MealDate { get; set; }

        // List of meal details
        public List<CreateMealDetailMemberRequestDTO> MealDetails { get; set; } = new List<CreateMealDetailMemberRequestDTO>();



    }
}
