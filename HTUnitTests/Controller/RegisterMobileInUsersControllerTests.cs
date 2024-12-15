/*using AutoMapper;
using BusinessObject.Dto.Register;
using BusinessObject.Models;
using HealthTrackingManageAPI;
using HealthTrackingManageAPI.Controllers;
using HealthTrackingManageAPI.NewFolder.Image;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTUnitTests.Controller
{
    public class RegisterMobileInUsersControllerTests
    {
        private readonly Mock<IUserRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IOptionsMonitor<AppSettingsKey>> _mockOptionsMonitor;
        private readonly Mock<CloudinaryService> _mockCloudinaryService;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<HealthTrackingDBContext> _mockContext;
        private readonly UsersController _controller;

        public RegisterMobileInUsersControllerTests()
        {
            _mockRepo = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockOptionsMonitor = new Mock<IOptionsMonitor<AppSettingsKey>>();
            _mockCloudinaryService = new Mock<CloudinaryService>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockContext = new Mock<HealthTrackingDBContext>();

            // Mock AppSettingsKey
            var appSettingsKey = new AppSettingsKey { *//* Initialize if needed *//* };
            _mockOptionsMonitor.Setup(o => o.CurrentValue).Returns(appSettingsKey);

            _controller = new UsersController(
                _mockContext.Object,
                _mockConfiguration.Object,
                _mockRepo.Object,
                _mockOptionsMonitor.Object,
                _mockCloudinaryService.Object
            );
        }

        [Fact]
        public async Task RegisterMobile_NormalCase_ReturnsOk()
        {
            // Arrange
            var requestDto = new RegisterationMobileRequestDTO
            {
                UserName = "testuser",
                Email = "test@example.com",
                Password = "P@ssword123",
                Dob = DateTime.Now.AddYears(-25),
                Gender = true,
                Height = 170,
                Weight = 70,
                weightPerWeek = 1,
                TargetWeight = 65,
                DietId = 1,
                ExerciseLevel = 3,
                PhoneNumber = "1234567890"
            };

            var mappedMember = new Member { Email = requestDto.Email };
            _mockMapper.Setup(m => m.Map<Member>(requestDto)).Returns(mappedMember);
            _mockRepo.Setup(r => r.Register(mappedMember, requestDto)).ReturnsAsync(new Member());

            // Act
            var result = await _controller.RegisterMobile(requestDto);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task RegisterMobile_AbnormalCase_ReturnsBadRequest()
        {
            // Arrange
            var requestDto = new RegisterationMobileRequestDTO
            {
                UserName = "",
                Email = "invalidemail",
                Password = "",
                Dob = DateTime.Now,
                Gender = true,
                Height = 0,
                Weight = -5,
                weightPerWeek = -1,
                TargetWeight = null,
                DietId = null,
                ExerciseLevel = 0,
                PhoneNumber = ""
            };

            var mappedMember = new Member { Email = requestDto.Email };
            _mockMapper.Setup(m => m.Map<Member>(requestDto)).Returns(mappedMember);
            _mockRepo.Setup(r => r.Register(mappedMember, requestDto)).ReturnsAsync((Member)null);

            // Act
            var result = await _controller.RegisterMobile(requestDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = result as BadRequestObjectResult;
            Assert.Equal("Error while registering the user", objectResult?.Value);
        }

        [Fact]
        public async Task RegisterMobile_BoundaryCase_Valid_ReturnsOk()
        {
            // Arrange
            var requestDto = new RegisterationMobileRequestDTO
            {
                UserName = "BoundaryUser",
                Email = "boundary@example.com",
                Password = "BoundaryP@ss1",
                Dob = DateTime.Now.AddYears(-18), // Minimum allowable age
                Gender = true,
                Height = 250, // Max height
                Weight = 500, // Max weight
                weightPerWeek = 0.1, // Minimum reasonable weight loss
                TargetWeight = 450,
                DietId = 1000, // Extreme diet ID
                ExerciseLevel = 10, // Max level
                PhoneNumber = "9876543210"
            };

            var mappedMember = new Member { Email = requestDto.Email };
            _mockMapper.Setup(m => m.Map<Member>(requestDto)).Returns(mappedMember);
            _mockRepo.Setup(r => r.Register(mappedMember, requestDto)).ReturnsAsync(new Member());

            // Act
            var result = await _controller.RegisterMobile(requestDto);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task RegisterMobile_BoundaryCase_Invalid_ReturnsBadRequest()
        {
            // Arrange
            var requestDto = new RegisterationMobileRequestDTO
            {
                UserName = "InvalidBoundaryUser",
                Email = "invalid@example.com",
                Password = "short",
                Dob = DateTime.Now.AddYears(1), // Future date
                Gender = true,
                Height = 0, // Min height
                Weight = -1, // Invalid weight
                weightPerWeek = -10, // Invalid weight loss
                TargetWeight = -50,
                DietId = -1, // Invalid diet ID
                ExerciseLevel = -5, // Invalid level
                PhoneNumber = ""
            };

            var mappedMember = new Member { Email = requestDto.Email };
            _mockMapper.Setup(m => m.Map<Member>(requestDto)).Returns(mappedMember);
            _mockRepo.Setup(r => r.Register(mappedMember, requestDto)).ReturnsAsync((Member)null);

            // Act
            var result = await _controller.RegisterMobile(requestDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = result as BadRequestObjectResult;
            Assert.Equal("Error while registering the user", objectResult?.Value);
        }
    }
}

*/