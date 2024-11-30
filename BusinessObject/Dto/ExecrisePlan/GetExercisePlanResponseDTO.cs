using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.ExecrisePlan
{
    public class GetExercisePlanResponseDTO
    {
        public int ExercisePlanId { get; set; }
        /*public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? ChangeBy { get; set; }
        public DateTime? ChangeDate { get; set; }*/
        public string? ExercisePlanImage { get; set; }
        public string Name { get; set; } = null!;
        public double TotalCaloriesBurned { get; set; }
        
    }
    public class GetExercisePlanResponseForTrainerDTO
    {
        public List<ExercisePlanDTO?> Data { get; set; } = new List<ExercisePlanDTO>();
        public int TotalRecords { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
