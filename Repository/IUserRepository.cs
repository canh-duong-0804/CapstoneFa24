
using BusinessObject.Models;

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
		Task<Member> Register(Member registerationRequestDTO);
		Task<Member> Login(Member loginRequestDTO);

	}
}
