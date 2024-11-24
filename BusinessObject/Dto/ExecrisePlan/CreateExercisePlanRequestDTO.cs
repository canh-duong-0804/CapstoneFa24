﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.ExecrisePlan
{
    public class CreateExercisePlanRequestDTO
    {
        public string Name { get; set; } = null!;
        public double TotalCaloriesBurned { get; set; }
        public string? ExercisePlanImage { get; set; }
        public bool? Status { get; set; }
    }
}