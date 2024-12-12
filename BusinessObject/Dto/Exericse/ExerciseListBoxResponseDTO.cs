using BusinessObject.Dto.CategoryExerice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Exericse
{
    public class ExerciseListBoxResponseDTO : ListBoxResponseDTO
    {

        public int ExerciseId { get; set; }
        public int Duration { get; set; }
        public int? TypeExercise { get; set; }
        public string? NameTypeExercise { get; set; }
       
        public string ExerciseName { get; set; } = null!;
       // public string? Description { get; set; }
        public double? MetValue { get; set; }

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

    }
}

