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
    
    public class GetExerciseResponseForTrainerDTO
    {
        public List<GetExerciseResponseDTO?> Data { get; set; } = new List<GetExerciseResponseDTO>();
        public int TotalRecords { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }

    public class GetExerciseResponseDTO
    {
        public int ExerciseId { get; set; }
      
        public string? TypeExercise { get; set; }
        public string? ExerciseImage { get; set; }
      
        public string ExerciseName { get; set; } = null!;
        public string? Description { get; set; }
        public double? MetValue { get; set; }

    }
}
