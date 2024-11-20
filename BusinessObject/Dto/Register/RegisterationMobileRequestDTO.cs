using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Register
{
    public class RegisterationMobileRequestDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Dob { get; set; }
        public bool Gender { get; set; }

        public double Height { get; set; }
        public double Weight { get; set; }
        //  

        public double weightPerWeek { get; set; }
        public double? TargetWeight { get; set; }
       // public double? TargetDate { get; set; }
        public int? DietId { get; set; }
        //


        public int ExerciseLevel { get; set; }
        
        public string PhoneNumber { get; set; }

        

    }
}
