/*using BusinessObject.Dto.ResetPassword;
using BusinessObject.Models;
using HealthTrackingManageAPI;
using HealthTrackingManageAPI.Controllers;
using HealthTrackingManageAPI.NewFolder.Image;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HTUnitTests.Controller
{
    public class ResetPasswordInUserController
    {
       
        private readonly UsersController _controller;
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IOptionsMonitor<AppSettingsKey>> _mockOptionsMonitor;
        private readonly Mock<ILogger<CloudinaryService>> _mockLogger;
        private readonly Mock<HealthTrackingDBContext> _mockContext;

        public ResetPasswordInUserController()
        {
            _mockUserRepo = new Mock<IUserRepository>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockOptionsMonitor = new Mock<IOptionsMonitor<AppSettingsKey>>();
            _mockLogger = new Mock<ILogger<CloudinaryService>>();
            _mockContext = new Mock<HealthTrackingDBContext>();

            // Setup mock configuration
            _mockConfiguration.Setup(x => x["Cloudinary:CloudName"]).Returns("test_cloud_name");
            _mockOptionsMonitor.Setup(x => x.CurrentValue).Returns(new AppSettingsKey
            {
                Secretkey = "TestSecretKeyForJWTTokenGeneration123456"
            });
        }

        // Helper method to setup user claims
        private void SetupUserClaims(string memberId)
        {
            var claims = new[]
            {
            new Claim("Id", memberId)
        };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };
        }

        // UTCID01: Successful password reset
        [Fact]
        public async Task ResetPassword_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            SetupUserClaims("123");
            var request = new ChangePasswordRequestDTO
            {
                PhoneNumber = "123456789",
                NewPassword = "NewPassword123",
                //ConfirmPassword = "NewPassword123"
            };

            _mockUserRepo.Setup(r => r.ResetPasswordAsync(request, 123))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.ResetPassword(request);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _mockUserRepo.Verify(r => r.ResetPasswordAsync(request, 123), Times.Once);
        }

        // UTCID02: Member ID not found in claims
        [Fact]
        public async Task ResetPassword_MemberIdClaimMissing_ReturnsUnauthorized()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };

            var request = new ChangePasswordRequestDTO
            {
                PhoneNumber = "123456789",
                NewPassword = "NewPassword123",
               // ConfirmPassword = "NewPassword123"
            };

            // Act
            var result = await _controller.ResetPassword(request);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Member ID not found in claims.", unauthorizedResult.Value);
        }

        // UTCID03: Invalid Member ID (non-numeric)
        [Fact]
        public async Task ResetPassword_InvalidMemberId_ReturnsBadRequest()
        {
            // Arrange
            var claims = new[]
            {
            new Claim("Id", "ABC")
        };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var request = new ChangePasswordRequestDTO
            {
                PhoneNumber = "123456789",
                NewPassword = "NewPassword123",
               // ConfirmPassword = "NewPassword123"
            };

            // Act
            var result = await _controller.ResetPassword(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid member ID.", badRequestResult.Value);
        }

        // UTCID04: Password reset fails
        [Fact]
        public async Task ResetPassword_ResetFails_ReturnsBadRequest()
        {
            // Arrange
            SetupUserClaims("123");
            var request = new ChangePasswordRequestDTO
            {
                PhoneNumber = "123456789",
                NewPassword = "NewPassword123",
                //ConfirmPassword = "NewPassword123"
            };

            _mockUserRepo.Setup(r => r.ResetPasswordAsync(request, 123))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.ResetPassword(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error while registering the user", badRequestResult.Value);
        }

        // UTCID05: Null or Empty Phone Number
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task ResetPassword_InvalidPhoneNumber_ReturnsBadRequest(string phoneNumber)
        {
            // Arrange
            SetupUserClaims("123");
            var request = new ChangePasswordRequestDTO
            {
                PhoneNumber = phoneNumber,
                NewPassword = "NewPassword123",
              //  ConfirmPassword = "NewPassword123"
            };

            // Act
            var result = await _controller.ResetPassword(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        // UTCID06: Null or Empty New Password
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task ResetPassword_InvalidNewPassword_ReturnsBadRequest(string newPassword)
        {
            // Arrange
            SetupUserClaims("123");
            var request = new ChangePasswordRequestDTO
            {
                PhoneNumber = "123456789",
                NewPassword = newPassword,
                //ConfirmPassword = newPassword
            };

            // Act
            var result = await _controller.ResetPassword(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
*/