﻿using HealthTrackingManageAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Repository.Model.Dto;
using System.Net;

namespace HealthTrackingManageAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUserRepository _userRepo;

		public UsersController(IUserRepository userRepo)
		{
			_userRepo = userRepo;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterationRequestDTO model)
		{
			bool ifUserNameUnique = _userRepo.IsUniqueUser(model.UserName);
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
		public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
		{
			var user = await _userRepo.Login(model);
			if (user == null)
			{
				return Unauthorized("Invalid username or password");
			}

			return Ok(user); 
		}
	}
}
