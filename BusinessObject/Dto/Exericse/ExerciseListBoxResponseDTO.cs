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
      
        public int? TypeExercise { get; set; }
        public string? NameTypeExercise { get; set; }
       
        public string ExerciseName { get; set; } = null!;
       // public string? Description { get; set; }
        public double? MetValue { get; set; }
       
    }
}

