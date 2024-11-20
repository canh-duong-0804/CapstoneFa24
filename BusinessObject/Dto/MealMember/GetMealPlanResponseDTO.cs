using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.MealMember
{
    public class GetMealPlanResponseDTO
    {
        public int MealPlanId { get; set; }
        public string CreateBy { get; set; }
        public string? MealPlanImage { get; set; }
        public DateTime? ChangeDate { get; set; }
        public string? ChangeBy { get; set; }
        public int? DietId { get; set; }
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public string Name { get; set; } = null!;
        public double TotalCalories { get; set; }
        public DateTime CreatedAt { get; set; }
        
    }
}
