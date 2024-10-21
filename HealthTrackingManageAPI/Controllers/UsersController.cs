using AutoMapper.Execution;
using BusinessObject.Dto;
using BusinessObject.Models;
using HealthTrackingManageAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repository;
using Repository.Model.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace HealthTrackingManageAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUserRepository _userRepo;
		private readonly IConfiguration _configuration;
		private readonly AppSettingsKey _appSettings;
		public UsersController(IConfiguration configuration, IUserRepository userRepo, IOptionsMonitor<AppSettingsKey> optionsMonitor)
		{
			_configuration = configuration;
			_userRepo = userRepo;
			_appSettings = optionsMonitor.CurrentValue;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterationRequestDTO member)
		{
			var mapper = MapperConfig.InitializeAutomapper();

			var model = mapper.Map<BusinessObject.Models.Member>(member);

			bool ifUserNameUnique = _userRepo.IsUniqueUser(model.Username);
			if (!ifUserNameUnique)
			{
				return BadRequest("Username already exists");
			}


			bool ifEmailUnique = _userRepo.IsUniqueEmail(model.Email);
			if (!ifEmailUnique)
			{
				return BadRequest("Email already exists");
			}


			bool ifPhoneUnique = _userRepo.IsUniquePhonenumber(model.PhoneNumber);
			if (!ifPhoneUnique)
			{
				return BadRequest("Phone number already exists");
			}


			var user = await _userRepo.Register(model);
			if (user == null)
			{
				return BadRequest("Error while registering the user");
			}


			return Ok(user);
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDTO member)
		{
			var mapper = MapperConfig.InitializeAutomapper();

			var model = mapper.Map<BusinessObject.Models.Member>(member);
			var user = await _userRepo.Login(model);
			if (user == null)
			{
				return Unauthorized("Invalid username or password");
			}

			// Generate JWT token
			var token = GenerateToken(user);

			var response = new LoginResponseDTO
			{
				Member = user,
				Token = token
			};

			return Ok(response);
		}
		
		private string GenerateToken(BusinessObject.Models.Member user)
		{
			var jwtTokenHandler = new JwtSecurityTokenHandler();
			var secretKetBytes = Encoding.UTF8.GetBytes(_appSettings.Secretkey);
			var tokenDescription = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{

			new Claim("Id", user.MemberId.ToString()),
			new Claim(ClaimTypes.Name, user.Username),
			new Claim(ClaimTypes.Email, user.Email),

            //roles
            new Claim("TokenId",Guid.NewGuid().ToString())


		}),
				Expires = DateTime.UtcNow.AddMinutes(1),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKetBytes),
				SecurityAlgorithms.HmacSha256Signature)

			};
			var token = jwtTokenHandler.CreateToken(tokenDescription);

			return jwtTokenHandler.WriteToken(token);
		}
	}
}
