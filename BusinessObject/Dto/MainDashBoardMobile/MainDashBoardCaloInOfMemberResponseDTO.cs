using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.MainDashBoardMobile
{
    public class MainDashBoardCaloInOfMemberResponseDTO
    {
        public double? Calories { get; set; }
        public double AmountWater { get; set; }
        public int streakNumberExercise { get; set; }
        public int streakNumberFood { get; set; }
        
        public double? Protein { get; set; }
        public double? Fat { get; set; }
        public double? Carbs { get; set; }
    }
}
