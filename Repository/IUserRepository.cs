using BusinessObject.Entities;
using HealthTrackingManageAPI.Models.Dto;
using Repository.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IUserRepository
    {
		bool IsUniqueEmail(string email);
		bool IsUniqueUser(string username);
		bool IsUniquePhonenumber(string number);
		Task<Member> Register(RegisterationRequestDTO registerationRequestDTO);
		Task<Member> Login(LoginRequestDTO loginRequestDTO);

	}
}
