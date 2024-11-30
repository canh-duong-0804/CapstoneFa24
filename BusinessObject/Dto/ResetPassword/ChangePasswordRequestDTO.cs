using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.ResetPassword
{
    public class ChangePasswordRequestDTO
    {

        public string PhoneNumber { get; set; } 
        public string NewPassword { get; set; } 
    }
    public class ChangePasswordRequestForAccountDTO
    {

        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
