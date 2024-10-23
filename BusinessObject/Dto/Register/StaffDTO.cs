using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Register
{
    public class StaffDTO
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public bool Sex { get; set; }
        public string Description { get; set; } = null!;
        public DateTime Dob { get; set; }
        public string StaffImage { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public byte Role { get; set; }
    
    }
}
