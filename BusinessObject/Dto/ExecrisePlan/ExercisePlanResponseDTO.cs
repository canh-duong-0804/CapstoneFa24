using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.ExecrisePlan
{
    public class ExercisePlanResponseDTO
    {
        public int ExercisePlanId { get; set; }
        public string Name { get; set; } = null!;
        public double TotalCaloriesBurned { get; set; }
        public string? ExercisePlanImage { get; set; }
        public bool? Status { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? ChangeBy { get; set; }
        public DateTime? ChangeDate { get; set; }
    }

}
