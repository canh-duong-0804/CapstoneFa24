using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.ExecriseDiary
{
	public class NewExerciseDetailDTO
	{
		public int ExerciseDiaryId { get; set; }
		public int ExerciseId { get; set; }
		public int DurationInMinutes { get; set; }
		public bool IsPractice { get; set; }
		
	}
}
