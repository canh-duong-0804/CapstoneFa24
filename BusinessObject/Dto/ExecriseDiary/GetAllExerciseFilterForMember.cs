using BusinessObject.Dto.Exericse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.ExecriseDiary
{
    public class GetAllExerciseFilterForMember
    {
        public int ExerciseId { get; set; }
        public double? Weight { get; set; }
        public int? TypeExercise { get; set; }
        public string CategoryExercise { get; set; }
        public string? ExerciseImage { get; set; }
        public double? MetValue { get; set; }
        public string ExerciseName { get; set; } = null!;

        public int? RepsResitance1 { get; set; }
        public int? RepsResitance2 { get; set; }
        public int? RepsResitance3 { get; set; }
        public int? SetsResitance1 { get; set; }
        public int? SetsResitance2 { get; set; }
        public int? SetsResitance3 { get; set; }
        public int? MinutesResitance1 { get; set; }
        public int? MinutesResitance2 { get; set; }
        public int? MinutesResitance3 { get; set; }



        public int? MinutesCadior1 { get; set; }
        public int? MinutesCadior2 { get; set; }
        public int? MinutesCadior3 { get; set; }
        public double? CaloriesCadior1 { get; set; }
        public double? CaloriesCadior2 { get; set; }
        public double? CaloriesCadior3 { get; set; }
      /*  public GetExerciseDetailOfResitanceResponseDTO getExerciseDetailOfResitanceResponseDTO { get; set; }
         public GetExerciseDetailOfCardiorResponseDTO getExerciseDetailOfCardiorResponseDTO { get; set; }*/
    }
}
