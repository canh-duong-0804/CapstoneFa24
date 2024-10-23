using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IStaffRepository
    {
        bool IsUniqueEmail(string email);
       // bool IsUniqueUser(string username);
        bool IsUniquePhonenumber(string number);
        Task<staff> Register(staff registerationRequestDTO);
        Task<staff> Login(staff loginRequestDTO);
    }
}
