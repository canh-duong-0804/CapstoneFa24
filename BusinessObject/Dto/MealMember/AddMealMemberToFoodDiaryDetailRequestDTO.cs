using BusinessObject.Dto.MealDetailMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.MealMember
{
    public class AddMealMemberToFoodDiaryDetailRequestDTO
    {





        //public DateTime? MealDate { get; set; }
        public int DiaryId { get; set; }
        public int MealType { get; set; }
        public int MealMemberId { get; set; }
      //  public DateTime MealDate { get; set; }



       // public List<GetAllFoodOfMealMemberResonseDTO> FoodDetails { get; set; } = new List<GetAllFoodOfMealMemberResonseDTO>();
    }
}
