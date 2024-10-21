
using AutoMapper.Execution;
using BusinessObject.Models;
using DataAccess;
using HealthTrackingManageAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Repository.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
		/*private readonly HealthTrackingDBContext _context;
		public UserRepository(HealthTrackingDBContext context)
		{
			_context = context;
		}

		public bool IsUniqueEmail(string email)
		{
			var user = _context.Members.FirstOrDefault(x => x.Email == email);
			if (user == null)
			{
				return true;
			}
			return false;
		}
		public bool IsUniqueUser(string username)
		{
			var user = _context.Members.FirstOrDefault(x => x.Username == username);
			if (user == null)
			{
				return true;
			}
			return false;
		}

		public bool IsUniquePhonenumber(string number)
		{
			var user = _context.Members.FirstOrDefault(x => x.PhoneNumber == number);
			if (user == null)
			{
				return true;
			}
			return false;
		}

		public async Task<Member> Register(RegisterationRequestDTO registerationRequestDTO)
		{




			var newMember = new Member
			{
				Username = registerationRequestDTO.UserName,
				Email = registerationRequestDTO.Email,
				Password = registerationRequestDTO.Password,
				Dob = registerationRequestDTO.Dob,
				PhoneNumber = registerationRequestDTO.PhoneNumber,
				Height = registerationRequestDTO.Height,
				Weight = registerationRequestDTO.Weight,
				Gender = registerationRequestDTO.Gender,
				ExerciseLevel = registerationRequestDTO.ExecriseLevel,
				Goal = registerationRequestDTO.Goal,
				UnderlyingDisease = registerationRequestDTO.UnderLyingDisease,
				CreatedAt = DateTime.Now,
				Status = true
			};


			_context.Members.Add(newMember);
			await _context.SaveChangesAsync();

			return newMember;
		}

		public async Task<Member> Login(LoginRequestDTO loginRequestDTO)
		{
			var user = await _context.Members.FirstOrDefaultAsync(x =>
				x.Username == loginRequestDTO.UserName && x.Password == loginRequestDTO.Password);

			return user;
		}*/
		public bool IsUniqueEmail(string email) => UserDAO.Instance.IsUniqueEmail(email);

        public bool IsUniquePhonenumber(string number) => UserDAO.Instance.IsUniquePhonenumber(number);
       

        public bool IsUniqueUser(string username) => UserDAO.Instance.IsUniqueUser(username);


        public Task<BusinessObject.Models.Member> Login(BusinessObject.Models.Member loginRequestDTO) => UserDAO.Instance.Login(loginRequestDTO);
        

        public Task<BusinessObject.Models.Member> Register(BusinessObject.Models.Member registerationRequestDTO) => UserDAO.Instance.Register(registerationRequestDTO);

    }
}
