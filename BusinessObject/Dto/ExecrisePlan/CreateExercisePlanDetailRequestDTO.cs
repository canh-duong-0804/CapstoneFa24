﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.ExecrisePlan
{
    public class CreateExercisePlanDetailRequestDTO
    {
        public int ExercisePlanId { get; set; }
        public byte Day { get; set; }
        public List<ExecriseInPlan> ExecriseInPlans { get; set; }
    }
    public class ExecriseInPlan
    {
        public int ExerciseId { get; set; }
       
        public int Duration { get; set; }
    }

}
