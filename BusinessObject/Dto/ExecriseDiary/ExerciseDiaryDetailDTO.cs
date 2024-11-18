namespace BusinessObject.DTOs
{
    
        public class ExerciseDiaryDetailDTO
        {
            // Unique identifier for the exercise diary detail
            public int? ExerciseDiaryDetailsId { get; set; }

            // Reference to the parent exercise diary
            public int? ExerciseDiaryId { get; set; }

            // Indicates if this exercise is a practice session
            public bool? IsPractice { get; set; }

            // Reference to the associated exercise
            public int? ExerciseId { get; set; }

            // Additional information about the exercise
            public string? ExerciseName { get; set; }
            public string? ExerciseImage { get; set; }

            // Duration of the exercise in minutes
            public int? Duration { get; set; }

            // Calories burned during the exercise
            public double? CaloriesBurned { get; set; }
        }
    

}
