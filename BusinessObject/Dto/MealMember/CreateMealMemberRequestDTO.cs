using BusinessObject.Dto.MealPlanDetailMember;
using Microsoft.AspNetCore.Http;
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
        public List<CreateMealDetailMemberRequestDTO> MealDetails { get; set; } = new();



    }
}
