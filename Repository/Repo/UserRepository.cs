
using AutoMapper.Execution;
using BusinessObject.Dto.Register;
using BusinessObject.Dto.ResetPassword;
using BusinessObject.Models;
using DataAccess;

using Microsoft.EntityFrameworkCore;
using Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Http;

namespace Repository.Repo
{
    public class UserRepository : IUserRepository
    {
        public Task<BusinessObject.Models.Member> GetMemberByIdAsync(int userId) => UserDAO.Instance.GetMemberByIdAsync(userId);

       
        public Task UpdateMemberProfileAsync(BusinessObject.Models.Member member, double weight) => UserDAO.Instance.UpdateMemberProfileAsync(member, weight);


        public bool IsUniqueEmail(string email) => UserDAO.Instance.IsUniqueEmail(email);

        public bool IsUniquePhonenumber(string number) => UserDAO.Instance.IsUniquePhonenumber(number);


        public bool IsUniqueUser(string username) => UserDAO.Instance.IsUniqueUser(username);



        public Task<BusinessObject.Models.Member> Login(BusinessObject.Models.Member loginRequestDTO,string password) => UserDAO.Instance.Login(loginRequestDTO,password);

      



        public Task<BusinessObject.Models.Member> Register(BusinessObject.Models.Member registerationRequestDTO, RegisterationMobileRequestDTO member) => UserDAO.Instance.Register(registerationRequestDTO, member);

        public Task<bool> ResetPasswordAsync(ChangePasswordRequestForAccountDTO request,int memberId) => UserDAO.Instance.ResetPasswordAsync(request,memberId);

        public Task<bool> ResetPasswordOtpAsync(ChangePasswordRequestDTO request) => UserDAO.Instance.ResetPasswordOtpAsync(request);

        public Task<BusinessObject.Models.Member> DeleteAccount(BusinessObject.Models.Member model, string password) => UserDAO.Instance.DeleteAccount(model, password);

        public Task<bool> UploadImageForMember(string urlImage, int memberId) => UserDAO.Instance.UploadImageForMember(urlImage, memberId);
        
    }
}
