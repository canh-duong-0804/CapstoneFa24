using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Register
{
    public class RegisterationResponseDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
    
        public DateTime Dob { get; set; }
        public string PhoneNumber { get; set; }

        public bool Gender { get; set; }
    }
}
