using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Register
{
    public class RegisterationRequestStaffDTO
    {

        public string FullName { get; set; }
        public string PhoneNumber { get; set; } 
        public bool Sex { get; set; }
        public string Description { get; set; }
        public DateTime Dob { get; set; }
        public string StaffImage { get; set; } 
        public string Email { get; set; }
        public string Password { get; set; }
        public byte Role { get; set; }


    }
}
