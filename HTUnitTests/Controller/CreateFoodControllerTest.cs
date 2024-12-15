using BusinessObject.Dto.Food;
using BusinessObject.Models;
using HealthTrackingManageAPI.Controllers;
using HealthTrackingManageAPI.NewFolder.Image;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
    public class CreateFoodControllerTest
    {
        private readonly Mock<IFoodRepository> _mockFoodRepository;
        private readonly Mock<CloudinaryService> _mockCloudinaryService;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<ILogger<CloudinaryService>> _mockCloudinaryLogger;
        private readonly FoodController _controller;

        public CreateFoodControllerTest()
        {
            // Initialize configuration mock
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(x => x["Cloudinary:CloudName"]).Returns("test_cloud_name");
            _mockConfiguration.Setup(x => x["Cloudinary:ApiKey"]).Returns("test_api_key");
            _mockConfiguration.Setup(x => x["Cloudinary:ApiSecret"]).Returns("test_api_secret");

            // Initialize Cloudinary logger mock
            _mockCloudinaryLogger = new Mock<ILogger<CloudinaryService>>();

            // Create CloudinaryService mock
            _mockCloudinaryService = new Mock<CloudinaryService>(
                _mockConfiguration.Object,
                _mockCloudinaryLogger.Object
            )
            { CallBase = true };

            // Initialize repository mock
            _mockFoodRepository = new Mock<IFoodRepository>();

            // Create controller
            _controller = new FoodController(_mockFoodRepository.Object, _mockCloudinaryService.Object);
        }

        private void SetupUserClaims(int memberId = 1)
        {
            var claims = new List<Claim>
            {
                new Claim("Id", memberId.ToString())
            };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };
        }

        // UTCID01: Successful food creation
        [Fact]
        public async Task CreateFood_ValidInput_ReturnsOkResultWithFoodId()
        {
            // Arrange
            SetupUserClaims();

            var createFoodDto = new CreateFoodRequestDTO
            {
                FoodName = "Test Food",
                Calories = 100,
                Portion = "1 serving",
                Serving = "100g"
            };

            var expectedFood = new Food
            {
                FoodId = 1,
                FoodName = "Test Food",
                Calories = 100,
                Portion = "1 serving (100g)"
            };

            _mockFoodRepository
                .Setup(r => r.CreateFoodAsync(It.IsAny<Food>()))
                .ReturnsAsync(expectedFood);

            // Act
            var result = await _controller.CreateFood(createFoodDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(expectedFood.FoodId, okResult.Value);

            _mockFoodRepository.Verify(r => r.CreateFoodAsync(It.Is<Food>(
                f => f.FoodName == createFoodDto.FoodName &&
                     f.Calories == createFoodDto.Calories &&
                     f.CreateBy == 1 &&
                     f.Portion == "1 serving (100g)"
            )), Times.Once);

            //what is verify mean in above code 
            //

        }

        // UTCID02: Create food with no serving
        [Fact]
        public async Task CreateFood_NoServing_PortionSetCorrectly()
        {
            // Arrange
            SetupUserClaims();

            var createFoodDto = new CreateFoodRequestDTO
            {
                FoodName = "Test Food",
                Calories = 100,
                Portion = "1 serving",
                Serving = null
            };

            var expectedFood = new Food
            {
                FoodId = 1,
                FoodName = "Test Food",
                Calories = 100,
                Portion = "1 serving"
            };

            _mockFoodRepository
                .Setup(r => r.CreateFoodAsync(It.IsAny<Food>()))
                .ReturnsAsync(expectedFood);

            // Act
            var result = await _controller.CreateFood(createFoodDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedFood.FoodId, okResult.Value);
            _mockFoodRepository.Verify(r => r.CreateFoodAsync(It.Is<Food>(
                f => f.Portion == "1 serving"
            )), Times.Once);
        }

        // UTCID03: Null food object
        [Fact]
        public async Task CreateFood_NullFoodObject_ReturnsBadRequest()
        {
            // Arrange
            SetupUserClaims();

            // Act
            var result = await _controller.CreateFood(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Food object is null.", badRequestResult.Value);
        }

        // UTCID04: Missing member ID claim
        [Fact]
        public async Task CreateFood_MissingMemberIdClaim_ReturnsUnauthorized()
        {
            // Arrange
            var createFoodDto = new CreateFoodRequestDTO
            {
                FoodName = "Test Food"
            };

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };

            // Act
            var result = await _controller.CreateFood(createFoodDto);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Member ID not found in claims.", unauthorizedResult.Value);
        }

        // UTCID05: Invalid member ID claim
        [Fact]
        public async Task CreateFood_InvalidMemberIdClaim_ReturnsBadRequest()
        {
            // Arrange
            var createFoodDto = new CreateFoodRequestDTO
            {
                FoodName = "Test Food"
            };

            var claims = new List<Claim>
            {
                new Claim("Id", "invalid_id")
            };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            // Act
            var result = await _controller.CreateFood(createFoodDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid member ID.", badRequestResult.Value);
        }

        // UTCID06: Failed food creation
        [Fact]
        public async Task CreateFood_RepositoryReturnsNull_ReturnsBadRequest()
        {
            // Arrange
            SetupUserClaims();

            var createFoodDto = new CreateFoodRequestDTO
            {
                FoodName = "Test Food"
            };

            _mockFoodRepository
                .Setup(r => r.CreateFoodAsync(It.IsAny<Food>()))
                .ReturnsAsync((Food)null);

            // Act
            var result = await _controller.CreateFood(createFoodDto);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

    }
}
