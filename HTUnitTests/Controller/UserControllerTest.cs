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

public class UsersControllerTests
{
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<IOptionsMonitor<AppSettingsKey>> _mockOptionsMonitor;
    private readonly Mock<CloudinaryService> _mockCloudinaryService;
    private readonly Mock<HealthTrackingDBContext> _mockContext;
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        // Setup mocks
        _mockUserRepo = new Mock<IUserRepository>();
        _mockConfiguration = new Mock<IConfiguration>();
        _mockOptionsMonitor = new Mock<IOptionsMonitor<AppSettingsKey>>();
        _mockCloudinaryService = new Mock<CloudinaryService>();
        _mockContext = new Mock<HealthTrackingDBContext>();

        // Create mock AppSettingsKey
        var mockAppSettings = new AppSettingsKey();
        _mockOptionsMonitor.Setup(x => x.CurrentValue).Returns(mockAppSettings);

        // Initialize controller
        _controller = new UsersController(
            _mockContext.Object,
            _mockConfiguration.Object,
            _mockUserRepo.Object,
            _mockOptionsMonitor.Object,
            _mockCloudinaryService.Object
        );
    }

    [Fact]
    public async Task CheckEmailUnique_UniqueEmail_ReturnsOkResult()
    {
        // Arrange
        string uniqueEmail = "test@example.com";
        _mockUserRepo.Setup(x => x.IsUniqueEmail(uniqueEmail)).Returns(true);

        // Act
        var result = await _controller.CheckEmailUnique(uniqueEmail);

        // Assert
        var okResult = Assert.IsType<OkResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task CheckEmailUnique_DuplicateEmail_ReturnsBadRequest()
    {
        // Arrange
        string duplicateEmail = "existing@example.com";
        _mockUserRepo.Setup(x => x.IsUniqueEmail(duplicateEmail)).Returns(false);

        // Act
        var result = await _controller.CheckEmailUnique(duplicateEmail);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.Equal("Email already exists", badRequestResult.Value);
    }

    [Fact]
    public async Task CheckPhoneUnique_UniquePhone_ReturnsOkResult()
    {
        // Arrange
        string uniquePhone = "1234567890";
        _mockUserRepo.Setup(x => x.IsUniquePhonenumber(uniquePhone)).Returns(true);

        // Act
        var result = await _controller.CheckPhoneUnique(uniquePhone);

        // Assert
        var okResult = Assert.IsType<OkResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task CheckPhoneUnique_DuplicatePhone_ReturnsBadRequest()
    {
        // Arrange
        string duplicatePhone = "9876543210";
        _mockUserRepo.Setup(x => x.IsUniquePhonenumber(duplicatePhone)).Returns(false);

        // Act
        var result = await _controller.CheckPhoneUnique(duplicatePhone);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.Equal("Phone number already exists", badRequestResult.Value);
    }
}