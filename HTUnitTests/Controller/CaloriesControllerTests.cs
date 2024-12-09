using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Repository.IRepo;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using HealthTrackingManageAPI.Controllers;
using BusinessObject.Dto.Nutrition;


namespace HTUnitTests.Controller
{
	public class CaloriesControllerTests
	{
		private readonly Mock<ICaloriesRepository> _caloriesRepositoryMock;
		private readonly CaloriesController _controller;

		public CaloriesControllerTests()
		{
			_caloriesRepositoryMock = new Mock<ICaloriesRepository>();
			_controller = new CaloriesController(_caloriesRepositoryMock.Object);

			// Set up a mock user with claims for the controller
			var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
			{
				new Claim("Id", "1")
			}, "mock"));

			_controller.ControllerContext = new ControllerContext
			{
				HttpContext = new DefaultHttpContext { User = user }
			};
		}

		[Fact]
		public async Task GetDailyCalories_ValidRequest_ReturnsOkResult()
		{
			// Arrange
			var date = DateTime.UtcNow.Date;
			var dailyCaloriesDto = new DailyCaloriesDto
			{
				TotalCalories = 1500,
				NetCalories = 1200,
				GoalCalories = 2000
			};

			_caloriesRepositoryMock
				.Setup(repo => repo.CalculateDailyCalories(It.IsAny<int>(), It.IsAny<DateTime>()))
				.ReturnsAsync(dailyCaloriesDto);

			// Act
			var result = await _controller.GetDailyCalories(date);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			var returnedDto = Assert.IsType<DailyCaloriesDto>(okResult.Value);
			Assert.Equal(dailyCaloriesDto.TotalCalories, returnedDto.TotalCalories);
		}

		[Fact]
		public async Task GetDailyCalories_MemberIdNotFound_ReturnsUnauthorizedResult()
		{
			// Arrange
			_controller.ControllerContext.HttpContext.User = new ClaimsPrincipal();

			// Act
			var result = await _controller.GetDailyCalories(DateTime.UtcNow.Date);

			// Assert
			var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
			Assert.Equal("Member ID not found in claims.", unauthorizedResult.Value);
		}

		[Fact]
		public async Task GetDailyCalories_NoDataFound_ReturnsNotFoundResult()
		{
			// Arrange
			var date = DateTime.UtcNow.Date;

			_caloriesRepositoryMock
				.Setup(repo => repo.CalculateDailyCalories(It.IsAny<int>(), It.IsAny<DateTime>()))
				.ReturnsAsync((DailyCaloriesDto)null);

			// Act
			var result = await _controller.GetDailyCalories(date);

			// Assert
			var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
			Assert.Equal("No daily calories found for the specified member and date.", notFoundResult.Value);
		}

		[Fact]
		public async Task GetDailyCalories_RepositoryThrowsException_ReturnsInternalServerError()
		{
			
			var date = DateTime.UtcNow.Date;

			_caloriesRepositoryMock
				.Setup(repo => repo.CalculateDailyCalories(It.IsAny<int>(), It.IsAny<DateTime>()))
				.ThrowsAsync(new Exception("Database error"));

			// Act
			var result = await _controller.GetDailyCalories(date);

			// Assert
			var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
			Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorResult.StatusCode);
			Assert.Equal("Database error", internalServerErrorResult.Value);
		}
	}
}
