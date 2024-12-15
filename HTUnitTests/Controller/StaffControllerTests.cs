using BusinessObject.Dto.Login;
using BusinessObject.Dto.Message;
using BusinessObject.Dto.Register;
using BusinessObject.Dto.Staff;
using BusinessObject.Models;
using HealthTrackingManageAPI.Controllers;
using HealthTrackingManageAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace HTUnitTests.Controller
{
    public class StaffControllerTests
    {
        private readonly Mock<IStaffRepository> _mockStaffRepo;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IOptionsMonitor<AppSettingsKey>> _mockOptionsMonitor;
        private readonly Mock<HealthTrackingDBContext> _mockContext;
        private readonly StaffController _controller;

        public StaffControllerTests()
        {
            _mockStaffRepo = new Mock<IStaffRepository>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockOptionsMonitor = new Mock<IOptionsMonitor<AppSettingsKey>>();
            _mockContext = new Mock<HealthTrackingDBContext>();

            // Setup mock AppSettingsKey
            var appSettings = new AppSettingsKey { Secretkey = "TestSecretKeyForJWTTokenGeneration" };
            _mockOptionsMonitor.Setup(x => x.CurrentValue).Returns(appSettings);

            _controller = new StaffController(
                _mockContext.Object,
                _mockConfiguration.Object,
                _mockStaffRepo.Object,
                _mockOptionsMonitor.Object
            );
        }

        #region Register Tests
        [Fact]
        public async Task Register_UniqueEmailAndPhone_ReturnsOkResult()
        {
            // Arrange
            var registerDto = new RegisterationRequestStaffDTO
            {
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                Password = "password123"
            };

            _mockStaffRepo.Setup(x => x.IsUniqueEmail(It.IsAny<string>())).Returns(true);
            _mockStaffRepo.Setup(x => x.IsUniquePhonenumber(It.IsAny<string>())).Returns(true);

            var staffModel = new staff
            {
                StaffId = 1,
                Email = registerDto.Email
            };
            _mockStaffRepo.Setup(x => x.RegisterAccountStaff(It.IsAny<staff>(), It.IsAny<string>()))
                .ReturnsAsync(staffModel);

            // Act
            var result = await _controller.Register(registerDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseDto = Assert.IsType<RegisterationResponseDTO>(okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task Register_DuplicateEmail_ReturnsBadRequest()
        {
            // Arrange
            var registerDto = new RegisterationRequestStaffDTO
            {
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                Password = "password123"
            };

            _mockStaffRepo.Setup(x => x.IsUniqueEmail(It.IsAny<string>())).Returns(false);

            // Act
            var result = await _controller.Register(registerDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        #endregion

        #region Login Tests
       /* [Fact]
        public async Task Login_ValidCredentials_ReturnsTokenAndStaff()
        {
            // Arrange
            var loginDto = new LoginRequestStaffDTO
            {
                Email = "test@example.com",
                Password = "password123"
            };
            var staffModel = new staff
            {
                StaffId = 1,
                Email = loginDto.Email,
                FullName = "Test Staff",
                Role = 1
            };

            // Mock the token generation
            _mockStaffRepo.Setup(x => x.Login(It.IsAny<staff>(), It.IsAny<string>()))
                .ReturnsAsync(staffModel);

           

            // Act
            var result = await _controller.LoginAdmin(loginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);                          
            Assert.Equal(200, okResult.StatusCode);
            
        }*/

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var loginDto = new LoginRequestStaffDTO
            {
                Email = "test@example.com",
                Password = "wrongpassword"
            };

            _mockStaffRepo.Setup(x => x.Login(It.IsAny<staff>(), It.IsAny<string>()))
                .ReturnsAsync((staff)null);

            // Act
            var result = await _controller.LoginAdmin(loginDto);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid username or password", unauthorizedResult.Value);
        }
        #endregion

        #region Get Staff Tests
        [Fact]
        public async Task GetAllAccountStaffForAdmin_ValidPage_ReturnsStaffList()
        {
            var loginDto = new LoginRequestStaffDTO
            {
                Email = "test@example.com",
                Password = "password123"
            };

            var staffModel = new staff
            {
                StaffId = 1,
                Email = loginDto.Email,
                FullName = "Test Staff",
                Role = 1
            };

            // Setup token generation dependencies
            SetupTokenGenerationDependencies();

            _mockStaffRepo.Setup(x => x.Login(It.IsAny<staff>(), It.IsAny<string>()))
                .ReturnsAsync(staffModel);

            // Act
            var result = await _controller.LoginAdmin(loginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<MessageResponse>(okResult.Value);

            // Detailed assertions
            Assert.NotNull(response);
        }

        [Fact]
        public async Task GetAccountStaffById_ExistingId_ReturnsStaff()
        {
            // Arrange
            int staffId = 1;
            var staffDto = new GetStaffByIdResponseDTO
            {
                StaffId = staffId,
                FullName = "Test Staff",
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                Role = 1
            };

            _mockStaffRepo.Setup(x => x.GetAccountStaffForAdminByIdAsync(staffId))
                .ReturnsAsync(staffDto);

            // Act
            var result = await _controller.GetAccountStaffById(staffId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            var returnedStaff = Assert.IsType<GetStaffByIdResponseDTO>(okResult.Value);
            Assert.Equal(staffId, returnedStaff.StaffId);
        }
        #endregion

        #region Update Staff Tests
        [Fact]
        public async Task UpdateAccountStaffById_ValidUpdate_ReturnsOkResult()
        {
            // Arrange
            var updateDto = new UpdateInfoAccountStaffByIdDTO
            {
                StaffId = 1,
                FullName = "Updated Name",
                Email = "updated@example.com",
                PhoneNumber = "9876543210",
                Dob = DateTime.Now,
                Sex = true,
                StaffImage = "image.jpg",
                Password = "newpassword"
            };

            // Setup user claims
            var claims = new List<Claim>
            {
                new Claim("Id", "1")
            };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            _mockStaffRepo.Setup(x => x.UpdateAccountStaffById(It.IsAny<UpdateInfoAccountStaffByIdDTO>()))
                .ReturnsAsync(updateDto);

            // Act
            var result = await _controller.UpdateAccountStaffById(updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            var returnedDto = Assert.IsType<UpdateInfoAccountStaffByIdDTO>(okResult.Value);
            Assert.Equal(updateDto.FullName, returnedDto.FullName);
        }
        #endregion

        private void SetupTokenGenerationDependencies()
        {
            // Mock context SaveChangesAsync
            _mockContext.Setup(x => x.SaveChangesAsync(default))
                .ReturnsAsync(1);

            // Mock RefreshTokensStaffs collection
            var mockRefreshTokenSet = new Mock<DbSet<RefreshTokensStaff>>();
            _mockContext.Setup(x => x.RefreshTokensStaffs)
                .Returns(mockRefreshTokenSet.Object);

            // Mock staffs collection
            var mockStaffSet = new Mock<DbSet<staff>>();
            _mockContext.Setup(x => x.staffs)
                .Returns(mockStaffSet.Object);

            // Ensure _appSettings is properly set
            var appSettings = new AppSettingsKey { Secretkey = "TestSecretKeyForJWTTokenGeneration" };
            _mockOptionsMonitor.Setup(x => x.CurrentValue).Returns(appSettings);
        }
    }
}
