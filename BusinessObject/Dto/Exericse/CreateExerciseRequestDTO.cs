﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Exericse
{
    public class CreateExerciseRequestDTO
    {
       
        public int ExerciseCategoryId { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? ExerciseLevel { get; set; }
       
        public int? Reps { get; set; }
        public int? Sets { get; set; }
        public int? Minutes { get; set; }
        public string? ExerciseImage { get; set; }
    
        public string ExerciseName { get; set; } = null!;
        public string? Description { get; set; }
        public double CaloriesPerHour { get; set; }
    }
}