using BusinessObject.Dto.MealMember;
using BusinessObject.Dto.MealPlan;
using BusinessObject.Dto.MealPlanDetail;
using BusinessObject.Models;
using HealthTrackingManageAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class MealPlanTrainnerControllerTests
    {
        private readonly Mock<IMealPlanTrainnerRepository> _mockMealPlanRepository;
        private readonly MealPlanTrainnerController _controller;

        public MealPlanTrainnerControllerTests()
        {
            _mockMealPlanRepository = new Mock<IMealPlanTrainnerRepository>();
            _controller = new MealPlanTrainnerController(_mockMealPlanRepository.Object);

            // Setup default user claims
            var claims = new List<Claim>
            {
                new Claim("Id", "1"),
                new Claim(ClaimTypes.Role, "1")
            };
            var identity = new ClaimsIdentity(claims, "TestAuthentication");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };
        }

        #region CreateMealPlanTrainer Tests
        [Fact]
        public async Task CreateMealPlanTrainer_ValidRequest_ReturnsOk()
        {
			// Arrange
			var request = new CreateMealPlanRequestDTO
			{
				Name = "Healthy Plan",
				TotalCalories = 2000
			};

			_mockMealPlanRepository
				.Setup(repo => repo.CreateMealPlanTrainerAsync(It.Is<MealPlan>(
					m => m.Name == request.Name && m.TotalCalories == request.TotalCalories)))
				.ReturnsAsync(true);

			// Act
			var result = await _controller.CreateMealPlanTrainer(request);

			// Assert
			Assert.IsType<OkResult>(result);
		}

        [Fact]
        public async Task CreateMealPlanTrainer_RepositoryFails_ReturnsStatusCode500()
        {
            // Arrange
            var request = new CreateMealPlanRequestDTO();
            _mockMealPlanRepository
                .Setup(repo => repo.CreateMealPlanTrainerAsync(It.IsAny<MealPlan>()))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.CreateMealPlanTrainer(request);

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }
        #endregion

        #region UpdateMealPlanTrainer Tests
        [Fact]
        public async Task UpdateMealPlanTrainer_ValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new UpdateMealPlanRequestDTO();
            _mockMealPlanRepository
                .Setup(repo => repo.UpdateMealPlanTrainerAsync(It.IsAny<MealPlan>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateMealPlanTrainer(request);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdateMealPlanTrainer_RepositoryFails_ReturnsStatusCode500()
        {
            // Arrange
            var request = new UpdateMealPlanRequestDTO();
            _mockMealPlanRepository
                .Setup(repo => repo.UpdateMealPlanTrainerAsync(It.IsAny<MealPlan>()))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateMealPlanTrainer(request);

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }
        #endregion

        #region DeleteMealPlan Tests
        [Fact]
        public async Task DeleteMealPlan_ValidRequest_ReturnsOk()
        {
            // Arrange
            int mealPlanId = 1;
            _mockMealPlanRepository
                .Setup(repo => repo.DeleteMealPlanAsync(mealPlanId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteMealPlan(mealPlanId);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteMealPlan_RepositoryFails_ReturnsStatusCode500()
        {
            // Arrange
            int mealPlanId = 1;
            _mockMealPlanRepository
                .Setup(repo => repo.DeleteMealPlanAsync(mealPlanId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteMealPlan(mealPlanId);

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }
        #endregion

        #region GetMealPlan Tests
        [Fact]
        public async Task GetMealPlan_ValidRequest_ReturnsOkWithData()
        {
            // Arrange
            int mealPlanId = 1;
            var expectedMealPlan = new GetMealPlanResponseDTO();
            _mockMealPlanRepository
                .Setup(repo => repo.GetMealPlanAsync(mealPlanId))
                .ReturnsAsync(expectedMealPlan);

            // Act
            var result = await _controller.GetMealPlan(mealPlanId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedMealPlan, okResult.Value);
        }

		
		#endregion

		#region CreateMealPlanDetail Tests
		[Fact]
        public async Task CreateMealPlanDetail_ValidRequest_ReturnsOkWithMessage()
        {
            // Arrange
            var request = new CreateMealPlanDetailRequestDTO();
            _mockMealPlanRepository
                .Setup(repo => repo.CreateMealPlanDetailAsync(request))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.CreateMealPlanDetail(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Asserts that the result is 200 OK
            Assert.Equal(200, okResult.StatusCode); // Asserts the status code
        }


        [Fact]
        public async Task CreateMealPlanDetail_RepositoryFails_ReturnsStatusCode500()
        {
            // Arrange
            var request = new CreateMealPlanDetailRequestDTO();
            _mockMealPlanRepository
                .Setup(repo => repo.CreateMealPlanDetailAsync(request))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.CreateMealPlanDetail(request);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Failed to create meal plan detail.", objectResult.Value);
        }

        [Fact]
        public async Task CreateMealPlanDetail_DayZero_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new CreateMealPlanDetailRequestDTO
            {
                MealPlanId = 1,
                Day = 0, // Invalid day
                DescriptionBreakFast = "Test Breakfast",
                ListFoodIdBreakfasts = new List<FoodQuantityResponseDTO>
        {
            new FoodQuantityResponseDTO { FoodId = 101, Quantity = 100 }
        }
            };

            // Act
            var result = await _controller.CreateMealPlanDetail(request);

            // Assert
            var badRequestResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, badRequestResult.StatusCode);


        }
		[Fact]
		public async Task CreateMealPlanDetail_Dayinvalid_ShouldReturnBadRequest()
		{
			// Arrange
			var request = new CreateMealPlanDetailRequestDTO
			{
				MealPlanId = 1,
				Day = 200, // Invalid day
				DescriptionBreakFast = "Test Breakfast",
				ListFoodIdBreakfasts = new List<FoodQuantityResponseDTO>
		{
			new FoodQuantityResponseDTO { FoodId = 101, Quantity = 100 }
		}
			};

			// Act
			var result = await _controller.CreateMealPlanDetail(request);

			// Assert
			var badRequestResult = Assert.IsType<ObjectResult>(result);
			Assert.Equal(500, badRequestResult.StatusCode);


		}

		#endregion

		#region GetMealPlanDetail Tests
		[Fact]
        public async Task GetMealPlanDetail_ValidRequest_ReturnsOkWithData()
        {
            // Arrange
            int mealPlanId = 1;
            int day = 1;
            var expectedMealPlanDetail = new GetMealPlanDetaiTrainnerlResponseDTO();
            _mockMealPlanRepository
                .Setup(repo => repo.GetMealPlanDetailAsync(mealPlanId, day))
                .ReturnsAsync(expectedMealPlanDetail);

            // Act
            var result = await _controller.GetMealPlanDetail(mealPlanId, day);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedMealPlanDetail, okResult.Value);
        }

        [Fact]
        public async Task GetMealPlanDetail_NullResult_ReturnsNotFound()
        {
            // Arrange
            int mealPlanId = 1;
            int day = 1;
            _mockMealPlanRepository
                .Setup(repo => repo.GetMealPlanDetailAsync(mealPlanId, day))
                .ReturnsAsync((GetMealPlanDetaiTrainnerlResponseDTO)null);

            // Act
            var result = await _controller.GetMealPlanDetail(mealPlanId, day);

            // Assert
            var okResult = Assert.IsType<NotFoundObjectResult>(result); // Asserts that the result is 200 OK
            Assert.Equal(404, okResult.StatusCode); // Asserts the status code
        }
        #endregion

        #region UpdateMealPlanDetail Tests
        [Fact]
        public async Task UpdateMealPlanDetail_ValidRequest_ReturnsOkWithMessage()
        {
            // Arrange
            var request = new CreateMealPlanDetailRequestDTO();
            _mockMealPlanRepository
                .Setup(repo => repo.UpdateMealPlanDetailAsync(request))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateMealPlanDetail(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Asserts that the result is 200 OK
            Assert.Equal(200, okResult.StatusCode); // Asserts the status code
        }

        [Fact]
        public async Task UpdateMealPlanDetail_RepositoryFails_ReturnsStatusCode500()
        {
            // Arrange
            var request = new CreateMealPlanDetailRequestDTO();
            _mockMealPlanRepository
                .Setup(repo => repo.UpdateMealPlanDetailAsync(request))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateMealPlanDetail(request);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);

           
        }


		[Fact]
		public async Task UpdateMealPlanDetail_DayInvalid_ShouldReturnBadRequest()
		{
			// Arrange
			var request = new CreateMealPlanDetailRequestDTO
			{
				MealPlanId = 1,
				Day = 0, // Invalid day (outside of 1-254 range)
				DescriptionBreakFast = "Test Breakfast",
				ListFoodIdBreakfasts = new List<FoodQuantityResponseDTO>
		{
			new FoodQuantityResponseDTO { FoodId = 101, Quantity = 100 }
		}
			};

			// Act
			var result = await _controller.UpdateMealPlanDetail(request);

			// Assert
			var objectResult = Assert.IsType<ObjectResult>(result);
			Assert.Equal(500, objectResult.StatusCode);
			
		}

		[Fact]
		public async Task UpdateMealPlanDetail_DayInvalid1_ShouldReturnBadRequest()
		{
			// Arrange
			var request = new CreateMealPlanDetailRequestDTO
			{
				MealPlanId = 1,
				Day = 255, // Invalid day (outside of 1-254 range)
				DescriptionBreakFast = "Test Breakfast",
				ListFoodIdBreakfasts = new List<FoodQuantityResponseDTO>
		{
			new FoodQuantityResponseDTO { FoodId = 101, Quantity = 100 }
		}
			};

			// Act
			var result = await _controller.UpdateMealPlanDetail(request);

			// Assert
			var objectResult = Assert.IsType<ObjectResult>(result);
			Assert.Equal(500, objectResult.StatusCode);

		}
		#endregion


	}
}
