using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.FoodDiaryDetails
{
    public class AddListFoodToDiaryRequestDTO
    {
        public List<FoodDiaryDetailRequestDTO> listFoodDiaryDetail { get; set; }
    }
}
