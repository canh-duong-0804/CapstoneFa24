﻿using AutoMapper.Execution;
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
        public Task DeleteAccountStaffByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<GetStaffByIdResponseDTO> GetAccountStaffByIdAsync(int id) => StaffDAO.Instance.GetAccountStaffByIdAsync(id);
      

        public Task<IEnumerable<AllStaffsResponseDTO>> GetAllAccountStaffsAsync() => StaffDAO.Instance.GetAllAccountStaffsAsync();
         

        public bool IsUniqueEmail(string email) => StaffDAO.Instance.IsUniqueEmail(email);



        public bool IsUniquePhonenumber(string number) => StaffDAO.Instance.IsUniquePhonenumber(number);



        public Task<staff> Login(staff loginRequestStaffDTO) => StaffDAO.Instance.LoginStaff(loginRequestStaffDTO);
        public Task<staff> RegisterAccountStaff(staff registerationRequesStafftDTO) => StaffDAO.Instance.RegisterStaff(registerationRequesStafftDTO);

        public Task<UpdateRoleStaffRequestDTO> UpdateRoleAccountStaffByIdAsync(UpdateRoleStaffRequestDTO staffRole) => StaffDAO.Instance.UpdateRoleAccountStaffByIdAsync(staffRole);

    }
}