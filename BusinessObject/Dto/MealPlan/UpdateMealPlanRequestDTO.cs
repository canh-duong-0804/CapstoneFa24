﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.MealPlan
{
    public class UpdateMealPlanRequestDTO
    {
        public int MealPlanId { get; set; }
        public string? MealPlanImage { get; set; }
        public DateTime? ChangeDate { get; set; }
        public int? ChangeBy { get; set; }
        public int? DietId { get; set; }
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public string Name { get; set; } = null!;
        public double TotalCalories { get; set; }
        

    }
}
