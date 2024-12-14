using BusinessObject.Dto.Food;
using BusinessObject.Dto.FoodDiary;
using BusinessObject.Dto.FoodDiaryDetails;
using BusinessObject.Dto.Streak;
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
    public class FoodDiaryControllerTests
    {
        private readonly Mock<IFoodDiaryRepository> _mockFoodDiaryRepository;
        private readonly FoodDiaryController _controller;
        private const int TestMemberId = 1;
        private const string TestMemberIdString = "1";

        public FoodDiaryControllerTests()
        {
            _mockFoodDiaryRepository = new Mock<IFoodDiaryRepository>();
            _controller = new FoodDiaryController(_mockFoodDiaryRepository.Object);

            // Setup user claims
            var claims = new List<Claim>
            {
                new Claim("Id", TestMemberIdString)
            };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };
        }

        [Fact]
        public async Task AddFoodListToDiaryForWebsite_RepositoryReturnsFalse_ReturnsNotFound()
        {
            // Arrange
            var request = new AddFoodDiaryDetailForWebsiteRequestDTO
            {
                MealType = 1,
                selectDate = DateTime.Now,
                ListFoodIdToAdd = new List<FoodDiaryDetailForWebisteRequestDTO>
        {
            new FoodDiaryDetailForWebisteRequestDTO
            {
                FoodId = 1,
                Quantity = 1.0,
                Portion = "serving"
            }
        }
            };
            _mockFoodDiaryRepository
                .Setup(x => x.addFoodListToDiaryForWebsite(request, TestMemberId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.addFoodListToDiaryForWebsite(request);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task AddFoodListToDiaryForWebsite_NullRequest_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.addFoodListToDiaryForWebsite(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteFoodDiaryWebsite_RepositoryReturnsFalse_ReturnsNotFound()
        {
            // Arrange
            var selectDate = DateTime.Now;
            const int mealType = 1;
            _mockFoodDiaryRepository
                .Setup(x => x.DeleteFoodDiaryWebsite(selectDate, TestMemberId, mealType))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteFoodDiaryWebsite(selectDate, mealType);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetFoodDairyDetailWebsite_NoMemberIdClaim_ReturnsUnauthorized()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity()) // No claims
                }
            };

            // Act
            var result = await _controller.GetFoodDairyDetailWebsite(DateTime.Now, 1);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task GetFoodDairyDetailWebsite_InvalidMemberIdClaim_ReturnsBadRequest()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                new Claim("Id", "invalid")
            }))
                }
            };

            // Act
            var result = await _controller.GetFoodDairyDetailWebsite(DateTime.Now, 1);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetFoodDairyDetailWebsite_RepositoryReturnsNull_ReturnsNotFound()
        {
            // Arrange
            var selectDate = DateTime.Now;
            const int mealType = 1;
            _mockFoodDiaryRepository
                .Setup(x => x.GetFoodDairyDetailWebsite(TestMemberId, selectDate, mealType))
                .ReturnsAsync((AddFoodDiaryDetailForWebsiteRequestDTO)null);

            // Act
            var result = await _controller.GetFoodDairyDetailWebsite(selectDate, mealType);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetAllDiariesForMonthWithMealTypes_NoMemberIdClaim_ReturnsUnauthorized()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity()) // No claims
                }
            };

            // Act
            var result = await _controller.GetAllDiariesForMonthWithMealTypes(DateTime.Now);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task GetAllDiariesForMonthWithMealTypes_InvalidMemberIdClaim_ReturnsBadRequest()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                new Claim("Id", "invalid")
            }))
                }
            };

            // Act
            var result = await _controller.GetAllDiariesForMonthWithMealTypes(DateTime.Now);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetAllDiariesForMonthWithMealTypes_NoDiaries_ReturnsNotFound()
        {
            // Arrange
            var date = DateTime.Now;
            _mockFoodDiaryRepository
                .Setup(x => x.GetAllDiariesForMonthWithMealTypesAsync(date, TestMemberId))
                .ReturnsAsync(new List<FoodDiaryWithMealTypeDTO>());

            // Act
            var result = await _controller.GetAllDiariesForMonthWithMealTypes(date);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetFoodHistory_NoMemberIdClaim_ReturnsUnauthorized()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity()) // No claims
                }
            };

            // Act
            var result = await _controller.GetFoodHistory();

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task GetFoodHistory_InvalidMemberIdClaim_ReturnsBadRequest()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                new Claim("Id", "invalid")
            }))
                }
            };

            // Act
            var result = await _controller.GetFoodHistory();

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetCalorieStreak_NoMemberIdClaim_ReturnsUnauthorized()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity()) // No claims
                }
            };

            // Act
            var result = await _controller.GetCalorieStreak(DateTime.Now);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task GetCalorieStreak_InvalidMemberIdClaim_ReturnsBadRequest()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                new Claim("Id", "invalid")
            }))
                }
            };

            // Act
            var result = await _controller.GetCalorieStreak(DateTime.Now);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetCalorieStreak_NoStreakFound_ReturnsNotFound()
        {
            // Arrange
            var date = DateTime.Now;
            _mockFoodDiaryRepository
                .Setup(x => x.GetCalorieStreakAsync(TestMemberId, date))
                .ReturnsAsync((CalorieStreakDTO)null);

            // Act
            var result = await _controller.GetCalorieStreak(date);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

    }
}
