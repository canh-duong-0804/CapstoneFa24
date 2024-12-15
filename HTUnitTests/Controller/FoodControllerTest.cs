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
    public class FoodControllerTests
    {
        private readonly Mock<IFoodRepository> _mockFoodRepository;
        private readonly Mock<CloudinaryService> _mockCloudinaryService;
        private readonly FoodController _controller;

        public FoodControllerTests()
        {
            _mockFoodRepository = new Mock<IFoodRepository>();

            // Mock the dependencies for CloudinaryService
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(x => x["Cloudinary:CloudName"]).Returns("test_cloud_name");
            mockConfiguration.Setup(x => x["Cloudinary:ApiKey"]).Returns("test_api_key");
            mockConfiguration.Setup(x => x["Cloudinary:ApiSecret"]).Returns("test_api_secret");

            var mockLogger = new Mock<ILogger<CloudinaryService>>();

            // Create a mock CloudinaryService using the actual constructor
            _mockCloudinaryService = new Mock<CloudinaryService>(
                mockConfiguration.Object,
                mockLogger.Object
            )
            { CallBase = true };

            _controller = new FoodController(_mockFoodRepository.Object, _mockCloudinaryService.Object);

            // Setup user claims for authorized methods
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("Id", "1")
            }));
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        #region GetAllFoodsForStaff Tests
        [Fact]
        public async Task GetAllFoodsForStaff_ValidPage_ReturnsOkResult()
        {
            // Arrange
            int page = 1;
            int pageSize = 5;
            int totalFoods = 10;
            var mockFoods = new List<AllFoodForStaffResponseDTO>
    {
        new AllFoodForStaffResponseDTO { FoodId = 1, FoodName = "Apple" },
        new AllFoodForStaffResponseDTO { FoodId = 2, FoodName = "Banana" }
    };
            _mockFoodRepository.Setup(x => x.GetTotalFoodsForStaffAsync())
                .ReturnsAsync(totalFoods);
            _mockFoodRepository.Setup(x => x.GetAllFoodsForStaffAsync(page, pageSize))
                .ReturnsAsync(mockFoods);

            // Act
            var result = await _controller.GetAllFoodsForStaff(page);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            // Use reflection to check the anonymous type
            var resultValue = okResult.Value;
            var resultType = resultValue.GetType();

            // Check Foods property
            var foodsProperty = resultType.GetProperty("Foods");
            Assert.NotNull(foodsProperty);
            var foods = foodsProperty.GetValue(resultValue) as IEnumerable<AllFoodForStaffResponseDTO>;
            Assert.Equal(mockFoods, foods);

            // Check TotalPages property
            var totalPagesProperty = resultType.GetProperty("TotalPages");
            Assert.NotNull(totalPagesProperty);
            var totalPages = (int)totalPagesProperty.GetValue(resultValue);
            Assert.Equal(2, totalPages);

            // Check CurrentPage property
            var currentPageProperty = resultType.GetProperty("CurrentPage");
            Assert.NotNull(currentPageProperty);
            var currentPage = (int)currentPageProperty.GetValue(resultValue);
            Assert.Equal(page, currentPage);
        }

        [Fact]
        public async Task GetAllFoodsForStaff_NoFoods_ReturnsNotFound()
        {
            // Arrange
            _mockFoodRepository.Setup(x => x.GetTotalFoodsForStaffAsync())
                .ReturnsAsync(0);

            // Act
            var result = await _controller.GetAllFoodsForStaff(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetAllFoodsForStaff_InvalidPage_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.GetAllFoodsForStaff(-1);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        #endregion

        #region CreateFood Tests
        [Fact]
        public async Task CreateFood_ValidFood_ReturnsOkResultWithFoodId()
        {
            // Arrange
            var createFoodDto = new CreateFoodRequestDTO
            {
                FoodName = "Test Food",
                Portion = "100g",
                Serving = "1 slice",
                Calories = 100
            };

            var createdFood = new Food { FoodId = 1 };

            _mockFoodRepository.Setup(x => x.CreateFoodAsync(It.IsAny<Food>()))
                .ReturnsAsync(createdFood);

            // Act
            var result = await _controller.CreateFood(createFoodDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(1, okResult.Value);
        }

        [Fact]
        public async Task CreateFood_NullFood_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.CreateFood(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        #endregion

        #region UpdateFood Tests
        #region UpdateFood Tests
        [Fact]
        public async Task UpdateFood_ValidFood_ReturnsNoContent()
        {
            // Arrange
            var updateFoodDto = new UpdateFoodRequestDTO
            {
                FoodId = 1,
                FoodName = "Updated Food",
                Portion = "200g",
                Serving = "2 slices",
                Calories = 200
            };

            var updatedFood = new Food
            {
                FoodId = 1,
                FoodName = "Updated Food"
            };

            _mockFoodRepository.Setup(x => x.UpdateFoodAsync(It.IsAny<Food>()))
                .ReturnsAsync(updatedFood);

            // Act
            var result = await _controller.UpdateFoodStatus(updateFoodDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateFood_NullResult_ReturnsNotFound()
        {
            // Arrange
            var updateFoodDto = new UpdateFoodRequestDTO
            {
                FoodId = 1,
                FoodName = "Updated Food"
            };

            _mockFoodRepository.Setup(x => x.UpdateFoodAsync(It.IsAny<Food>()))
                .ReturnsAsync((Food)null);

            // Act
            var result = await _controller.UpdateFoodStatus(updateFoodDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        #endregion

        #endregion

        #region DeleteFood Tests
        [Fact]
        public async Task DeleteFood_ExistingFood_ReturnsNoContent()
        {
            // Arrange
            int foodId = 1;
            _mockFoodRepository.Setup(x => x.DeleteFoodAsync(foodId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteFood(foodId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteFood_NonExistingFood_ReturnsNotFound()
        {
            // Arrange
            int foodId = 1;
            _mockFoodRepository.Setup(x => x.DeleteFoodAsync(foodId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteFood(foodId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Food item not found or already deleted.", notFoundResult.Value);
        }
        #endregion

        #region GetFoodForStaffById Tests
        [Fact]
        public async Task GetFoodForStaffById_ExistingFood_ReturnsOkResult()
        {
            // Arrange
            int foodId = 1;
            var foodDto = new GetFoodForStaffByIdResponseDTO { FoodId = foodId, FoodName = "Test Food" };

            _mockFoodRepository.Setup(x => x.GetFoodForStaffByIdAsync(foodId))
                .ReturnsAsync(foodDto);

            // Act
            var result = await _controller.GetFoodForStaffByIdAsync(foodId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(foodDto, okResult.Value);
        }

        [Fact]
        public async Task GetFoodForStaffById_NonExistingFood_ReturnsNotFound()
        {
            // Arrange
            int foodId = 1;
            _mockFoodRepository.Setup(x => x.GetFoodForStaffByIdAsync(foodId))
                .ReturnsAsync((GetFoodForStaffByIdResponseDTO)null);

            // Act
            var result = await _controller.GetFoodForStaffByIdAsync(foodId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Food not found.", notFoundResult.Value);
        }
        #endregion

        #region GetFoodForMemberById Tests
        [Fact]
        public async Task GetFoodDiaryForMemberById_ExistingFood_ReturnsOkResult()
        {
            // Arrange
            int foodId = 1;
            DateTime selectDate = DateTime.Now;
            var foodDto = new GetFoodForMemberByIdResponseDTO { FoodId = foodId, FoodName = "Test Food" };

            _mockFoodRepository.Setup(x => x.GetFoodForMemberByIdAsync(foodId, selectDate, 1))
                .ReturnsAsync(foodDto);

            // Act
            var result = await _controller.GetFoodDiaryForMemberById(foodId, selectDate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(foodDto, okResult.Value);
        }

        [Fact]
        public async Task GetFoodDiaryForMemberById_NonExistingFood_ReturnsNotFound()
        {
            // Arrange
            int foodId = 1;
            DateTime selectDate = DateTime.Now;

            _mockFoodRepository.Setup(x => x.GetFoodForMemberByIdAsync(foodId, selectDate, 1))
                .ReturnsAsync((GetFoodForMemberByIdResponseDTO)null);

            // Act
            var result = await _controller.GetFoodDiaryForMemberById(foodId, selectDate);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Food not found.", notFoundResult.Value);
        }
        #endregion
    }
}
