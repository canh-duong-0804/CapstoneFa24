using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Login
{
    public class LoginRequestDTO
    {
      
        public string Email { get; set; }
        
        public string Password { get; set; }
    }
}
