using System;
using System.Collections.Generic;

namespace BusinessObject.Dto.ExerciseDiary
{
	public class ExerciseDiaryDto
	{
		public int ExerciseDiaryId { get; set; }
		public DateTime? Date { get; set; }
		public ExerciseDto Exercise { get; set; }  // Single Exercise, not a List
	}

	public class ExerciseDto
	{
		public int ExerciseId { get; set; }
		public string ExerciseName { get; set; }
		public int? Duration { get; set; }
		public double CaloriesBurned { get; set; }
	}
}
