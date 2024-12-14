using BusinessObject.Dto.Nutrition;
using HealthTrackingManageAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Repository.IRepo;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace HTUnitTests.Controller
{
	public class NutrientsControllerTests
	{
		private readonly Mock<INutrientRepository> _mockNutrientRepository;
		private readonly NutrientsController _controller;

		public NutrientsControllerTests()
		{
			_mockNutrientRepository = new Mock<INutrientRepository>();
			_controller = new NutrientsController(_mockNutrientRepository.Object);
		}

		[Fact]
		public async Task GetDailyNutrition_ValidMemberAndDate_ReturnsOkResult()
		{
			// Arrange
			int memberId = 1;
			DateTime date = DateTime.Now.Date;
			var expectedNutrition = new DailyNutritionDto(); // Replace with actual DTO

			// Setup user claims
			var claims = new[]
			{
				new Claim("Id", memberId.ToString())
			};
			var identity = new ClaimsIdentity(claims, "TestAuthentication");
			var claimsPrincipal = new ClaimsPrincipal(identity);
			_controller.ControllerContext = new ControllerContext
			{
				HttpContext = new DefaultHttpContext { User = claimsPrincipal }
			};

			// Setup repository mock
			_mockNutrientRepository
				.Setup(repo => repo.CalculateDailyNutrition(It.IsAny<int>(), It.IsAny<DateTime>()))
				.ReturnsAsync(expectedNutrition);

			// Act
			var result = await _controller.GetDailyNutrition(date);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal(expectedNutrition, okResult.Value);
		}

        [Fact]
        public async Task GetDailyNutrition_Unauthorized_WhenMemberIdClaimIsMissing()
        {
            // Arrange
            DateTime date = DateTime.Now.Date;

            // Setup user claims with missing "Id"
            var claims = new Claim[] { };
            var identity = new ClaimsIdentity(claims, "TestAuthentication");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            // Act
            var result = await _controller.GetDailyNutrition(date);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Member ID not found in claims.", unauthorizedResult.Value);
        }

      

        [Fact]
        public async Task GetDailyNutrition_NotFound_WhenNoNutritionFoundForTheDate()
        {
            // Arrange
            int memberId = 1;
            DateTime date = DateTime.Now.Date;

            // Setup user claims
            var claims = new[] { new Claim("Id", memberId.ToString()) };
            var identity = new ClaimsIdentity(claims, "TestAuthentication");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            // Setup repository mock to return null for the specified member and date
            _mockNutrientRepository
                .Setup(repo => repo.CalculateDailyNutrition(It.IsAny<int>(), It.IsAny<DateTime>()))
                .ReturnsAsync((DailyNutritionDto)null);

            // Act
            var result = await _controller.GetDailyNutrition(date);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No daily nutrition found for the specified member and date.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetDailyNutrition_InternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            int memberId = 1;
            DateTime date = DateTime.Now.Date;

            // Setup user claims
            var claims = new[] { new Claim("Id", memberId.ToString()) };
            var identity = new ClaimsIdentity(claims, "TestAuthentication");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            // Setup repository mock to throw an exception
            _mockNutrientRepository
                .Setup(repo => repo.CalculateDailyNutrition(It.IsAny<int>(), It.IsAny<DateTime>()))
                .ThrowsAsync(new Exception("An error occurred while calculating daily nutrition"));

            // Act
            var result = await _controller.GetDailyNutrition(date);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorResult.StatusCode);
            Assert.Equal("An error occurred while calculating daily nutrition", internalServerErrorResult.Value);
        }

    }
}
