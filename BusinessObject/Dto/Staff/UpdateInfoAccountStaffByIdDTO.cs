using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Staff
{
    public class UpdateInfoAccountStaffByIdDTO
    {
        public int StaffId { get; set; }
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public bool Sex { get; set; }
       
        public DateTime Dob { get; set; }
        public string StaffImage { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
