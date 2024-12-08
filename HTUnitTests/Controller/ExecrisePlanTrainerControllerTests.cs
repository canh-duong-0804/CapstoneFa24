using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessObject.Dto.ExecrisePlan;
using BusinessObject.Models;
using HealthTrackingManageAPI.Controllers;
using Repository.IRepo;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class ExecrisePlanTrainerControllerTests
{
	private readonly Mock<IExecrisePlanTrainerRepository> _mockRepository;
	private readonly ExecrisePlanTrainerController _controller;

	public ExecrisePlanTrainerControllerTests()
	{
		_mockRepository = new Mock<IExecrisePlanTrainerRepository>();
		_controller = new ExecrisePlanTrainerController(_mockRepository.Object);
	}

	[Fact]
	public async Task GetAllExercisePlans_ReturnsOkResult_WhenDataExists()
	{
		// Arrange
		var exercisePlans = new List<ExercisePlanDTO>
		{
			new ExercisePlanDTO { ExercisePlanId = 1, Name = "Plan A", TotalCaloriesBurned = 500, ExercisePlanImage = "imageA.png" },
			new ExercisePlanDTO { ExercisePlanId = 2, Name = "Plan B", TotalCaloriesBurned = 300, ExercisePlanImage = "imageB.png" }
		};
		var pagedResult = new GetExercisePlanResponseForTrainerDTO
		{
			Data = exercisePlans,
			TotalRecords = 2,
			Page = 1,
			PageSize = 10,
			TotalPages = 1
		};

		_mockRepository.Setup(repo => repo.GetAllExercisePlansAsync(1, 10))
			.ReturnsAsync(pagedResult);

		// Act
		var result = await _controller.GetAllExercisePlans(1);

		// Assert
		var okResult = Assert.IsType<OkObjectResult>(result);
		var returnedData = Assert.IsType<GetExercisePlanResponseForTrainerDTO>(okResult.Value);
		Assert.Equal(2, returnedData.TotalRecords);
	}

	[Fact]
	public async Task GetAllExercisePlans_ReturnsNotFound_WhenNoDataExists()
	{
		// Arrange
		_mockRepository.Setup(repo => repo.GetAllExercisePlansAsync(1, 10))
			.ReturnsAsync((GetExercisePlanResponseForTrainerDTO)null);

		// Act
		var result = await _controller.GetAllExercisePlans(1);

		// Assert
		Assert.IsType<NotFoundObjectResult>(result);
	}

	[Fact]
	public async Task CreateExercisePlan_ReturnsOkResult_WhenValidRequest()
	{
		// Arrange
		var request = new CreateExercisePlanRequestDTO
		{
			Name = "New Plan",
			TotalCaloriesBurned = 1000,
			ExercisePlanImage = "newImage.png",
			Status = true
		};

		// Mock the user claims
		var userId = "123"; // Simulated user ID
		var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
		{
		new Claim("Id", userId)
		}));

		// Set up the controller with the mocked user
		_controller.ControllerContext = new ControllerContext()
		{
			HttpContext = new DefaultHttpContext() { User = user }
		};

		_mockRepository.Setup(repo => repo.AddExercisePlanAsync(It.IsAny<ExercisePlan>()))
			.ReturnsAsync(true);

		// Act
		var result = await _controller.CreateExercisePlan(request);

		// Assert
		var okResult = Assert.IsType<OkObjectResult>(result);

		// Handle anonymous type for the response
		var responseObject = okResult.Value;
		Assert.NotNull(responseObject);

		// Use reflection to get the Message property
		var messageProperty = responseObject.GetType().GetProperty("Message");
		Assert.NotNull(messageProperty);

		var messageValue = messageProperty.GetValue(responseObject) as string;
		Assert.Equal("Exercise plan created successfully.", messageValue);

		// Verify that the repository method was called with the correct plan
		_mockRepository.Verify(repo => repo.AddExercisePlanAsync(It.Is<ExercisePlan>(
			plan => plan.Name == request.Name &&
					plan.TotalCaloriesBurned == request.TotalCaloriesBurned &&
					plan.ExercisePlanImage == request.ExercisePlanImage &&
					plan.Status == request.Status &&
					plan.CreateBy == int.Parse(userId)
		)), Times.Once);
	}
	[Fact]
	public async Task CreateExercisePlan_ReturnsBadRequest_WhenRepositoryReturnsFalse()
	{
		// Arrange
		var request = new CreateExercisePlanRequestDTO
		{
			Name = "New Plan",
			TotalCaloriesBurned = 1000,
			ExercisePlanImage = "newImage.png",
			Status = true
		};

		// Mock the user claims
		var userId = "123"; // Simulated user ID
		var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
		{
		new Claim("Id", userId)
		}));

		// Set up the controller with the mocked user
		_controller.ControllerContext = new ControllerContext()
		{
			HttpContext = new DefaultHttpContext() { User = user }
		};

		_mockRepository.Setup(repo => repo.AddExercisePlanAsync(It.IsAny<ExercisePlan>()))
			.ReturnsAsync(false);

		// Act
		var result = await _controller.CreateExercisePlan(request);

		// Assert
		var objectResult = Assert.IsType<ObjectResult>(result);
		Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
		Assert.Equal("Failed to create exercise plan.", objectResult.Value);
	}

	[Fact]
	public async Task UpdateExercisePlanDetail_ReturnsOkResult_WhenUpdateIsSuccessful()
	{
		// Arrange
		var request = new GetExercisePlanDetailDTO
		{
			ExercisePlanId = 1,
			Day = 1,
			execriseInPlans = new List<DayExerciseDTO>
			{
				new DayExerciseDTO { ExerciseId = 1, ExerciseName = "Push-ups", Duration = 10 },
				new DayExerciseDTO { ExerciseId = 2, ExerciseName = "Running", Duration = 20 }
			}
		};

		_mockRepository.Setup(repo => repo.UpdateExercisePlanDetailAsync(request))
			.ReturnsAsync(true);

		// Act
		var result = await _controller.UpdateExercisePlanDetail(request);

		// Assert
		var okResult = Assert.IsType<OkObjectResult>(result);
		Assert.Equal("Exercise plan detail updated successfully.", okResult.Value);
	}

	[Fact]
	public async Task UpdateExercisePlanDetail_ReturnsNotFound_WhenUpdateFails()
	{
		// Arrange
		var request = new GetExercisePlanDetailDTO
		{
			ExercisePlanId = 1,
			Day = 1,
			execriseInPlans = new List<DayExerciseDTO>
		{
			new DayExerciseDTO { ExerciseId = 1, ExerciseName = "Push-ups", Duration = 10 },
			new DayExerciseDTO { ExerciseId = 2, ExerciseName = "Running", Duration = 20 }
		}
		};

		_mockRepository.Setup(repo => repo.UpdateExercisePlanDetailAsync(request))
			.ReturnsAsync(false);

		// Act
		var result = await _controller.UpdateExercisePlanDetail(request);

		// Assert
		var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
		Assert.Equal("Failed to update exercise plan detail. The provided data may not exist.", notFoundResult.Value);
	}


	[Fact]
	public async Task DeleteExercisePlanDetail_ReturnsOkResult_WhenDeletionIsSuccessful()
	{
		// Arrange
		int detailId = 1;
		_mockRepository.Setup(repo => repo.DeleteExercisePlanDetailAsync(detailId))
			.ReturnsAsync(true);

		// Act
		var result = await _controller.DeleteExercisePlanDetail(detailId);

		// Assert
		var okResult = Assert.IsType<OkObjectResult>(result);

		// Handle anonymous type
		var responseObject = okResult.Value;
		Assert.NotNull(responseObject);

		// Use reflection to get the Message property
		var messageProperty = responseObject.GetType().GetProperty("Message");
		Assert.NotNull(messageProperty);

		var messageValue = messageProperty.GetValue(responseObject) as string;
		Assert.Equal("Exercise plan detail deleted successfully.", messageValue);
	}

	[Fact]
	public async Task UpdateExercisePlan_ReturnsOkResult_WhenValidRequest()
	{
		// Arrange
		var request = new UpdateExercisePlanRequestDTO
		{
			ExercisePlanId = 1,
			Name = "Updated Plan",
			TotalCaloriesBurned = 1200,
			ExercisePlanImage = "updatedImage.png",
			Status = false
		};

		// Mock the user claims
		var userId = "123"; // Simulated user ID
		var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
		{
		new Claim("Id", userId)
		}));

		// Set up the controller with the mocked user
		_controller.ControllerContext = new ControllerContext()
		{
			HttpContext = new DefaultHttpContext() { User = user }
		};

		// Mock existing plan
		var existingPlan = new ExercisePlan
		{
			ExercisePlanId = 1,
			Name = "Original Plan",
			TotalCaloriesBurned = 1000,
			ExercisePlanImage = "originalImage.png",
			Status = true
		};

		// Setup repository methods
		_mockRepository.Setup(repo => repo.GetExercisePlanByIdAsync(request.ExercisePlanId))
			.ReturnsAsync(existingPlan);

		_mockRepository.Setup(repo => repo.UpdateExercisePlanAsync(It.IsAny<ExercisePlan>()))
			.ReturnsAsync(true);

		// Act
		var result = await _controller.UpdateExercisePlan(request);

		// Assert
		var okResult = Assert.IsType<OkObjectResult>(result);

		// Verify response
		var responseObject = okResult.Value;
		Assert.NotNull(responseObject);

		var messageProperty = responseObject.GetType().GetProperty("Message");
		Assert.NotNull(messageProperty);

		var messageValue = messageProperty.GetValue(responseObject) as string;
		Assert.Equal("Exercise plan updated successfully.", messageValue);

		// Verify repository method calls and updated plan
		_mockRepository.Verify(repo => repo.GetExercisePlanByIdAsync(request.ExercisePlanId), Times.Once);
		_mockRepository.Verify(repo => repo.UpdateExercisePlanAsync(It.Is<ExercisePlan>(
			plan => plan.Name == request.Name &&
					plan.TotalCaloriesBurned == request.TotalCaloriesBurned &&
					plan.ExercisePlanImage == request.ExercisePlanImage &&
					plan.Status == request.Status &&
					plan.ChangeBy == int.Parse(userId)
		)), Times.Once);
	}

	[Fact]
	public async Task UpdateExercisePlan_ReturnsNotFound_WhenPlanDoesNotExist()
	{
		// Arrange
		var request = new UpdateExercisePlanRequestDTO
		{
			ExercisePlanId = 1,
			Name = "Updated Plan",
			TotalCaloriesBurned = 1200,
			ExercisePlanImage = "updatedImage.png",
			Status = false
		};

		// Mock the user claims
		var userId = "123"; // Simulated user ID
		var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
		{
		new Claim("Id", userId)
		}));

		// Set up the controller with the mocked user
		_controller.ControllerContext = new ControllerContext()
		{
			HttpContext = new DefaultHttpContext() { User = user }
		};

		// Setup repository method to return null for non-existent plan
		_mockRepository.Setup(repo => repo.GetExercisePlanByIdAsync(request.ExercisePlanId))
			.ReturnsAsync((ExercisePlan)null);

		// Act
		var result = await _controller.UpdateExercisePlan(request);

		// Assert
		var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
		Assert.Equal("Exercise plan not found.", notFoundResult.Value);
	}

	[Fact]
	public async Task UpdateExercisePlan_ReturnsUnauthorized_WhenUserIdInvalid()
	{
		// Arrange
		var request = new UpdateExercisePlanRequestDTO
		{
			ExercisePlanId = 1,
			Name = "Updated Plan",
			TotalCaloriesBurned = 1200,
			ExercisePlanImage = "updatedImage.png",
			Status = false
		};

		// Create user with invalid ID
		var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
		{
		new Claim("Id", "invalid")
		}));

		// Set up the controller with the mocked user
		_controller.ControllerContext = new ControllerContext()
		{
			HttpContext = new DefaultHttpContext() { User = user }
		};

		// Act
		var result = await _controller.UpdateExercisePlan(request);

		// Assert
		Assert.IsType<UnauthorizedResult>(result);
	}


	[Fact]
	public async Task GetExercisePlanDetail_ReturnsOkResult_WhenValidRequest()
	{
		// Arrange
		int exercisePlanId = 1;
		byte day = 1;
		var expectedDetail = new GetExercisePlanDetailDTO
		{
			ExercisePlanId = exercisePlanId,
			Day = day,
			execriseInPlans = new List<DayExerciseDTO>
		{
			new DayExerciseDTO { ExerciseId = 1, ExerciseName = "Push-ups", Duration = 10 }
		}
		};

		_mockRepository.Setup(repo => repo.GetExercisePlanDetailAsync(exercisePlanId, day))
			.ReturnsAsync(expectedDetail);

		// Act
		var result = await _controller.GetExercisePlanDetail(exercisePlanId, day);

		// Assert
		var okResult = Assert.IsType<OkObjectResult>(result);
		Assert.Equal(expectedDetail, okResult.Value);
	}

	[Fact]
	public async Task DeleteExercisePlan_ReturnsOkResult_WhenDeletionSuccessful()
	{
		// Arrange
		int planId = 1;

		_mockRepository.Setup(repo => repo.SoftDeleteExercisePlanAsync(planId))
			.ReturnsAsync(true);

		// Act
		var result = await _controller.DeleteExercisePlan(planId);

		// Assert
		var okResult = Assert.IsType<OkObjectResult>(result);

		// Verify the response object
		var responseObject = okResult.Value;
		Assert.NotNull(responseObject);

		var messageProperty = responseObject.GetType().GetProperty("Message");
		Assert.NotNull(messageProperty);

		var messageValue = messageProperty.GetValue(responseObject) as string;
		Assert.Equal("Exercise plan deleted successfully.", messageValue);
	}

	[Fact]
	public async Task CreateExercisePlanDetail_ReturnsInternalServerError_WhenAddingFails()
	{
		// Arrange
		var request = new CreateExercisePlanDetailRequestDTO
		{
			ExercisePlanId = 1,
			Day = 1,
			ExecriseInPlans = new List<ExecriseInPlan>
		{
			new ExecriseInPlan
			{
				ExerciseId = 1,
				Duration = 10
			}
		}
		};

		_mockRepository.Setup(repo => repo.AddExercisePlanDetailAsync(It.IsAny<List<ExercisePlanDetail>>()))
			.ReturnsAsync(false);

		// Act
		var result = await _controller.CreateExercisePlanDetail(request);

		// Assert
		var statusResult = Assert.IsType<ObjectResult>(result);
		Assert.Equal(StatusCodes.Status500InternalServerError, statusResult.StatusCode);
		Assert.Equal("Failed to create exercise plan detail.", statusResult.Value);
	}
}
