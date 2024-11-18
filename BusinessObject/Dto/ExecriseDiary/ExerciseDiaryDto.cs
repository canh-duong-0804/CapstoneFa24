using BusinessObject.DTOs;

namespace BusinessObject.DTOs
{
    public class ExerciseDiaryDTO
    {
        public int ExerciseDiaryId { get; set; }
        public int MemberId { get; set; }
        public int? ExercisePlanId { get; set; }
        public DateTime? Date { get; set; }
        public int? TotalDuration { get; set; }
        public double? TotalCaloriesBurned { get; set; }
        public List<ExerciseDiaryDetailDTO>? ExerciseDiaryDetails { get; set; }
    }
}
