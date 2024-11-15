﻿namespace BusinessObject.DTOs
{
    public class ExerciseDiaryDetailDTO
    {
        public int? ExerciseDiaryDetailsId { get; set; }
        public int? ExerciseDiaryId { get; set; }
        public bool? IsPractice { get; set; }
        public int? ExerciseId { get; set; }
        public int? Duration { get; set; }
        public double? CaloriesBurned { get; set; }
    }
}
