using BusinessObject.Dto.ExecriseDiary;
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
    public class GetAllExercisesFilterInExerciseControllerTest
    {
        private readonly Mock<IExerciseRepository> _mockRepo;
        private readonly ExerciseController _controller;

        public GetAllExercisesFilterInExerciseControllerTest()
        {
            _mockRepo = new Mock<IExerciseRepository>();
            _controller = new ExerciseController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllExercisesFilter_NormalCase_ReturnsOk()
        {
            // Arrange
            var mockExercises = new List<GetAllExerciseFilterForMember>
            {
                new GetAllExerciseFilterForMember { ExerciseId = 1, ExerciseName = "Running", TypeExercise = 1 },
                new GetAllExerciseFilterForMember { ExerciseId = 2, ExerciseName = "Weightlifting", TypeExercise = 2 }
            };

            _mockRepo.Setup(repo => repo.GetAllExercisesFilterAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int>()))
                .ReturnsAsync(mockExercises);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("Id", "1") }))
                }
            };

            // Act
            var result = await _controller.GetAllExercisesFilter(null, null);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var objectResult = result as OkObjectResult;
            Assert.Equal(mockExercises, objectResult?.Value);
        }

        [Fact]
        public async Task GetAllExercisesFilter_MemberIdNotFound_ReturnsUnauthorized()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = await _controller.GetAllExercisesFilter(null, null);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
            var objectResult = result as UnauthorizedObjectResult;
            Assert.Equal("Member ID not found in claims.", objectResult?.Value);
        }

        [Fact]
        public async Task GetAllExercisesFilter_InvalidMemberId_ReturnsBadRequest()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("Id", "invalid") }))
                }
            };

            // Act
            var result = await _controller.GetAllExercisesFilter(null, null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = result as BadRequestObjectResult;
            Assert.Equal("Invalid member ID.", objectResult?.Value);
        }

        [Fact]
        public async Task GetAllExercisesFilter_NoExercisesFound_ReturnsNotFound()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllExercisesFilterAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int>()))
                .ReturnsAsync((List<GetAllExerciseFilterForMember>)null);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("Id", "1") }))
                }
            };

            // Act
            var result = await _controller.GetAllExercisesFilter(null, null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData(1, "Running")]
        [InlineData(2, "Weightlifting")]
        [InlineData(3, "Yoga")]
        public async Task GetAllExercisesFilter_FilterByType_ReturnsOk(int isCardioFilter, string expectedName)
        {
            // Arrange
            var mockExercises = new List<GetAllExerciseFilterForMember>
            {
                new GetAllExerciseFilterForMember { ExerciseId = 1, ExerciseName = expectedName, TypeExercise = isCardioFilter }
            };

            _mockRepo.Setup(repo => repo.GetAllExercisesFilterAsync(It.IsAny<string>(), isCardioFilter, It.IsAny<int>()))
                .ReturnsAsync(mockExercises);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("Id", "1") }))
                }
            };

            // Act
            var result = await _controller.GetAllExercisesFilter(null, isCardioFilter);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var objectResult = result as OkObjectResult;
            Assert.Equal(mockExercises, objectResult?.Value);
        }
    }
}

  