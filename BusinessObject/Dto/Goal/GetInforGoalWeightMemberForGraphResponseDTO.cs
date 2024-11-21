using BusinessObject.Dto.MealDetailMember;
using BusinessObject.Dto.MealMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Goal
{
    public class GetInforGoalWeightMemberForGraphResponseDTO
    {
        public List<CurrentWeightMemberResponseDTO> currentWeight { get; set; } = new List<CurrentWeightMemberResponseDTO>();

        public List<GoalWeightMemberResponseDTO> goalWeight { get; set; } = new List<GoalWeightMemberResponseDTO>();
    }
}
