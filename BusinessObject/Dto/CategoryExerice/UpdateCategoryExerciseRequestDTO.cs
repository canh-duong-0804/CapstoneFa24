using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.CategoryExerice
{
    public class UpdateCategoryExerciseRequestDTO
    {
        public int ExerciseCategoryId { get; set; }
        public string ExerciseCategoryName { get; set; } = null!;
    }
}
