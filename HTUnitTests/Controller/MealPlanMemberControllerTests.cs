using BusinessObject.Dto.MealPlan;
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
    public class MealPlanMemberControllerTests
    {
        private readonly Mock<IMealPlanRepository> _mockMealPlanRepository;
        private readonly MealPlanMemberController _controller;

        public MealPlanMemberControllerTests()
        {
            // Setup mocks
            _mockMealPlanRepository = new Mock<IMealPlanRepository>();

            // Create controller
            _controller = new MealPlanMemberController(_mockMealPlanRepository.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        // Helper method to setup authorized user
        private void SetupAuthorizedUser(string userId = "1")
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("Id", userId)
            }));
            _controller.ControllerContext.HttpContext.User = user;
        }

        // Helper method to setup unauthorized user
        private void SetupUnauthorizedUser()
        {
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal();
        }

        #region GetAllMealPlansForMember Tests
        [Fact]
        public async Task GetAllMealPlansForMember_ReturnsOk_WhenMealPlansExist()
        {
            // Arrange
            var mealPlans = new List<GetAllMealPlanForMemberResponseDTO>
            {
                new GetAllMealPlanForMemberResponseDTO { MealPlanId = 1, Name = "Plan 1" },
                new GetAllMealPlanForMemberResponseDTO { MealPlanId = 2, Name = "Plan 2" }
            };
            _mockMealPlanRepository
                .Setup(repo => repo.GetAllMealPlansForMemberAsync())
                .ReturnsAsync(mealPlans);

            // Act
            var result = await _controller.GetAllMealPlansForMember();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<GetAllMealPlanForMemberResponseDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetAllMealPlansForMember_ReturnsNotFound_WhenNoMealPlansExist()
        {
            // Arrange
            // Arrange
            _mockMealPlanRepository
                .Setup(repo => repo.GetAllMealPlansForMemberAsync())
                .ReturnsAsync(new List<GetAllMealPlanForMemberResponseDTO>());

            // Act
            var result = await _controller.GetAllMealPlansForMember();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No meal plans found for this member.", notFoundResult.Value);
        }
        #endregion

       

        [Fact]
        public async Task AddMealPlanToDiary_ReturnsUnauthorized_WhenUserIsNotAuthorized()
        {
            SetupUnauthorizedUser();

            var result = await _controller.AddMealPlanToDiary(1, DateTime.UtcNow);

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task AddMealPlanToDiary_ReturnsBadRequest_WhenMemberIdIsInvalid()
        {
            SetupAuthorizedUser("invalid_id");

            var result = await _controller.AddMealPlanToDiary(1, DateTime.UtcNow);

            Assert.IsType<BadRequestResult>(result);
        }
       

        [Fact]
        public async Task GetMealPlanDetailForMember_ReturnsOk_WhenMealPlanDetailExists()
        {
            _mockMealPlanRepository.Setup(repo => repo.GetMealPlanDetailForMemberAsync(1, 1))
                .ReturnsAsync(new MealPlanDetailResponseDTO());

            var result = await _controller.GetMealPlanDetailForMember(1, 1);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetMealPlanDetailForMember_ReturnsNotFound_WhenMealPlanDetailDoesNotExist()
        {
            _mockMealPlanRepository.Setup(repo => repo.GetMealPlanDetailForMemberAsync(1, 1))
                .ReturnsAsync((MealPlanDetailResponseDTO)null);

            var result = await _controller.GetMealPlanDetailForMember(1, 1);

            Assert.IsType<NotFoundResult>(result);
        }


        

        [Fact]
        public async Task AddMealPlanDetailWithDayToFoodDiary_ReturnsBadRequest_WhenRepositoryFails()
        {
            SetupAuthorizedUser();

            _mockMealPlanRepository.Setup(repo => repo.AddMealPlanDetailWithDayToFoodDiaryAsync(It.IsAny<AddMealPlanDetailDayToFoodDiaryDetailRequestDTO>(), 1))
                .ReturnsAsync(false);

            var request = new AddMealPlanDetailDayToFoodDiaryDetailRequestDTO();

            var result = await _controller.AddMealPlanDetailWithDayToFoodDiary(request);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task AddMealPlanToDiary_ReturnsOkWithEmptyString_WhenSuccess()
        {
            // Arrange
            SetupAuthorizedUser();
            _mockMealPlanRepository.Setup(repo => repo.AddMealPlanToFoodDiaryAsync(1, 1, It.IsAny<DateTime>()))
                .ReturnsAsync(1); // Success case returns 1

            // Act
            var result = await _controller.AddMealPlanToDiary(1, DateTime.UtcNow);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("", okResult.Value);
        }

        [Fact]
        public async Task AddMealPlanToDiary_ReturnsStatusCode500_WhenRepositoryReturns2()
        {
            // Arrange
            SetupAuthorizedUser();
            _mockMealPlanRepository.Setup(repo => repo.AddMealPlanToFoodDiaryAsync(1, 1, It.IsAny<DateTime>()))
                .ReturnsAsync(2); // Failure case returns 2

            // Act
            var result = await _controller.AddMealPlanToDiary(1, DateTime.UtcNow);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task AddMealPlanToDiary_ReturnsBadRequest_WhenRepositoryReturns3()
        {
            // Arrange
            SetupAuthorizedUser();
            _mockMealPlanRepository.Setup(repo => repo.AddMealPlanToFoodDiaryAsync(1, 1, It.IsAny<DateTime>()))
                .ReturnsAsync(3); // Bad request case returns 3

            // Act
            var result = await _controller.AddMealPlanToDiary(1, DateTime.UtcNow);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
