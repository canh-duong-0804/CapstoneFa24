using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Exericse
{
    public class AllExerciseResponseDTO
    {
        public int ExerciseCategoryId { get; set; }
        public string ExerciseCategoryName { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? ExerciseLevel { get; set; }

       
        public string? ExerciseImage { get; set; }

        public string ExerciseName { get; set; } = null!;
        public string? Description { get; set; }
        public double CaloriesPerHour { get; set; }
    }
}
