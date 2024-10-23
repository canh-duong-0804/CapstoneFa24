using BusinessObject.Dto.Login;
using BusinessObject.Dto.Message;
using BusinessObject.Dto.Register;
using BusinessObject.Dto;
using BusinessObject.Models;
using HealthTrackingManageAPI.NewFolder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Repository.IRepo;
using BusinessObject;

namespace HealthTrackingManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffRepository _staffRepo;
        private readonly IConfiguration _configuration;
        private readonly AppSettingsKey _appSettings;
        private readonly HealthTrackingDBContext _context;
        public StaffController(HealthTrackingDBContext context, IConfiguration configuration, IStaffRepository staffRepo, IOptionsMonitor<AppSettingsKey> optionsMonitor)
        {
            _configuration = configuration;
            _staffRepo = staffRepo;
            _context = context;
            _appSettings = optionsMonitor.CurrentValue;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequestStaffDTO staff)
        {
            var mapper = MapperConfig.InitializeAutomapper();

            var model = mapper.Map<BusinessObject.Models.staff>(staff);

           /* bool ifUserNameUnique = _staffRepo.IsUniqueUser(model.Username);
            if (!ifUserNameUnique)
            {
                return BadRequest("Username already exists");
            }
*/

            bool ifEmailUnique = _staffRepo.IsUniqueEmail(model.Email);
            if (!ifEmailUnique)
            {
                return BadRequest("Email already exists");
            }


            bool ifPhoneUnique = _staffRepo.IsUniquePhonenumber(model.PhoneNumber);
            if (!ifPhoneUnique)
            {
                return BadRequest("Phone number already exists");
            }


            var user = await _staffRepo.Register(model);
            if (user == null)
            {
                return BadRequest("Error while registering the user");
            }


            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestStaffDTO staffRequest)
        {
            var mapper = MapperConfig.InitializeAutomapper();

            var model = mapper.Map<BusinessObject.Models.staff>(staffRequest);
            var staff = await _staffRepo.Login(model);
            if (staff == null)
            {
                return Unauthorized("Invalid username or password");
            }

            // Generate JWT token
            var token = await GenerateToken(staff);
            var staffDTO = mapper.Map<BusinessObject.Dto.Register.StaffDTO>(staff);
            var response = new MessageResponse
            {
                Success = true,
                Message = "Authenticate success",
                Data = token,
                ObjectResponse = staffDTO
            };

            return Ok(response);
        }

        private async Task<TokenModel> GenerateToken(BusinessObject.Models.staff staff)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKetBytes = Encoding.UTF8.GetBytes(_appSettings.Secretkey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {

                new Claim("Id", staff.StaffId.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, staff.FullName),
                new Claim(JwtRegisteredClaimNames.Email, staff.Email),
                new Claim(JwtRegisteredClaimNames.Sub, staff.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("FullName",staff.FullName),
                  
           
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


            var refreshTokenEntity = new RefreshTokensStaff
            {
                //Id = Guid.NewGuid(),
                JwtId = token.Id,
                StaffId = staff.StaffId,
                Token = RefreshToken,
                IsUsed = false,
                IsRevoked = false,
                IssuedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddHours(1)
            };

            await _context.RefreshTokensStaffs.AddAsync(refreshTokenEntity);
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



                var storedToken = _context.RefreshTokensStaffs.FirstOrDefault(x => x.Token == model.RefreshToken);


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
                var user = await _context.staffs.SingleOrDefaultAsync(nd => nd.StaffId == storedToken.StaffId);
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
    }
}