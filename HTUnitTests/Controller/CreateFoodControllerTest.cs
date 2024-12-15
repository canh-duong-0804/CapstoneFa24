/*using BusinessObject.Dto.Food;
using BusinessObject.Models;
using HealthTrackingManageAPI.Controllers;
using HealthTrackingManageAPI.NewFolder.Image;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTUnitTests.Controller
{
    public class CreateFoodControllerTest
    {
        private readonly Mock<IFoodRepository> _mockFoodRepository;
        private readonly Mock<CloudinaryService> _mockCloudinaryService;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<ILogger<FoodController>> _mockLogger;
        private readonly FoodController _controller;


        public CreateFoodControllerTest()
        {
            _mockFoodRepository = new Mock<IFoodRepository>();
            // Mock Cloudinary configuration
            _mockConfiguration.Setup(x => x["Cloudinary:CloudName"]).Returns("test_cloud_name");
            _mockConfiguration.Setup(x => x["Cloudinary:ApiKey"]).Returns("test_api_key");
            _mockConfiguration.Setup(x => x["Cloudinary:ApiSecret"]).Returns("test_api_secret");

            var mockLogger = new Mock<ILogger<CloudinaryService>>();

            // Create a mock CloudinaryService using the actual constructor
            _mockCloudinaryService = new Mock<CloudinaryService>(
                _mockConfiguration.Object,
                mockLogger.Object
            )
            { CallBase = true };
            _controller = new FoodController(_mockFoodRepository.Object, _mockCloudinaryService.Object);
        }

        // UTCID01: Valid page number with existing foods
        [Fact]
        public async Task GetAllFoodsForStaff_ValidPage_ReturnsOkResult()
        {
            // Arrange
            int page = 1;
            int totalFoods = 10;
            var mockFoods = CreateMockFoodList(5);

            _mockFoodRepository.Setup(r => r.GetTotalFoodsForStaffAsync()).ReturnsAsync(totalFoods);
            _mockFoodRepository.Setup(r => r.GetAllFoodsForStaffAsync(page, 5)).ReturnsAsync(mockFoods);

            // Act
            var result = await _controller.GetAllFoodsForStaff(page);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value as dynamic;
            Assert.Equal(5, response.Foods.Count);
            Assert.Equal(2, response.TotalPages);
            Assert.Equal(page, response.CurrentPage);
        }

        // UTCID02: No page number specified (default to page 1)
        [Fact]
        public async Task GetAllFoodsForStaff_NoPageSpecified_ReturnsFirstPage()
        {
            // Arrange
            int totalFoods = 10;
            var mockFoods = CreateMockFoodList(5);

            _mockFoodRepository.Setup(r => r.GetTotalFoodsForStaffAsync()).ReturnsAsync(totalFoods);
            _mockFoodRepository.Setup(r => r.GetAllFoodsForStaffAsync(1, 5)).ReturnsAsync(mockFoods);

            // Act
            var result = await _controller.GetAllFoodsForStaff(null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value as dynamic;
            Assert.Equal(1, response.CurrentPage);
        }

        // UTCID03: Invalid page number (less than 1)
        [Fact]
        public async Task GetAllFoodsForStaff_InvalidPageNumber_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.GetAllFoodsForStaff(-1);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Page number must be greater than or equal to 1.", badRequestResult.Value);
        }

        // UTCID04: Page number exceeds total pages
        [Fact]
        public async Task GetAllFoodsForStaff_PageExceedsTotalPages_ReturnsLastPage()
        {
            // Arrange
            int totalFoods = 10;
            var mockFoods = CreateMockFoodList(5);

            _mockFoodRepository.Setup(r => r.GetTotalFoodsForStaffAsync()).ReturnsAsync(totalFoods);
            _mockFoodRepository.Setup(r => r.GetAllFoodsForStaffAsync(2, 5)).ReturnsAsync(mockFoods);

            // Act
            var result = await _controller.GetAllFoodsForStaff(3);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value as dynamic;
            Assert.Equal(2, response.CurrentPage);
        }

        // UTCID05: No foods found
        [Fact]
        public async Task GetAllFoodsForStaff_NoFoodsFound_ReturnsNotFound()
        {
            // Arrange
            _mockFoodRepository.Setup(r => r.GetTotalFoodsForStaffAsync()).ReturnsAsync(0);

            // Act
            var result = await _controller.GetAllFoodsForStaff(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        // Helper method to create mock food list
        private IEnumerable<AllFoodForStaffResponseDTO> CreateMockFoodList(int count)
        {
            return Enumerable.Range(1, count)
                .Select(i => new AllFoodForStaffResponseDTO
                {

                    FoodId = i,
                    FoodName = $"Food {i}"
                })
                .ToList();
        }
    }
}
*/