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
using Xunit;

namespace HTUnitTests.Controller
{
    public class GetAllExerciseInExercisesControllerTests
    {
        [Fact]
        public async Task GetAllExercises_Unauthorized_Returns401()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var controller = new ExerciseController(mockRepo.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = await controller.GetAllExercises(null, null);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
            var objectResult = result as UnauthorizedObjectResult;
            Assert.Equal("Member ID not found in claims.", objectResult?.Value);
        }

        [Fact]
        public async Task GetAllExercises_InvalidMemberId_Returns400()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var controller = new ExerciseController(mockRepo.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("Id", "invalid") }))
                }
            };

            // Act
            var result = await controller.GetAllExercises(null, null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = result as BadRequestObjectResult;
            Assert.Equal("Invalid member ID.", objectResult?.Value);
        }

        [Fact]
        public async Task GetAllExercises_NoExercisesFound_Returns404()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            mockRepo.Setup(repo => repo.GetAllExercisesForMemberAsync(null, null, It.IsAny<int>()))
                .ReturnsAsync(new List<GetAllExerciseForMember>()); // Return an empty list to simulate no exercises found.

            var controller = new ExerciseController(mockRepo.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("Id", "1") }))
                }
            };

            // Act
            var result = await controller.GetAllExercises(null, null);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAllExercises_ValidRequest_Returns200()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var fakeExercises = new List<GetAllExerciseForMember>
{
    new GetAllExerciseForMember { ExerciseId = 1, ExerciseName = "Running", TypeExercise = 1 },
    new GetAllExerciseForMember { ExerciseId = 2, ExerciseName = "Weightlifting", TypeExercise = 2 }
};

            mockRepo.Setup(repo => repo.GetAllExercisesForMemberAsync(null, null, It.IsAny<int>()))
                .ReturnsAsync(fakeExercises); // Match the type to Task<List<GetAllExerciseForMember>>


            var controller = new ExerciseController(mockRepo.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("Id", "1") }))
                }
            };

            // Act
            var result = await controller.GetAllExercises(null, null);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var objectResult = result as OkObjectResult;
            Assert.Equal(fakeExercises, objectResult?.Value);
        }

        [Theory]
        [InlineData(1, "Running")]
        [InlineData(2, "Weightlifting")]
        [InlineData(3, "Yoga")]
        public async Task GetAllExercises_FilteredByType_ReturnsFilteredExercises(int typeExercise, string expectedName)
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var fakeExercises = new List<GetAllExerciseForMember>
    {
        new GetAllExerciseForMember { ExerciseId = 1, ExerciseName = expectedName, TypeExercise = typeExercise }
    };

            mockRepo.Setup(repo => repo.GetAllExercisesForMemberAsync(null, typeExercise, It.IsAny<int>()))
                .ReturnsAsync(fakeExercises); // Ensure the mocked return type matches the repository signature.

            var controller = new ExerciseController(mockRepo.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("Id", "1") }))
                }
            };

            // Act
            var result = await controller.GetAllExercises(null, typeExercise);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var objectResult = result as OkObjectResult;
            Assert.Equal(fakeExercises, objectResult?.Value);
        }

        [Fact]
        public async Task GetAllExercises_EmptySearch_ReturnsAllExercises()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var fakeExercises = new List<GetAllExerciseForMember>
    {
        new GetAllExerciseForMember { ExerciseId = 1, ExerciseName = "Running", TypeExercise = 1 },
        new GetAllExerciseForMember { ExerciseId = 2, ExerciseName = "Weightlifting", TypeExercise = 2 }
    };

            mockRepo.Setup(repo => repo.GetAllExercisesForMemberAsync(null, null, It.IsAny<int>()))
                .ReturnsAsync(fakeExercises); // Return a list of exercises.

            var controller = new ExerciseController(mockRepo.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("Id", "1") }))
                }
            };

            // Act
            var result = await controller.GetAllExercises(null, null);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var objectResult = result as OkObjectResult;
            Assert.Equal(fakeExercises, objectResult?.Value);
        }

        [Fact]
        public async Task GetAllExercises_BoundaryCase_ReturnsNoExercises()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            mockRepo.Setup(repo => repo.GetAllExercisesForMemberAsync("", 99, It.IsAny<int>()))
                .ReturnsAsync(new List<GetAllExerciseForMember>()); // Return an empty list to simulate no exercises found.

            var controller = new ExerciseController(mockRepo.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("Id", "1") }))
                }
            };

            // Act
            var result = await controller.GetAllExercises("", 99);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}


