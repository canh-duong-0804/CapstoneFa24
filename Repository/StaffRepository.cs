using BusinessObject.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class StaffRepository : IStaffRepository
    {
        public bool IsUniqueEmail(string email)
        {
            throw new NotImplementedException();
        }

        public bool IsUniquePhonenumber(string number) => StaffDAO.Instance.IsUniquePhonenumber(number);



        public Task<staff> Login(staff loginRequestStaffDTO) => StaffDAO.Instance.LoginStaff(loginRequestStaffDTO);
        public Task<staff> Register(staff registerationRequesStafftDTO) => StaffDAO.Instance.RegisterStaff(registerationRequesStafftDTO);
    }
}
