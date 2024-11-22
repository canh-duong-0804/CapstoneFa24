using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.CopyMeal
{
    public class InsertCopyMealDTO
    {

        public int DiaryIdPreviouseMeal {  get; set; }
        public int MealTypePreviousMeal { get; set; }
       
        
        public DateTime SelectDateToAdd { get; set; }

    }
}
