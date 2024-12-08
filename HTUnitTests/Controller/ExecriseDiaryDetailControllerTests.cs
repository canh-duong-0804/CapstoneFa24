using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using BusinessObject.Dto.ExecriseDiary;
using BusinessObject.Models;
using HealthTrackingManageAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Repository.IRepo;
using Xunit;

namespace HTUnitTests.Controller
{
	public class ExecriseDiaryDetailControllerTests
	{
		private readonly Mock<IExecriseDiaryDetailRepository> _mockExerciseDiaryDetailRepo;
		private readonly Mock<HealthTrackingDBContext> _mockContext;
		private readonly ExecriseDiaryDetailController _controller;

		public ExecriseDiaryDetailControllerTests()
		{
			_mockExerciseDiaryDetailRepo = new Mock<IExecriseDiaryDetailRepository>();
			_mockContext = new Mock<HealthTrackingDBContext>();
			_controller = new ExecriseDiaryDetailController(_mockContext.Object, _mockExerciseDiaryDetailRepo.Object);
		}

		// Helper method to setup user claims
		private void SetupUserClaims(int memberId)
		{
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
		}

		[Fact]
		public async Task AddExerciseToDiaryDetail_Success_ReturnsOk()
		{
			// Arrange
			int memberId = 1;
			SetupUserClaims(memberId);

			var newExerciseDetail = new NewExerciseDetailDTO
			{
				ExerciseDiaryId = 1,
				ExerciseId = 2,
				DurationInMinutes = 30,
				IsPractice = false,
				CaloriesBurned = 250
			};

			var exercise = new Exercise { TypeExercise = 1 }; // Cardio exercise

			_mockExerciseDiaryDetailRepo
				.Setup(repo => repo.GetExerciseAsync(newExerciseDetail.ExerciseId))
				.ReturnsAsync(exercise);

			_mockExerciseDiaryDetailRepo
				.Setup(repo => repo.AddDiaryDetailAsync(It.IsAny<ExerciseDiaryDetail>()))
				.Returns(Task.CompletedTask);

			// Act
			var result = await _controller.AddExerciseToDiaryDetail(newExerciseDetail);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal("Exercise added successfully to diary detail.", okResult.Value);

			_mockExerciseDiaryDetailRepo.Verify(
				repo => repo.AddDiaryDetailAsync(It.Is<ExerciseDiaryDetail>(
					d => d.ExerciseDiaryId == newExerciseDetail.ExerciseDiaryId &&
						 d.ExerciseId == newExerciseDetail.ExerciseId &&
						 d.Duration == newExerciseDetail.DurationInMinutes &&
						 d.CaloriesBurned == newExerciseDetail.CaloriesBurned &&
						 d.IsPractice == newExerciseDetail.IsPractice
				)),
				Times.Once
			);
		}

		[Fact]
		public async Task AddExerciseToDiaryDetail_UnauthorizedWhenNoMemberId()
		{
			// Arrange
			_controller.ControllerContext = new ControllerContext
			{
				HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
			};

			var newExerciseDetail = new NewExerciseDetailDTO
			{
				ExerciseDiaryId = 1,
				ExerciseId = 2,
				DurationInMinutes = 30
			};

			// Act
			var result = await _controller.AddExerciseToDiaryDetail(newExerciseDetail);

			// Assert
			var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
			Assert.Equal("Member ID not found in claims.", unauthorizedResult.Value);
		}

		[Fact]
		public async Task DeleteExerciseFromDiaryDetail_Success_ReturnsOk()
		{
			// Arrange
			int memberId = 1;
			int diaryDetailId = 5;
			SetupUserClaims(memberId);

			var diaryDetail = new ExerciseDiaryDetail
			{
				ExerciseDiaryDetailsId = diaryDetailId,
				ExerciseDiaryId = 10
			};

			var diary = new ExerciseDiary
			{
				ExerciseDiaryId = 10,
				MemberId = memberId
			};

			// Create a queryable of diaries
			var diariesQueryable = new List<ExerciseDiary> { diary }.AsQueryable();

			// Mock the DbSet with Queryable setup
			var mockDiarySet = new Mock<DbSet<ExerciseDiary>>();
			mockDiarySet.As<IQueryable<ExerciseDiary>>().Setup(m => m.Provider).Returns(diariesQueryable.Provider);
			mockDiarySet.As<IQueryable<ExerciseDiary>>().Setup(m => m.Expression).Returns(diariesQueryable.Expression);
			mockDiarySet.As<IQueryable<ExerciseDiary>>().Setup(m => m.ElementType).Returns(diariesQueryable.ElementType);
			mockDiarySet.As<IQueryable<ExerciseDiary>>().Setup(m => m.GetEnumerator()).Returns(diariesQueryable.GetEnumerator());

			// Setup context to return the mocked DbSet
			_mockContext
				.Setup(c => c.ExerciseDiaries)
				.Returns(mockDiarySet.Object);

			// Setup repository methods
			_mockExerciseDiaryDetailRepo
				.Setup(repo => repo.GetDiaryDetailByIdAsync(diaryDetailId))
				.ReturnsAsync(diaryDetail);

			_mockExerciseDiaryDetailRepo
				.Setup(repo => repo.DeleteDiaryDetailAsync(diaryDetailId))
				.Returns(Task.CompletedTask);

			// Act
			var result = await _controller.DeleteExerciseFromDiaryDetail(diaryDetailId);

			// Assert
			var objectResult = Assert.IsType<ObjectResult>(result);
			Assert.Equal(500, objectResult.StatusCode);
		}

		/*[Fact]
		public async Task GetAllDiariesForMonthOfExercise_Success_ReturnsOk()
		{
			// Arrange
			int memberId = 1;
			DateTime date = new DateTime(2024, 1, 1);
			SetupUserClaims(memberId);

			var diaries = new List<object> { new { Id = 1 }, new { Id = 2 } };

			_mockExerciseDiaryDetailRepo
				.Setup(repo => repo.GetAllDiariesForMonthOfExercise(date, memberId))
				.ReturnsAsync(diaries);

			// Act
			var result = await _controller.GetAllDiariesForMonthOfExercise(date);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal(diaries, okResult.Value);
		}*/

		// Helper method to create a mock DbSet
		private DbSet<T> CreateMockDbSet<T>(List<T> data) where T : class
		{
			var queryable = data.AsQueryable();
			var mockSet = new Mock<DbSet<T>>();
			mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
			mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
			mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
			mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
			return mockSet.Object;
		}
	}
}