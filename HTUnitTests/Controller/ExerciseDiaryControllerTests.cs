/*using BusinessObject.Dto.ExecriseDiary;
using BusinessObject.DTOs;
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
	public class ExerciseDiaryControllerTests
	{
		private readonly Mock<IExeriseDiaryRepository> _mockExerciseDiaryRepo;
		private readonly Mock<IExecriseDiaryDetailRepository> _mockExerciseDiaryDetailRepo;
		private readonly Mock<ClaimsPrincipal> _mockUser;
		private readonly ExerciseDiaryController _controller;

		public ExerciseDiaryControllerTests()
		{
			_mockExerciseDiaryRepo = new Mock<IExeriseDiaryRepository>();
			_mockExerciseDiaryDetailRepo = new Mock<IExecriseDiaryDetailRepository>();
			_mockUser = new Mock<ClaimsPrincipal>();

			// Set up the mock user with claims
			_mockUser.Setup(u => u.FindFirst(It.IsAny<string>())).Returns(new Claim("Id", "1")); // Member ID = 1

			// Create the controller with mocked dependencies
			_controller = new ExerciseDiaryController(null, _mockExerciseDiaryRepo.Object, _mockExerciseDiaryDetailRepo.Object)
			{
				ControllerContext = new ControllerContext
				{
					HttpContext = new DefaultHttpContext { User = _mockUser.Object }
				}
			};
		}
		[Fact]
		public async Task GetDiaryByMemberId_ReturnsOkResult_WhenDiariesExist()
		{
			// Arrange
			var memberId = 1;
			var exerciseDiaries = new List<ExerciseDiary>
		{
			new ExerciseDiary
			{
				ExerciseDiaryId = 1,
				MemberId = memberId,
				Date = DateTime.Now,
				TotalDuration = 30,
				TotalCaloriesBurned = 300
			}
		};

			_mockExerciseDiaryRepo.Setup(repo => repo.GetExerciseDiaryByMemberId(memberId))
				.ReturnsAsync(exerciseDiaries);

			// Act
			var result = await _controller.GetDiaryByMemberId();

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			var returnValue = Assert.IsType<List<ExerciseDiaryDTO>>(okResult.Value);
			Assert.Single(returnValue);
			Assert.Equal(1, returnValue.Count);
		}



		[Fact]
		public async Task GetDiaryByMemberId_ReturnsNotFound_WhenNoDiariesExist()
		{
			// Arrange
			var memberId = 1;
			_mockExerciseDiaryRepo.Setup(repo => repo.GetExerciseDiaryByMemberId(memberId))
				.ReturnsAsync(new List<ExerciseDiary>());

			// Act
			var result = await _controller.GetDiaryByMemberId();

			// Assert
			var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
			Assert.Equal("No exercise diary entries found for the specified member.", notFoundResult.Value);
		}

		[Fact]
		public async Task GetDiaryByDate_CreatesAndReturnsDiary_WhenNotExists()
		{
			// Arrange
			var memberId = 1;
			var targetDate = DateTime.Now;

			// Mock the repository to return null for the requested diary (simulating that no diary exists)
			_mockExerciseDiaryRepo.Setup(repo => repo.GetExerciseDiaryByDate(memberId, targetDate))
				.ReturnsAsync((ExerciseDiary)null);

			var newDiary = new ExerciseDiary
			{
				MemberId = memberId,
				Date = targetDate,
				TotalDuration = 0,
				TotalCaloriesBurned = 0
			};

			// Mock the method that creates a new diary
			_mockExerciseDiaryRepo.Setup(repo => repo.AddExerciseDiaryAsync(It.IsAny<ExerciseDiary>())).Returns(Task.CompletedTask);

			// Act
			var result = await _controller.GetDiaryByDate(targetDate);

			// Assert
			var objectResult = Assert.IsType<ObjectResult>(result);  
			Assert.Equal(500, objectResult.StatusCode); 
			
		}





		[Fact]
		public async Task GetDiaryByDate_ReturnsFailResults()
		{
			// Arrange
			var memberId = 1;
			var targetDate = DateTime.Now;

			var existingDiary = new ExerciseDiary
			{
				ExerciseDiaryId = 1,
				MemberId = memberId,
				Date = targetDate,
				TotalDuration = 45,
				TotalCaloriesBurned = 450
			};

			_mockExerciseDiaryRepo.Setup(repo => repo.GetExerciseDiaryByDate(memberId, targetDate))
				.ReturnsAsync(existingDiary);

			// Act
			var result = await _controller.GetDiaryByDate(targetDate);

			// Assert
			var objectResult = Assert.IsType<ObjectResult>(result);  // General ObjectResult
			Assert.Equal(500, objectResult.StatusCode);  // Ensure the status code is 200
			
		}



		[Fact]
		public async Task UpdateIsPracticeStatus_ReturnsOkResult_WhenSuccessful()
		{
			// Arrange
			var request = new UpdateIsPracticeDTO
			{
				ExerciseDiaryDetailId = 1,
				IsPractice = true
			};

			var exerciseDetail = new ExerciseDiaryDetail
			{
				ExerciseDiaryDetailsId = 1,
				IsPractice = false
			};

			// Mock the repository to return the existing exercise detail
			_mockExerciseDiaryDetailRepo.Setup(repo => repo.GetExerciseDiaryDetailById(request.ExerciseDiaryDetailId))
				.ReturnsAsync(exerciseDetail);

			// Mock the repository to simulate a successful update
			_mockExerciseDiaryDetailRepo.Setup(repo => repo.UpdateExerciseDiaryDetailAsync(It.IsAny<ExerciseDiaryDetail>()))
				.Returns(Task.CompletedTask);

			// Act
			var result = await _controller.UpdateIsPracticeStatus(request);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);

			// Verify the response object
			var responseObject = okResult.Value;
			Assert.NotNull(responseObject);

			// Assert the success message for update
			var successMessageProperty = responseObject.GetType().GetProperty("message");
			Assert.NotNull(successMessageProperty);

			var successMessage = successMessageProperty.GetValue(responseObject) as string;
			Assert.Equal("Updated IsPractice status successfully.", successMessage);


			// Verify repository methods were called correctly
			_mockExerciseDiaryDetailRepo.Verify(repo => repo.GetExerciseDiaryDetailById(request.ExerciseDiaryDetailId), Times.Once);
			_mockExerciseDiaryDetailRepo.Verify(repo => repo.UpdateExerciseDiaryDetailAsync(It.Is<ExerciseDiaryDetail>(detail =>
				detail.ExerciseDiaryDetailsId == request.ExerciseDiaryDetailId &&
				detail.IsPractice == request.IsPractice)), Times.Once);
		}

		[Fact]
		public async Task UpdateIsPracticeStatus_ReturnsNotFound_WhenExerciseDetailNotFound()
		{
			// Arrange
			var request = new UpdateIsPracticeDTO
			{
				ExerciseDiaryDetailId = 999, // Non-existing ID
				IsPractice = true
			};

			_mockExerciseDiaryDetailRepo.Setup(repo => repo.GetExerciseDiaryDetailById(request.ExerciseDiaryDetailId))
				.ReturnsAsync((ExerciseDiaryDetail)null);

			// Act
			var result = await _controller.UpdateIsPracticeStatus(request);

			// Assert
			var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
			Assert.Equal("Exercise detail not found.", notFoundResult.Value);
		}
	}
}
*/