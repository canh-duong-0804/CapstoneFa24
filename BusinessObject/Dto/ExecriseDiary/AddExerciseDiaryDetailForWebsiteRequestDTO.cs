using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.ExecriseDiary
{
    public class AddExerciseDiaryDetailForWebsiteRequestDTO
    {

        public int ExerciseDiaryId { get; set; }

        public DateTime selectDate { get; set; }
        public List<ExerciseDiaryDetailForWebisteRequestDTO> ListFoodIdToAdd { get; set; } = new List<ExerciseDiaryDetailForWebisteRequestDTO>();

    }

    public class ExerciseDiaryDetailForWebisteRequestDTO
    {
        public int ExerciseId { get; set; }
        public int? DurationInMinutes { get; set; }
        public bool IsPractice { get; set; }

        public float CaloriesBurned { get; set; }

    }
}




    