using AutoMapper.Execution;
using BusinessObject.Dto.Register;
using BusinessObject.Dto.Staff;
using BusinessObject.Models;
using DataAccess;
using Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repo
{
    public class StaffRepository : IStaffRepository
    {
        public Task<bool> DeleteAccountStaffByIdAsync(int id) => StaffDAO.Instance.DeleteAccountStaffByIdAsync(id);

        public Task<GetStaffPersonalByIdResponseDTO> GetAccountPersonalForStaffByIdAsync(int id) => StaffDAO.Instance.GetAccountPersonalForStaffByIdAsync(id);


        public Task<GetStaffByIdResponseDTO> GetAccountStaffForAdminByIdAsync(int id) => StaffDAO.Instance.GetAccountStaffByIdAsync(id);


        //public Task<IEnumerable<AllStaffsResponseDTO>> GetAllAccountStaffsAsync() => StaffDAO.Instance.GetAllAccountStaffsAsync(int page, int pageSize);

        public Task<IEnumerable<AllStaffsResponseDTO>> GetAllAccountStaffsAsync(int page, int pageSize) =>
          StaffDAO.Instance.GetAllAccountStaffsAsync(page, pageSize);


        public Task<int> GetTotalStaffCountAsync() => StaffDAO.Instance.GetTotalStaffCountAsync();


        public bool IsUniqueEmail(string email) => StaffDAO.Instance.IsUniqueEmail(email);



        public bool IsUniquePhonenumber(string number) => StaffDAO.Instance.IsUniquePhonenumber(number);



        public Task<staff> Login(staff loginRequestStaffDTO, string password) => StaffDAO.Instance.LoginStaff(loginRequestStaffDTO,password);
        public Task<staff> RegisterAccountStaff(staff registerationRequesStafftDTO, string password) => StaffDAO.Instance.RegisterStaff(registerationRequesStafftDTO,password);

        public Task<UpdateInfoAccountStaffByIdDTO> UpdateAccountStaffById(UpdateInfoAccountStaffByIdDTO staffRole) => StaffDAO.Instance.UpdateAccountStaffById(staffRole);


        public Task<UpdateRoleStaffRequestDTO> UpdateRoleAccountStaffByIdAsync(UpdateRoleStaffRequestDTO staffRole) => StaffDAO.Instance.UpdateRoleAccountStaffByIdAsync(staffRole);

    }
}
