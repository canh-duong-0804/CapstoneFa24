using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.ExecriseDiary
{
	
		public class NewExerciseDiaryBatchDTO
		{
			public int MemberId { get; set; }               // Member logging the exercises
			public DateTime Date { get; set; }              // Date for all exercises in this batch
			public List<NewExerciseDiaryDTO> Exercises { get; set; } = new List<NewExerciseDiaryDTO>();
		}

		public class NewExerciseDiaryDTO
		{
			public int ExercisePlanId { get; set; }         // ID of the exercise plan
			public int ExerciseId { get; set; }             // ID of the specific exercise
			public int Duration { get; set; }               // Duration in minutes
			public float CaloriesBurned { get; set; }       // Calories burned
		}
	
}
