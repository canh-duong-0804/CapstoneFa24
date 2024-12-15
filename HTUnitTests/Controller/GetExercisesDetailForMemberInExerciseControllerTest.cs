using BusinessObject.Dto.Exericse;
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
    public class GetExercisesDetailForMemberInExerciseControllerTest
    {
        private readonly Mock<IExerciseRepository> _mockRepo;
        private readonly ExerciseController _controller;

        public GetExercisesDetailForMemberInExerciseControllerTest()
        {
            _mockRepo = new Mock<IExerciseRepository>();
            _controller = new ExerciseController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetExercisesDetailForMember_NormalCase_ReturnsOk()
        {
            // Arrange
            int exerciseId = 1;
            var mockExerciseDetail = new GetExerciseDetailOfCardiorResponseDTO
            {
                ExerciseId = exerciseId,
                ExerciseName = "Running",
                TypeExercise = 1
            };

            _mockRepo.Setup(repo => repo.GetExercisesCardioDetailForMemberrAsync(exerciseId, It.IsAny<int>()))
                .ReturnsAsync(mockExerciseDetail);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("Id", "1") }))
                }
            };

            // Act
            var result = await _controller.GetExercisesDetailForMember(exerciseId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var objectResult = result as OkObjectResult;
            Assert.Equal(mockExerciseDetail, objectResult?.Value);
        }

        [Fact]
        public async Task GetExercisesDetailForMember_MemberIdNotFound_ReturnsUnauthorized()
        {
            // Arrange
            int exerciseId = 1;
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = await _controller.GetExercisesDetailForMember(exerciseId);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
            var objectResult = result as UnauthorizedObjectResult;
            Assert.Equal("Member ID not found in claims.", objectResult?.Value);
        }

        [Fact]
        public async Task GetExercisesDetailForMember_InvalidMemberId_ReturnsBadRequest()
        {
            // Arrange
            int exerciseId = 1;
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("Id", "invalid") }))
                }
            };

            // Act
            var result = await _controller.GetExercisesDetailForMember(exerciseId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = result as BadRequestObjectResult;
            Assert.Equal("Invalid member ID.", objectResult?.Value);
        }

        [Fact]
        public async Task GetExercisesDetailForMember_NoExercisesFound_ReturnsNotFound()
        {
            // Arrange
            int exerciseId = 1;
            _mockRepo.Setup(repo => repo.GetExercisesCardioDetailForMemberrAsync(exerciseId, It.IsAny<int>()))
                .ReturnsAsync((GetExerciseDetailOfCardiorResponseDTO)null);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("Id", "1") }))
                }
            };

            // Act
            var result = await _controller.GetExercisesDetailForMember(exerciseId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            var objectResult = result as NotFoundObjectResult;
            Assert.Equal("No exercises found for the member.", objectResult?.Value);
        }

        [Fact]
        public async Task GetExercisesDetailForMember_BoundaryCase_ExerciseIdZero_ReturnsNotFound()
        {
            // Arrange
            int exerciseId = 0;
            _mockRepo.Setup(repo => repo.GetExercisesCardioDetailForMemberrAsync(exerciseId, It.IsAny<int>()))
                .ReturnsAsync((GetExerciseDetailOfCardiorResponseDTO)null);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("Id", "1") }))
                }
            };

            // Act
            var result = await _controller.GetExercisesDetailForMember(exerciseId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
