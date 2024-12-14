using BusinessObject.Dto.MealDetailMember;
using BusinessObject.Dto.MealMember;
using BusinessObject.Dto.MealPlanDetailMember;
using BusinessObject.Models;
using DataAccess;
using HealthTrackingManageAPI.Controllers;
using HealthTrackingManageAPI.NewFolder.Image;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace HTUnitTests.Controller
{
    public class MealMemberControllerTests
    {
        private readonly Mock<IMealMemberRepository> _mockMealMemberRepo;
        private readonly Mock<CloudinaryService> _mockCloudinaryService;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<ILogger<MealMemberController>> _mockLogger;
        private readonly MealMemberController _controller;

        public MealMemberControllerTests()
        {
            // Setup mocks
            _mockMealMemberRepo = new Mock<IMealMemberRepository>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<MealMemberController>>();

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

            // Create controller with mocked dependencies
            _controller = new MealMemberController(_mockMealMemberRepo.Object, _mockCloudinaryService.Object)
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

        #region GetAllMealMembers Tests
        [Fact]
        public async Task GetAllMealMembers_ReturnsOk_WhenMealMembersExist()
        {
            // Arrange
            SetupAuthorizedUser();
            var mealMembers = new List<MealMember>
            {
                new MealMember { MealMemberId = 1, NameMealMember = "Meal 1" },
                new MealMember { MealMemberId = 2, NameMealMember = "Meal 2" }
            };
            _mockMealMemberRepo
                .Setup(repo => repo.GetAllMealMembersAsync(It.IsAny<int>()))
                .ReturnsAsync(mealMembers);

            // Act
            var result = await _controller.GetAllMealMembers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<GetAllMealMemberResonseDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetAllMealMembers_ReturnsNotFound_WhenNoMealMembersExist()
        {
            // Arrange
            SetupAuthorizedUser();
            _mockMealMemberRepo
                .Setup(repo => repo.GetAllMealMembersAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<MealMember>());

            // Act
            var result = await _controller.GetAllMealMembers();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetAllMealMembers_ReturnsUnauthorized_WhenNoUserIdClaim()
        {
            // Arrange
            SetupUnauthorizedUser();

            // Act
            var result = await _controller.GetAllMealMembers();

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }
        #endregion

        #region UploadImageMeal Tests
       /* [Fact]
        public async Task UploadImageMeal_ReturnsOk_WhenImageUploadSuccessful()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.Length).Returns(1);

            var uploadResult = new CloudinaryUploadResult
            {
                Url = new System.Uri("https://example.com/image.jpg")
            };

            _mockCloudinaryService
                .Setup(s => s.UploadImageAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
                .ReturnsAsync(uploadResult);

            _mockMealMemberRepo
                .Setup(repo => repo.UploadImageForMealMember(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.UploadImageMeal(mockFile.Object, 1);

            // Assert
            Assert.IsType<OkResult>(result);
        }*/

       /* [Fact]
        public async Task UploadImageMeal_ReturnsBadRequest_WhenImageUploadFails()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.Length).Returns(1);

            _mockCloudinaryService
                .Setup(s => s.UploadImageAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
                .ThrowsAsync(new System.Exception("Upload failed"));

            // Act
            var result = await _controller.UploadImageMeal(mockFile.Object, 1);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }*/

        [Fact]
        public async Task UploadImageMeal_ReturnsBadRequest_WhenNoImage()
        {
            // Act
            var result = await _controller.UploadImageMeal(null, 1);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
        #endregion

        #region CreateMealForMember Tests
        [Fact]
        public async Task CreateMealForMember_ReturnsOk_WhenValidInput()
        {
            // Arrange
            SetupAuthorizedUser();
            var mealMemberDto = new CreateMealMemberRequestDTO
            {
                MealDetails = new List<CreateMealDetailMemberRequestDTO>
                {
                    new CreateMealDetailMemberRequestDTO
                    {
                        FoodId = 1,
                        Quantity = 1
                    }
                }
            };

            _mockMealMemberRepo
                .Setup(repo => repo.CreateMealMemberAsync(It.IsAny<MealMember>()))
                .ReturnsAsync(1);

            _mockMealMemberRepo
                .Setup(repo => repo.CreateMealMemberDetailsAsync(It.IsAny<List<MealMemberDetail>>()))
                .Returns(Task.CompletedTask);

            _mockMealMemberRepo
                .Setup(repo => repo.UpdateMealMemberTotalCaloriesAsync(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateMealForMember(mealMemberDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(1, okResult.Value);
        }

        [Fact]
        public async Task CreateMealForMember_ReturnsBadRequest_WhenNoInput()
        {
            // Arrange
            SetupAuthorizedUser();

            // Act
            var result = await _controller.CreateMealForMember(null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task CreateMealForMember_ReturnsUnauthorized_WhenNoUserIdClaim()
        {
            // Arrange
            SetupUnauthorizedUser();
            var mealMemberDto = new CreateMealMemberRequestDTO();

            // Act
            var result = await _controller.CreateMealForMember(mealMemberDto);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }
        #endregion

        #region GetMealMemberDetail Tests
        [Fact]
        public async Task GetMealMemberDetail_ReturnsOk_WhenMealMemberExists()
        {
            // Arrange
            int mealMemberId = 1;
            var mealMemberDetail = new MealMemberDetailResonseDTO
            {
                MealPlanId = mealMemberId
            };

            _mockMealMemberRepo
                .Setup(repo => repo.GetMealMemberDetailAsync(mealMemberId))
                .ReturnsAsync(mealMemberDetail);

            // Act
            var result = await _controller.GetMealMemberDetail(mealMemberId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<MealMemberDetailResonseDTO>(okResult.Value);
            Assert.Equal(mealMemberId, returnValue.MealPlanId);
        }

        [Fact]
        public async Task GetMealMemberDetail_ReturnsNotFound_WhenMealMemberDoesNotExist()
        {
            // Arrange
            int mealMemberId = 1;

            _mockMealMemberRepo
                .Setup(repo => repo.GetMealMemberDetailAsync(mealMemberId))
                .ReturnsAsync((MealMemberDetailResonseDTO)null);

            // Act
            var result = await _controller.GetMealMemberDetail(mealMemberId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        #endregion

        #region AddMealMemberToDiary Tests
        [Fact]
        public async Task AddMealMemberToDiary_ReturnsOk_WhenSuccessful()
        {
            // Arrange
            SetupAuthorizedUser();
            var addMealMemberDto = new AddMealMemberToFoodDiaryDetailRequestDTO();

            _mockMealMemberRepo
                .Setup(repo => repo.AddMealMemberToDiaryDetailAsync(It.IsAny<AddMealMemberToFoodDiaryDetailRequestDTO>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.AddMealMemberToDiary(addMealMemberDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Meal successfully added to the diary.", okResult.Value);
        }

        [Fact]
        public async Task AddMealMemberToDiary_ReturnsServerError_WhenFailed()
        {
            // Arrange
            SetupAuthorizedUser();
            var addMealMemberDto = new AddMealMemberToFoodDiaryDetailRequestDTO();

            _mockMealMemberRepo
                .Setup(repo => repo.AddMealMemberToDiaryDetailAsync(It.IsAny<AddMealMemberToFoodDiaryDetailRequestDTO>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.AddMealMemberToDiary(addMealMemberDto);

            // Assert
            Assert.IsType<StatusCodeResult>(result);
        }

        [Fact]
        public async Task AddMealMemberToDiary_ReturnsUnauthorized_WhenNoUserIdClaim()
        {
            // Arrange
            SetupUnauthorizedUser();
            var addMealMemberDto = new AddMealMemberToFoodDiaryDetailRequestDTO();

            // Act
            var result = await _controller.AddMealMemberToDiary(addMealMemberDto);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }
        #endregion

        #region DeleteMealMemberDetail Tests
        [Fact]
        public async Task DeleteMealMemberDetail_ReturnsOk_WhenSuccessful()
        {
            // Arrange
            int detailId = 1;

            _mockMealMemberRepo
                .Setup(repo => repo.DeleteMealMemberDetailAsync(detailId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteMealMemberDetail(detailId);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteMealMemberDetail_ReturnsNotFound_WhenFailed()
        {
            // Arrange
            int detailId = 1;

            _mockMealMemberRepo
                .Setup(repo => repo.DeleteMealMemberDetailAsync(detailId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteMealMemberDetail(detailId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        #endregion

        #region AddMultipleFoodsToMealMember Tests
        [Fact]
        public async Task AddMultipleFoodsToMealMember_ReturnsOk_WhenValidInput()
        {
            // Arrange
            SetupAuthorizedUser();
            var addFoodDto = new AddMoreFoodToMealMemberRequestDTO
            {
                mealMemberId = 1,
                MealDetails = new List<CreateMealDetailMemberRequestDTO>
                {
                    new CreateMealDetailMemberRequestDTO
                    {
                        FoodId = 1,
                        Quantity = 1
                    }
                }
            };

            _mockMealMemberRepo
                .Setup(repo => repo.CreateMealMemberDetailsAsync(It.IsAny<List<MealMemberDetail>>()))
                .Returns(Task.CompletedTask);

            _mockMealMemberRepo
                .Setup(repo => repo.UpdateMealMemberTotalCaloriesAsync(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddMultipleFoodsToMealMember(addFoodDto);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task AddMultipleFoodsToMealMember_ReturnsBadRequest_WhenNoInput()
        {
            // Arrange
            SetupAuthorizedUser();

            // Act
            var result = await _controller.AddMultipleFoodsToMealMember(null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task AddMultipleFoodsToMealMember_ReturnsBadRequest_WhenEmptyMealDetails()
        {
            // Arrange
            SetupAuthorizedUser();
            var addFoodDto = new AddMoreFoodToMealMemberRequestDTO
            {
                mealMemberId = 1,
                MealDetails = new List<CreateMealDetailMemberRequestDTO>()
            };

            // Act
            var result = await _controller.AddMultipleFoodsToMealMember(addFoodDto);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task AddMultipleFoodsToMealMember_ReturnsUnauthorized_WhenNoUserIdClaim()
        {
            // Arrange
            SetupUnauthorizedUser();
            var addFoodDto = new AddMoreFoodToMealMemberRequestDTO
            {
                mealMemberId = 1,
                MealDetails = new List<CreateMealDetailMemberRequestDTO>
                {
                    new CreateMealDetailMemberRequestDTO
                    {
                        FoodId = 1,
                        Quantity = 1
                    }
                }
            };

            // Act
            var result = await _controller.AddMultipleFoodsToMealMember(addFoodDto);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }
        #endregion


        public class CloudinarySettings
        {
            public string CloudName { get; set; }
            public string ApiKey { get; set; }
            public string ApiSecret { get; set; }
        }
    }
}