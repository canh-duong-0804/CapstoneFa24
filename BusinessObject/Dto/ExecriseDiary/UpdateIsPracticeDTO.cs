using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.ExecriseDiary
{
    public class UpdateIsPracticeDTO
    {
        public int DiaryId { get; set; }
/*        public int ExerciseId { get; set; }*/
        public bool IsPractice { get; set; }

        public int ExerciseDiaryDetailId { get; set; }
    }
}
