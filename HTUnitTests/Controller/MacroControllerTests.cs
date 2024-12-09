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
	public class MacroControllerTests
	{
		private readonly Mock<IMacroRepository> _mockMacroRepository;
		private readonly MacroController _controller;

		public MacroControllerTests()
		{
			_mockMacroRepository = new Mock<IMacroRepository>();
			_controller = new MacroController(_mockMacroRepository.Object);
		}

		[Fact]
		public async Task GetDailyMacros_ValidMemberAndDate_ReturnsOkResult()
		{
			// Arrange
			int memberId = 1;
			DateTime date = DateTime.Now.Date;
			var expectedMacros = new MacroNutrientDto
			{
				TotalCarbs = 50,
				TotalFat = 20,
				TotalProtein = 30,
				HighestCarbFood = new FoodMacroDto
				{
					FoodName = "Oats",
					Quantity = 100,
					MacroValue = 25
				},
				HighestFatFood = new FoodMacroDto
				{
					FoodName = "Avocado",
					Quantity = 50,
					MacroValue = 15
				},
				HighestProteinFood = new FoodMacroDto
				{
					FoodName = "Chicken",
					Quantity = 150,
					MacroValue = 30
				}
			};

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
			_mockMacroRepository
				.Setup(repo => repo.GetMacroNutrientsByDate(memberId, date))
				.ReturnsAsync(expectedMacros);

			// Act
			var result = await _controller.GetDailyMacros(date);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal(expectedMacros, okResult.Value);
		}

		[Fact]
		public async Task GetDailyMacros_MissingMemberId_ReturnsUnauthorized()
		{
			// Arrange
			DateTime date = DateTime.Now.Date;

			// Setup user claims without the "Id" claim
			var claims = new[] { new Claim("SomeOtherClaim", "value") };
			var identity = new ClaimsIdentity(claims, "TestAuthentication");
			var claimsPrincipal = new ClaimsPrincipal(identity);
			_controller.ControllerContext = new ControllerContext
			{
				HttpContext = new DefaultHttpContext { User = claimsPrincipal }
			};

			// Act
			var result = await _controller.GetDailyMacros(date);

			// Assert
			var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
			Assert.Equal("Member ID not found in claims.", unauthorizedResult.Value);
		}

		[Fact]
		public async Task GetDailyMacros_NoMacrosFound_ReturnsNotFound()
		{
			// Arrange
			int memberId = 1;
			DateTime date = DateTime.Now.Date;
			MacroNutrientDto? expectedMacros = null;

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

			// Setup repository mock to return null (no macros found)
			_mockMacroRepository
				.Setup(repo => repo.GetMacroNutrientsByDate(memberId, date))
				.ReturnsAsync(expectedMacros);

			// Act
			var result = await _controller.GetDailyMacros(date);

			// Assert
			var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
			Assert.Equal("No daily macros found for the specified member and date.", notFoundResult.Value);
		}

		[Fact]
		public async Task GetDailyMacros_Exception_ReturnsInternalServerError()
		{
			// Arrange
			int memberId = 1;
			DateTime date = DateTime.Now.Date;

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

			// Setup repository mock to throw an exception
			_mockMacroRepository
				.Setup(repo => repo.GetMacroNutrientsByDate(memberId, date))
				.ThrowsAsync(new Exception("Database error"));

			// Act
			var result = await _controller.GetDailyMacros(date);

			// Assert
			var statusCodeResult = Assert.IsType<ObjectResult>(result);
			Assert.Equal(500, statusCodeResult.StatusCode);
			Assert.Equal("Database error", statusCodeResult.Value);
		}
	}
}
