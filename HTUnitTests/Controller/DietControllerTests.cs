using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using HealthTrackingManageAPI.Controllers;
using BusinessObject.Models;
using Repository.IRepo;
using BusinessObject.Dto.Diet;

namespace HTUnitTests.Controller
{
    public class DietControllerTests
    {
        private readonly Mock<IFoodRepository> _mockDietRepository;
        private readonly DietController _controller;

        public DietControllerTests()
        {
            _mockDietRepository = new Mock<IFoodRepository>();
            _controller = new DietController(_mockDietRepository.Object);
        }

        [Fact]
        public async Task GetAllDiet_NormalCase_ReturnsOk()
        {
            // Arrange
            var mockDiets = new List<DietResponseDTO>
            {
                new DietResponseDTO { DietId = 1, DietName = "Keto Diet" },
                new DietResponseDTO { DietId = 2, DietName = "Paleo Diet" }
            };

            _mockDietRepository.Setup(repo => repo.GetAllDietAsync())
                .ReturnsAsync(mockDiets);

            // Act
            var result = await _controller.GetAllDiet();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var objectResult = result as OkObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(mockDiets, objectResult.Value);
        }

        [Fact]
        public async Task GetAllDiet_EmptyList_ReturnsOk()
        {
            // Arrange
            var mockDiets = new List<DietResponseDTO>(); // Empty list

            _mockDietRepository.Setup(repo => repo.GetAllDietAsync())
                .ReturnsAsync(mockDiets);

            // Act
            var result = await _controller.GetAllDiet();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var objectResult = result as OkObjectResult;
            Assert.NotNull(objectResult);
            Assert.Empty((List<DietResponseDTO>)objectResult.Value);
        }

        


       
    }
}
