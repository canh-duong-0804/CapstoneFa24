using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IStaffRepository
    {
        bool IsUniqueEmail(string email);
        bool IsUniquePhonenumber(string number);
        Task<staff> Register(staff registerationRequestDTO);
        Task<staff> Login(staff loginRequestDTO);
    }
}
