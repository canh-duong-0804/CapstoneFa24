using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Food
{
    public class GetFoodForMemberByIdResquestDTO
    {
        public int FoodId { get; set; }
        public DateTime SelectDate { get; set; }
    }
}
