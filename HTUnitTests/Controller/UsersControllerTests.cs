using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using HealthTrackingManageAPI.Controllers;
using BusinessObject.Models;
using BusinessObject.Dto.Login;
using BusinessObject.Dto.Member;
using Repository.IRepo;
using BusinessObject.Dto.Register;
using BusinessObject.Dto.ResetPassword;
using HealthTrackingManageAPI.NewFolder.Image;
using BusinessObject.Dto.Message;
using HealthTrackingManageAPI;

namespace HTUnitTests.Controller
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IOptionsMonitor<AppSettingsKey>> _mockOptionsMonitor;
        private readonly Mock<ILogger<CloudinaryService>> _mockLogger;
        private readonly Mock<HealthTrackingDBContext> _mockContext;

        public UsersControllerTests()
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

        private UsersController CreateController(Member testMember = null)
        {
            testMember ??= new Member
            {
                MemberId = 1,
                Username = "testuser",
                Email = "test@example.com",
                PhoneNumber = "1234567890"
            };

            return new UsersController(
                _mockContext.Object,
                _mockConfiguration.Object,
                _mockUserRepo.Object,
                _mockOptionsMonitor.Object,
                new CloudinaryService(_mockConfiguration.Object, _mockLogger.Object)
            );
        }

        private void SetupUserContextWithId(UsersController controller, string userId)
        {
            var claims = new List<Claim>
            {
                new Claim("Id", userId)
            };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };
        }

        #region Login Tests
       /* [Fact]
        public async Task Login_ValidCredentials_ReturnsSuccessfulResponse()
        {
            // Arrange
            var loginRequest = new LoginRequestDTO
            {
                PhoneNumber = "1234567890",
                Password = "password123"
            };

            var expectedUser = new Member
            {
                MemberId = 1,
                Username = "testuser",
                PhoneNumber = "1234567890"
            };

            _mockUserRepo.Setup(repo => repo.Login(It.IsAny<Member>(), It.IsAny<string>()))
                .ReturnsAsync(expectedUser);

            var controller = CreateController();

            // Act
            var result = await controller.Login(loginRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<MessageResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Login successful", response.Message);
        }*/

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var loginDto = new LoginRequestDTO
            {
                PhoneNumber = "1234567890",
                Password = "invalidpassword"
            };

            _mockUserRepo.Setup(repo => repo.Login(It.IsAny<Member>(), loginDto.Password))
                .ReturnsAsync((Member)null);

            var controller = CreateController();

            // Act
            var result = await controller.Login(loginDto);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid username or password", unauthorizedResult.Value);
        }
        #endregion

        #region Registration Tests
        [Fact]
        public async Task RegisterMobile_ValidRequest_ReturnsOk()
        {
            // Arrange
            var registerDto = new RegisterationMobileRequestDTO
            {
                UserName = "newuser",
                Email = "newuser@example.com",
                PhoneNumber = "9876543210"
            };

            var member = new Member
            {
                Username = registerDto.UserName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber
            };

            _mockUserRepo.Setup(repo => repo.Register(It.IsAny<Member>(), registerDto))
                .ReturnsAsync(member);

            var controller = CreateController();

            // Act
            var result = await controller.RegisterMobile(registerDto);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task RegisterMobile_RegistrationFails_ReturnsBadRequest()
        {
            // Arrange
            var registerDto = new RegisterationMobileRequestDTO
            {
                UserName = "newuser",
                Email = "newuser@example.com",
                PhoneNumber = "9876543210"
            };

            _mockUserRepo.Setup(repo => repo.Register(It.IsAny<Member>(), registerDto))
                .ReturnsAsync((Member)null);

            var controller = CreateController();

            // Act
            var result = await controller.RegisterMobile(registerDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error while registering the user", badRequestResult.Value);
        }
        #endregion

        #region Profile Tests
        [Fact]
        public async Task ViewMemberProfile_AuthenticatedUser_ReturnsProfile()
        {
            // Arrange
            var member = new Member
            {
                MemberId = 1,
                Username = "testuser",
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                BodyMeasureChanges = new List<BodyMeasureChange>
                {
                    new BodyMeasureChange { Weight = 70.5 }
                }
            };

            _mockUserRepo.Setup(repo => repo.GetMemberByIdAsync(1))
                .ReturnsAsync(member);

            var controller = CreateController(member);
            SetupUserContextWithId(controller, "1");

            // Act
            var result = await controller.ViewMemberProfile();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var profileDto = Assert.IsType<MemberProfileDto>(okResult.Value);
            Assert.Equal(member.Username, profileDto.Username);
            Assert.Equal(70.5, profileDto.Weight);
        }

        [Fact]
        public async Task EditMemberProfile_ValidRequest_ReturnsOk()
        {
            // Arrange
            var updatedProfile = new MemberProfileDto
            {
                Username = "updateduser",
                Email = "updated@example.com",
                Weight = 75.0
            };

            var existingMember = new Member
            {
                MemberId = 1,
                Username = "olduser",
                Email = "old@example.com"
            };

            // Setup mock repository methods
            _mockUserRepo.Setup(repo => repo.GetMemberByIdAsync(1))
                .ReturnsAsync(existingMember);

            _mockUserRepo.Setup(repo => repo.UpdateMemberProfileAsync(It.IsAny<Member>(), updatedProfile.Weight))
                .Returns(Task.CompletedTask);

            // Create controller and set up user context
            var controller = CreateController(existingMember);
            SetupUserContextWithId(controller, "1");

            // Act
            var result = await controller.EditMemberProfile(updatedProfile);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            var responseObj = okResult.Value;
            var messageProperty = responseObj.GetType().GetProperty("message");
            var messageValue = messageProperty.GetValue(responseObj)?.ToString();
            Assert.Equal("Profile updated successfully", messageValue);
        }
        #endregion

        #region Image Upload Tests
        /*[Fact]
        public async Task UploadImageAvatarMember_ValidImage_ReturnsOk()
        {
            // Arrange
            var memberId = 1;
            var mockFile = CreateMockIFormFile();

            var uploadResult = new ImageModel
            {
                Url = "https://example.com/image.jpg"
            };

            _mockUserRepo
                .Setup(repo => repo.UploadImageForMember(uploadResult.Url.ToString(), memberId))
                .ReturnsAsync(true);

            var mockCloudinaryService = new Mock<CloudinaryService>(_mockConfiguration.Object, _mockLogger.Object)
            {
                CallBase = true
            };

            mockCloudinaryService
                .Setup(x => x.UploadImageAsync(It.IsAny<IFormFile>(), "user_uploads"))
                .ReturnsAsync(uploadResult);

            var controller = CreateController();
            SetupUserContextWithId(controller, memberId.ToString());

            // Act
            var result = await controller.UploadImageAvatarMember(mockFile);

            // Assert
            Assert.IsType<OkResult>(result);
        }
*/
        [Fact]
        public async Task UploadImageAvatarMember_InvalidImage_ReturnsBadRequest()
        {
            // Arrange
            var memberId = 1;
            var controller = CreateController();
            SetupUserContextWithId(controller, memberId.ToString());

            // Act
            var result = await controller.UploadImageAvatarMember(null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
        #endregion

        private IFormFile CreateMockIFormFile()
        {
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1000);
            fileMock.Setup(f => f.FileName).Returns("test.jpg");
            fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(new byte[1000]));
            return fileMock.Object;
        }
    }
}
