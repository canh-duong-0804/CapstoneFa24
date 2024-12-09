using System;
using System.Threading.Tasks;
using BusinessObject.Models;
using HealthTrackingManageAPI.Controllers;
using HealthTrackingManageAPI.NewFolder.Image;
using HealthTrackingManageAPI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Repository.IRepo;
using Xunit;
namespace HTUnitTests.Controller
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            // Setup mock repository and controller before each test
            _mockUserRepo = new Mock<IUserRepository>();
            _controller = new UsersController(_mockUserRepo.Object);
        }

        [Fact]
        public async Task CheckEmailUnique_UniqueEmail_ReturnsOkResult()
        {
            // Arrange
            string uniqueEmail = "unique.email@example.com";
            _mockUserRepo.Setup(repo => repo.IsUniqueEmail(uniqueEmail)).Returns(true);

            // Act
            var result = await _controller.CheckEmailUnique(uniqueEmail);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            _mockUserRepo.Verify(repo => repo.IsUniqueEmail(uniqueEmail), Times.Once);
        }

        [Fact]
        public async Task CheckEmailUnique_DuplicateEmail_ReturnsBadRequest()
        {
            // Arrange
            string duplicateEmail = "existing.email@example.com";
            _mockUserRepo.Setup(repo => repo.IsUniqueEmail(duplicateEmail)).Returns(false);

            // Act
            var result = await _controller.CheckEmailUnique(duplicateEmail);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Email already exists", badRequestResult.Value);
            _mockUserRepo.Verify(repo => repo.IsUniqueEmail(duplicateEmail), Times.Once);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task CheckEmailUnique_InvalidEmail_ShouldHandleEdgeCases(string email)
        {
            // Arrange
            _mockUserRepo.Setup(repo => repo.IsUniqueEmail(email)).Returns(true);

            // Act
            var result = await _controller.CheckEmailUnique(email);

            // Assert
            // Depending on your exact requirements, you might want to modify this test
            // This assumes you want to return Ok for empty/null emails
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            _mockUserRepo.Verify(repo => repo.IsUniqueEmail(email), Times.Once);
        }
    }
}