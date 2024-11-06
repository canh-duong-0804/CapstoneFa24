
using AutoMapper.Execution;
using BusinessObject.Models;
using DataAccess;

using Microsoft.EntityFrameworkCore;
using Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repo
{
    public class UserRepository : IUserRepository
    {
        public Task<BusinessObject.Models.Member> GetMemberByIdAsync(int userId) => UserDAO.Instance.GetMemberByIdAsync(userId);

        // Method to update a member's profile
        public Task UpdateMemberProfileAsync(BusinessObject.Models.Member member) => UserDAO.Instance.UpdateMemberProfileAsync(member);


        public bool IsUniqueEmail(string email) => UserDAO.Instance.IsUniqueEmail(email);

        public bool IsUniquePhonenumber(string number) => UserDAO.Instance.IsUniquePhonenumber(number);


        public bool IsUniqueUser(string username) => UserDAO.Instance.IsUniqueUser(username);


<<<<<<< Updated upstream
        public Task<BusinessObject.Models.Member> Login(BusinessObject.Models.Member loginRequestDTO,string password) => UserDAO.Instance.Login(loginRequestDTO,password);
=======
        public Task<BusinessObject.Models.Member> Login(BusinessObject.Models.Member loginRequestDTO, string password) => UserDAO.Instance.Login(loginRequestDTO,  password);
>>>>>>> Stashed changes


        public Task<BusinessObject.Models.Member> Register(BusinessObject.Models.Member registerationRequestDTO, string password) => UserDAO.Instance.Register(registerationRequestDTO, password);

        
    }
}
