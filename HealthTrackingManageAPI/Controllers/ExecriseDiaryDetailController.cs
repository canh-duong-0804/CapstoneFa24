using BusinessObject.Dto.ExecriseDiary;
using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.IRepo;
using Repository.Repo;
using System.Security.Claims;

namespace HealthTrackingManageAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ExecriseDiaryDetailController : ControllerBase
	{
		private readonly HealthTrackingDBContext _context;
		private readonly IExecriseDiaryDetailRepository _exerciseDiaryDetailRepo;

		public ExecriseDiaryDetailController(HealthTrackingDBContext context, IExecriseDiaryDetailRepository exerciseDiaryDetailRepo)
		{
			_exerciseDiaryDetailRepo = exerciseDiaryDetailRepo;
			_context = context;
		}


		[HttpPost("AddExercise")]
		[Authorize]
		public async Task<IActionResult> AddExerciseToDiaryDetail([FromBody] NewExerciseDetailDTO newExerciseDetail)
		{
			try
			{
				// Get member ID from claims
				var memberId = User.FindFirstValue("Id");

				if (memberId == null)
				{
					return Unauthorized("Member ID not found in claims.");
				}

				// Fetch the exercise details to check if it's cardio or resistance
				var exercise = await _exerciseDiaryDetailRepo.GetExerciseAsync(newExerciseDetail.ExerciseId);

				if (exercise == null)
				{
					return NotFound("Exercise not found.");
				}

				double? caloriesBurned = null; // Default for resistance exercises

				if (exercise.IsCardio == true)
				{
					// Cardio exercises: Calculate calories burned
					var bodyWeightKg = await _exerciseDiaryDetailRepo.GetLatestBodyWeightAsync(memberId);

					if (bodyWeightKg == null || bodyWeightKg <= 0)
					{
						return BadRequest("Invalid or missing body weight. Please update your profile.");
					}

					var metValue = await _exerciseDiaryDetailRepo.GetExerciseMetValueAsync(newExerciseDetail.ExerciseId);

					if (metValue == null)
					{
						return NotFound("Missing MET value for the exercise.");
					}

					caloriesBurned = (metValue.Value * 3.5 * bodyWeightKg.Value * newExerciseDetail.DurationInMinutes) / 200;
				}

				// Prepare diary detail entry
				var diaryDetail = new ExerciseDiaryDetail
				{
					ExerciseDiaryId = newExerciseDetail.ExerciseDiaryId,
					ExerciseId = newExerciseDetail.ExerciseId,
					Duration = newExerciseDetail.DurationInMinutes,
					CaloriesBurned = caloriesBurned, // Null for resistance exercises
					IsPractice = newExerciseDetail.IsPractice
				};

				// Add to diary detail via repository
				await _exerciseDiaryDetailRepo.AddDiaryDetailAsync(diaryDetail);

				return Ok("Exercise added successfully to diary detail.");
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
			}
		}
	}
}
