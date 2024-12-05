using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.ExecriseDiary
{
    public class ExerciseDiaryForAllMonthDTO
    {
        public DateTime Date { get; set; }
        public int exerciseDiaryId { get; set; }
        public bool HasExercise { get; set; }
        
    }
}
