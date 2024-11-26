﻿
using BusinessObject.Dto.Register;
using BusinessObject.Dto.ResetPassword;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IUserRepository
    {
        bool IsUniqueEmail(string email);
        bool IsUniqueUser(string username);
        bool IsUniquePhonenumber(string number);

        //Task<Member> Register(Member registerationRequestDTO);

        Task<Member> Register(Member registerationRequestDTO, RegisterationMobileRequestDTO member);

        Task<Member> Login(Member loginRequestDTO, string password);
        Task<Member> GetMemberByIdAsync(int userId);
        Task UpdateMemberProfileAsync(Member user);
        Task<bool> ResetPasswordAsync(ChangePasswordRequestDTO request,int memberId);
        Task<bool> ResetPasswordOtpAsync(ChangePasswordRequestDTO request, int memberId);
        Task<BusinessObject.Models.Member> DeleteAccount(Member model, string password);
    }
}
