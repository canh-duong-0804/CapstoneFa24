using AutoMapper.Execution;
using BusinessObject;
using BusinessObject.Dto;
using BusinessObject.Dto.Login;
using BusinessObject.Dto.Member;
using BusinessObject.Dto.Message;
using BusinessObject.Dto.Register;
using BusinessObject.Dto.ResetPassword;
using BusinessObject.Models;
using HealthTrackingManageAPI.NewFolder;
using HealthTrackingManageAPI.NewFolder.Image;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Repository.IRepo;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
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
        private readonly HealthTrackingDBContext _context;
        private readonly CloudinaryService _cloudinaryService;
        public UsersController(HealthTrackingDBContext context, IConfiguration configuration, IUserRepository userRepo, IOptionsMonitor<AppSettingsKey> optionsMonitor, CloudinaryService cloudinaryService)
        {
            _configuration = configuration;
            _userRepo = userRepo;
            _context = context;
            _appSettings = optionsMonitor.CurrentValue;
            _cloudinaryService = cloudinaryService; 
        }

       /* [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequestDTO member)
        {
            var mapper = MapperConfig.InitializeAutomapper();

            var model = mapper.Map<BusinessObject.Models.Member>(member);

           *//* bool ifUserNameUnique = _userRepo.IsUniqueUser(model.Username);
            if (!ifUserNameUnique)
            {
                return BadRequest("Username already exists");
            }*//*


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


            var user = await _userRepo.Register(model,member.Password);
            if (user == null)
            {
                return BadRequest("Error while registering the user");
            }

            
            return Ok();
        }*/


        [HttpPost("register-mobile")]
        public async Task<IActionResult> RegisterMobile([FromBody] RegisterationMobileRequestDTO member)
        {
            var mapper = MapperConfig.InitializeAutomapper();

            var model = mapper.Map<BusinessObject.Models.Member>(member);
            var user = await _userRepo.Register(model,member);
            if (user == null)
            {
                return BadRequest("Error while registering the user");
            }

    
            return Ok();
        }
        [HttpGet("check-email")]
        public async Task<IActionResult> CheckEmailUnique([FromQuery] string email)
        {
            
                bool ifEmailUnique =  _userRepo.IsUniqueEmail(email);
                if (!ifEmailUnique)
                {
                    return BadRequest("Email already exists");
                }

                return Ok();
            
        }

        [HttpGet("check-phone")]
        public async Task<IActionResult> CheckPhoneUnique([FromQuery] string phoneNumber)
        {
            
                bool ifPhoneUnique =  _userRepo.IsUniquePhonenumber(phoneNumber);
                if (!ifPhoneUnique)
                {
                    return BadRequest("Phone number already exists");
                }

                return Ok();
            
        }


        [HttpPut("reset-password")]
        [Authorize]
        public async Task<IActionResult> ResetPassword([FromBody] ChangePasswordRequestDTO request)
        {
            var memberIdClaim = User.FindFirstValue("Id");
            if (memberIdClaim == null)
            {
                return Unauthorized("Member ID not found in claims.");
            }

            if (!int.TryParse(memberIdClaim, out int memberId))
            {
                return BadRequest("Invalid member ID.");
            }

            var user = await _userRepo.ResetPasswordAsync(request,memberId);
            if (user == null)
            {
                return BadRequest("Error while registering the user");
            }
            return Ok();
        }
        
        [HttpPut("reset-password-otp")]
        public async Task<IActionResult> ResetPasswordOtp([FromBody] ChangePasswordRequestDTO request)
        {
            var memberIdClaim = User.FindFirstValue("Id");
            

            var user = await _userRepo.ResetPasswordOtpAsync(request);
            if (user == null)
            {
                return BadRequest("Error while registering the user");
            }

    
            return Ok();
        }






        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO member)
        {
            var mapper = MapperConfig.InitializeAutomapper();

            var model = mapper.Map<BusinessObject.Models.Member>(member);
            model.PhoneNumber = member.Email;
            var user = await _userRepo.Login(model, member.Password);

            if (user == null)
            {
                return Unauthorized("Invalid username or password");
            }

            // Generate JWT token
            var token = await GenerateToken(user);
            var memberDTO = mapper.Map<BusinessObject.Dto.Login.LoginResponseMemberDTO>(user);
            var response = new MessageResponse
            {
                Success = true,
                Message = "Authenticate success",
                Data = token,
              Member = memberDTO
            };

            return Ok(response);
        }
        
        
        [HttpPost("Delete-Account")]
        public async Task<IActionResult> DeleteAccount([FromBody] LoginRequestDTO member)
        {
            var mapper = MapperConfig.InitializeAutomapper();

            var model = mapper.Map<BusinessObject.Models.Member>(member);
            model.PhoneNumber = member.Email;
            var user = await _userRepo.DeleteAccount(model, member.Password);

            if (user == null)
            {
                return Unauthorized("Invalid username or password");
            }

            

            return Ok();
        }

        private async Task<TokenModel> GenerateToken(BusinessObject.Models.Member user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKetBytes = Encoding.UTF8.GetBytes(_appSettings.Secretkey);
          
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {

                new Claim("Id", user.MemberId.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserName",user.Username),
                  
           
            //roles
            //new Claim("TokenId",Guid.NewGuid().ToString())


        }),

                //time access
                //Expires = DateTime.UtcNow.AddSeconds(20),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKetBytes),
                SecurityAlgorithms.HmacSha256Signature)

            };
            var token = jwtTokenHandler.CreateToken(tokenDescription);

            var accessToken = jwtTokenHandler.WriteToken(token);
            var RefreshToken = GenerateRefreshToken();


            var refreshTokenEntity = new RefreshTokensMember
            {
                //Id = Guid.NewGuid(),
                JwtId = token.Id,
                MemberId = user.MemberId,
                Token = RefreshToken,
                IsUsed = false,
                IsRevoked = false,
                IssuedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddHours(1)
            };

            await _context.RefreshTokensMembers.AddAsync(refreshTokenEntity);
            await _context.SaveChangesAsync();

            return new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = RefreshToken

            };
        }

        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }

        }

        [HttpPost("RenewToken")]
        public async Task<IActionResult> RenewToken(TokenModel model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.Secretkey);
            var tokenValidateParam = new TokenValidationParameters
            {
               
                ValidateIssuer = false,
                ValidateAudience = false,

                
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                ClockSkew = TimeSpan.Zero,

                ValidateLifetime = false 
            };
            try
            {
                //check 1: AccessToken valid format
                var tokenInVerification = jwtTokenHandler.ValidateToken(model.AccessToken, tokenValidateParam, out var validatedToken);

                //check 2: Check alg
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                    if (!result)//false
                    {
                        return Ok(new MessageResponse
                        {
                            Success = false,
                            Message = "Invalid token"
                        });
                    }
                }

                //check 3: Check accessToken expire?
                var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
                if (expireDate > DateTime.UtcNow)
                {
                    return Ok(new MessageResponse
                    {
                        Success = false,
                        Message = "Access token has not yet expired"
                    });
                }

                //check 4: Check refreshtoken exist in DB



                var storedToken = _context.RefreshTokensMembers.FirstOrDefault(x => x.Token == model.RefreshToken);


                if (storedToken == null)
                {
                    return Ok(new MessageResponse
                    {
                        Success = false,
                        Message = "Refresh token does not exist"
                    });
                }

                //check 5: check refreshToken is used/revoked?
                if (storedToken.IsUsed)
                {
                    return Ok(new MessageResponse
                    {
                        Success = false,
                        Message = "Refresh token has been used"
                    });
                }
                if (storedToken.IsRevoked)
                {
                    return Ok(new MessageResponse
                    {
                        Success = false,
                        Message = "Refresh token has been revoked"
                    });
                }

                //check 6: AccessToken id == JwtId in RefreshToken
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if (storedToken.JwtId != jti)
                {
                    return Ok(new MessageResponse
                    {
                        Success = false,
                        Message = "Token doesn't match"
                    });
                }

                //Update token is used
                storedToken.IsRevoked = true;
                storedToken.IsUsed = true;
                _context.Update(storedToken);
                await _context.SaveChangesAsync();

                //create new token
                var user = await _context.Members.SingleOrDefaultAsync(nd => nd.MemberId == storedToken.MemberId);
                var token = await GenerateToken(user);

                return Ok(new MessageResponse
                {
                    Success = true,
                    Message = "Renew token success",
                    Data = token
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponse
                {
                    Success = false,
                    Message = "Something went wrong"
                });
            }
        }

        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();

            return dateTimeInterval;
        }
		[Authorize]
		[HttpGet("viewMemberProfile")]
		public async Task<IActionResult> ViewMemberProfile()
		{
			try
			{
				// Since we're using [Authorize], we can assume the user is authenticated
				var userId = User.FindFirstValue("Id");

				if (userId == null)
				{
					return BadRequest("User ID not found in token");
				}

				// Ensure userId is a valid integer
				if (!int.TryParse(userId, out var parsedUserId))
				{
					return BadRequest("Invalid user ID format.");
				}

				// Get user details
				var user = await _userRepo.GetMemberByIdAsync(parsedUserId);
                var bodyMeasureChange = user.BodyMeasureChanges.OrderByDescending(d=>d.BodyMeasureId).FirstOrDefault();
                if (user == null)
				{
					return NotFound("User not found");
				}

				var mapper = MapperConfig.InitializeAutomapper();
				var userProfileDto = mapper.Map<MemberProfileDto>(user);
                userProfileDto.Weight = bodyMeasureChange.Weight ?? 0;

                return Ok(userProfileDto);
			}
			catch (Exception ex)
			{
				// Log the exception (assuming you have a logger)
				
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}


        [Authorize]
        [HttpPut("editMemberProfile")]
        public async Task<IActionResult> EditMemberProfile([FromBody] MemberProfileDto updatedProfile)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid profile data.");
            }

            try
            {
               
                var userId = User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User ID not found in token.");
                }

               
                var user = await _userRepo.GetMemberByIdAsync(int.Parse(userId));
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                var mapper = MapperConfig.InitializeAutomapper();
                mapper.Map(updatedProfile, user);

                
                await _userRepo.UpdateMemberProfileAsync(user,updatedProfile.Weight);

                
                return Ok(new
                {
                    message = "Profile updated successfully",
                    imagePath = updatedProfile.ImageMember
                });
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [Authorize]
        [HttpPut("upload-image-avatar-member")]
        public async Task<IActionResult> UploadImageAvatarMember(IFormFile? imageFile)
        {
            var memberIdClaim = User.FindFirstValue("Id");
            if (memberIdClaim == null)
            {
                return Unauthorized();
            }

            if (!int.TryParse(memberIdClaim, out int memberId))
            {
                return BadRequest();
            }
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadResult = await _cloudinaryService.UploadImageAsync(
                    imageFile,
                    "user_uploads"
                );
                var check = await _userRepo.UploadImageForMember(uploadResult.Url.ToString(), memberId);


                if (check) return Ok();

            }
            return BadRequest();
        }
    }
}