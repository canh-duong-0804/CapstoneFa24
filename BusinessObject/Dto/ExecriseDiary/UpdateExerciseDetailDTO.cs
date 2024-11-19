namespace BusinessObject.Dto.ExecriseDiary
{
    public class UpdateExerciseDetailDTO
    {
        public int ExerciseDiaryDetailId { get; set; }
        public int? DurationInMinutes { get; set; }
        public double? CaloriesBurned { get; set; }
        public bool? IsPractice { get; set; }
    }
}
