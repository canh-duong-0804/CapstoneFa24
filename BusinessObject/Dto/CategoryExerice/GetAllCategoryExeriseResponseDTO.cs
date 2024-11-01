using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.CategoryExerice
{
    public class GetAllCategoryExeriseResponseDTO
    {
        public int ExerciseCategoryId { get; set; }
        public string ExerciseCategoryName { get; set; } = null!;
        public bool? Status { get; set; }
        //public int? Value { get; set; }
    }
}
