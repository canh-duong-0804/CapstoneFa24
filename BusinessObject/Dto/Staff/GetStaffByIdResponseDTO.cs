using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Staff
{
    public class GetStaffByIdResponseDTO
    {
        public int StaffId { get; set; }
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public bool Sex { get; set; }
        public string Description { get; set; } = null!;
        public DateTime Dob { get; set; }
        public string StaffImage { get; set; } = null!;
        public string Email { get; set; } = null!;
       public string Password { get; set; } = null!;
        public byte Role { get; set; }
        public DateTime StartWorkingDate { get; set; }
        public DateTime? EndWorkingDate { get; set; }
        public bool? Status { get; set; }
    }
}
