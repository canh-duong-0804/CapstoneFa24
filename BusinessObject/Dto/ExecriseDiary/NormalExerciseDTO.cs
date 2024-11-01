
using System;

namespace BusinessObject.Dto.ExerciseDiary
{
	public class NormalExerciseDTO
	{
		public int ExerciseId { get; set; }       // The ID of the exercise
		public int Duration { get; set; }          // Duration of the exercise in minutes
		public float CaloriesBurned { get; set; }  // Calories burned during the exercise
	}

	public class NewNormalExerciseDiaryDTO
	{
		public DateTime Date { get; set; }                     // The date of the exercise
		public List<NormalExerciseDTO> Exercises { get; set; } // List of exercises to log
	}
}
