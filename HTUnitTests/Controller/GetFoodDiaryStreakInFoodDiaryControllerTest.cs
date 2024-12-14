/*using BusinessObject.Dto.Streak;
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
    public class GetFoodDiaryStreakInFoodDiaryControllerTest
    {
        private Mock<IFoodDiaryRepository> _mockFoodDiaryRepository;
        private FoodDiaryController _controller;

        public GetFoodDiaryStreakInFoodDiaryControllerTest()
        {
            _mockFoodDiaryRepository = new Mock<IFoodDiaryRepository>();
        }

        // UTCID01: Successful retrieval of calorie streak
        [Fact]
        public async Task GetCalorieStreak_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var memberId = 1;
            var date = new DateTime(2024, 12, 9);
            var expectedStreak = new CalorieStreakDTO { *//* populate with test data *//* };

            // Setup the controller with claims
            var controller = SetupControllerWithClaims(memberId.ToString());

            // Setup repository mock
            _mockFoodDiaryRepository
                .Setup(repo => repo.GetCalorieStreakAsync(memberId, date))
                .ReturnsAsync(expectedStreak);

            // Act
            var result = await controller.GetCalorieStreak(date);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedStreak, okResult.Value);
        }

        // UTCID02: Unauthorized - No member ID in claims
        [Fact]
        public async Task GetCalorieStreak_NoMemberIdInClaims_ReturnsUnauthorized()
        {
            // Arrange
            var date = new DateTime(2024, 12, 9);
            var controller = SetupControllerWithClaims(null);

            // Act
            var result = await controller.GetCalorieStreak(date);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Member ID not found in claims.", unauthorizedResult.Value);
        }

        // UTCID03: Bad Request - Invalid member ID
        [Fact]
        public async Task GetCalorieStreak_InvalidMemberId_ReturnsBadRequest()
        {
            // Arrange
            var date = new DateTime(2024, 12, 9);
            var controller = SetupControllerWithClaims("invalid");

            // Act
            var result = await controller.GetCalorieStreak(date);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid member ID.", badRequestResult.Value);
        }

        // UTCID04: Not Found - No calorie streak found
        [Fact]
        public async Task GetCalorieStreak_NoStreakFound_ReturnsNotFound()
        {
            // Arrange
            var memberId = 1;
            var date = new DateTime(2024, 12, 9);
            var controller = SetupControllerWithClaims(memberId.ToString());

            // Setup repository mock to return null
            _mockFoodDiaryRepository
                .Setup(repo => repo.GetCalorieStreakAsync(memberId, date))
                .ReturnsAsync((CalorieStreakDTO)null);

            // Act
            var result = await controller.GetCalorieStreak(date);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No calorie streak found.", notFoundResult.Value);
        }

        // Boundary Test - Null Date
        [Fact]
        public async Task GetCalorieStreak_NullDate_ReturnsValidResult()
        {
            // Arrange
            var memberId = 1;
            var controller = SetupControllerWithClaims(memberId.ToString());

            // Setup repository mock
            _mockFoodDiaryRepository
                .Setup(repo => repo.GetCalorieStreakAsync(memberId, It.IsAny<DateTime>()))
                .ReturnsAsync(new CalorieStreakDTO { *//* populate with test data *//* });

            // Act
            var result = await controller.GetCalorieStreak(default);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        // Helper method to set up controller with claims
        private FoodDiaryController SetupControllerWithClaims(string memberId)
        {
            var controller = new FoodDiaryController(_mockFoodDiaryRepository.Object);

            var claims = new List<Claim>();
            if (memberId != null)
            {
                claims.Add(new Claim("Id", memberId));
            }

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(claims))
                }
            };

            return controller;
        }
    }
}

*/