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
    public class EmailCheckControllerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly UsersController _controller;

        public EmailCheckControllerTests()
        {
            // Setup mock dependencies
            _mockUserRepo = new Mock<IUserRepository>();

            // Create a mock configuration, context, options, and cloudinary service if needed
            var mockConfiguration = new Mock<IConfiguration>();
            var mockContext = new Mock<HealthTrackingDBContext>();
            var mockOptionsMonitor = new Mock<IOptionsMonitor<AppSettingsKey>>();
         //   var mockCloudinaryService = new Mock<CloudinaryService>();

            // Setup options monitor to return a default AppSettingsKey
            mockOptionsMonitor.Setup(x => x.CurrentValue).Returns(new AppSettingsKey());

            // Initialize the controller with mock dependencies
            _controller = new UsersController(
                mockContext.Object,
                mockConfiguration.Object,
                _mockUserRepo.Object,
                mockOptionsMonitor.Object,
                null
                //mockCloudinaryService.Object
            );
        }

        [Fact]
        public async Task CheckEmailUnique_UniqueEmail_ReturnsOk()
        {
            // Arrange
            string uniqueEmail = "test@example.com";
            _mockUserRepo.Setup(repo => repo.IsUniqueEmail(uniqueEmail)).Returns(true);

            // Act
            var result = await _controller.CheckEmailUnique(uniqueEmail);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task CheckEmailUnique_ExistingEmail_ReturnsBadRequest()
        {
            // Arrange
            string existingEmail = "ngocvd@gmail.com";
            _mockUserRepo.Setup(repo => repo.IsUniqueEmail(existingEmail)).Returns(false);

            // Act
            var result = await _controller.CheckEmailUnique(existingEmail);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Email already exists", badRequestResult.Value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task CheckEmailUnique_InvalidEmail_ReturnsBadRequest(string invalidEmail)
        {
            // Arrange
            _mockUserRepo.Setup(repo => repo.IsUniqueEmail(invalidEmail)).Returns(false);

            // Act
            var result = await _controller.CheckEmailUnique(invalidEmail);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CheckEmailUnique_AbnormalEmail_ReturnsBadRequest()
        {
            // Arrange
            string abnormalEmail = "<script>alert('test')</script>@example.com";
            _mockUserRepo.Setup(repo => repo.IsUniqueEmail(abnormalEmail)).Returns(false);

            // Act
            var result = await _controller.CheckEmailUnique(abnormalEmail);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CheckEmailUnique_BoundaryEmail_LongEmail_ReturnsBadRequest()
        {
            // Arrange
            string longEmail = new string('a', 250) + "@example.com";
            _mockUserRepo.Setup(repo => repo.IsUniqueEmail(longEmail)).Returns(false);

            // Act
            var result = await _controller.CheckEmailUnique(longEmail);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}