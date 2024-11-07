using BusinessObject.Dto.ExecriseDiary;
using BusinessObject.Dto.ExerciseDiary;
using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepo;
using System.Security.Claims;

namespace HealthTrackingManageAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ExerciseDiaryController : ControllerBase
	{
		private readonly HealthTrackingDBContext _context;
		private readonly IExeriseDiaryRepository _exerciseDiaryRepo;

		public ExerciseDiaryController(HealthTrackingDBContext context, IExeriseDiaryRepository exerciseDiaryRepo)
		{
			_exerciseDiaryRepo = exerciseDiaryRepo;
			_context = context;
		}

		[Authorize]
		[HttpPost("logExerciseDiary")]
		public async Task<IActionResult> LogExerciseDiary([FromBody] NewExerciseDiaryBatchDTO batchDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var memberIdClaim = User.FindFirstValue("Id");
			if (memberIdClaim == null)
				return Unauthorized("Member ID not found in claims.");

			if (!int.TryParse(memberIdClaim, out int memberId))
				return BadRequest("Invalid member ID.");

			// Create a list to hold exercise diary entries
			var exerciseDiaries = new List<ExerciseDiary>();

			// Process each exercise entry in the batch
			foreach (var exerciseDto in batchDto.Exercises)
			{
				// Validate that the referenced ExercisePlan and Exercise exist
				var exercisePlanExists = await _exerciseDiaryRepo.CheckExercisePlanExistsAsync(exerciseDto.ExercisePlanId);
				var exerciseExists = await _exerciseDiaryRepo.CheckExerciseExistsAsync(exerciseDto.ExerciseId);

				if (!exercisePlanExists || !exerciseExists)
					return NotFound("Exercise Plan or Exercise not found.");

				// Create new ExerciseDiary entry
				var exerciseDiary = new ExerciseDiary
				{
					MemberId = memberId,                    // Use memberId from claims
					ExercisePlanId = exerciseDto.ExercisePlanId,
					ExerciseId = exerciseDto.ExerciseId,
					Date = batchDto.Date,
					Duration = exerciseDto.Duration,
					CaloriesBurned = exerciseDto.CaloriesBurned
				};

				exerciseDiaries.Add(exerciseDiary);
			}

			// Save all entries in a single transaction using the repository
			await _exerciseDiaryRepo.AddExerciseDiaries(exerciseDiaries);

			return Ok("Exercise diary entries logged successfully.");
		}




		[Authorize]
		[HttpPost("logNormalExerciseDiary")]
		public async Task<IActionResult> LogNormalExerciseDiary([FromBody] NewNormalExerciseDiaryDTO normalExerciseDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var memberIdClaim = User.FindFirstValue("Id");
			if (memberIdClaim == null)
				return Unauthorized("Member ID not found in claims.");

			if (!int.TryParse(memberIdClaim, out int memberId))
				return BadRequest("Invalid member ID.");

			// Process each normal exercise entry
			foreach (var exerciseDto in normalExerciseDto.Exercises)
			{
				// Validate that the exercise exists
				var exerciseExists = await _exerciseDiaryRepo.CheckExerciseExistsAsync(exerciseDto.ExerciseId);
				if (!exerciseExists)
					return NotFound("Exercise not found.");

				// Create new ExerciseDiary entry
				var exerciseDiary = new ExerciseDiary
				{
					MemberId = memberId,                   // Use memberId from claims
					ExerciseId = exerciseDto.ExerciseId,
					Date = normalExerciseDto.Date,
					Duration = exerciseDto.Duration,
					CaloriesBurned = exerciseDto.CaloriesBurned
				};

				// Log the exercise diary entry using the repository
				await _exerciseDiaryRepo.AddExerciseDiary(exerciseDiary);
			}

			return Ok("Normal exercise diary entries logged successfully.");
		}
		[Authorize]
		[HttpGet("member/exercise_diary")]
		public async Task<IActionResult> GetDiaryByMemberId()
		{
			var memberIdClaim = User.FindFirstValue("Id");
			if (memberIdClaim == null || !int.TryParse(memberIdClaim, out int memberId))
				return Unauthorized("Member ID not found in claims.");

			try
			{
				// Retrieve exercise diaries for the specified member ID
				var exerciseDiaries = await _exerciseDiaryRepo.GetExerciseDiaryByMemberId(memberId);

				if (exerciseDiaries == null || exerciseDiaries.Count == 0)
					return NotFound("No exercise diary entries found for the specified member.");

				// Map to DTOs
				var diaryDtos = exerciseDiaries.Select(diary => new ExerciseDiaryDto
				{
					ExerciseDiaryId = diary.ExerciseDiaryId,
					Date = diary.Date,
					Exercise = new ExerciseDto
					{
						ExerciseId = diary.ExerciseId ?? 0,  // Ensure it's not null if ExerciseId is nullable
						ExerciseName = diary.Exercise?.ExerciseName ?? "Unknown", // Check for null reference
						Duration = diary.Duration,
						CaloriesBurned = diary.CaloriesBurned ?? 0  // Handle null CaloriesBurned
					}
				}).ToList();

				return Ok(diaryDtos);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}



		[Authorize]
		[HttpPost("logExerciseDiaryByPlan")]
		public async Task<IActionResult> LogExerciseDiaryByPlan([FromBody] LogExerciseDiaryByPlanDTO diaryByPlanDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var memberIdClaim = User.FindFirstValue("Id");
			if (memberIdClaim == null)
				return Unauthorized("Member ID not found in claims.");

			if (!int.TryParse(memberIdClaim, out int memberId))
				return BadRequest("Invalid member ID.");

			// Validate that the ExercisePlan exists
			var exercisePlanExists = await _exerciseDiaryRepo.CheckExercisePlanExistsAsync(diaryByPlanDto.ExercisePlanId);
			if (!exercisePlanExists)
				return NotFound("Exercise Plan not found.");

			// Get exercises for the specified plan
			var exercises = await _exerciseDiaryRepo.GetExercisesByPlanIdAsync(diaryByPlanDto.ExercisePlanId);
			if (exercises == null || exercises.Count == 0)
				return NotFound("No exercises found in the specified plan.");

			// Create a list of ExerciseDiary entries
			var exerciseDiaries = exercises.Select(exercise => new ExerciseDiary
			{
				MemberId = memberId,
				ExercisePlanId = diaryByPlanDto.ExercisePlanId,
				ExerciseId = exercise.ExerciseId,
				Date = diaryByPlanDto.Date,
				Duration = exercise.Duration, // Assuming duration comes from ExercisePlanDetail
				CaloriesBurned = CalculateCaloriesBurned(exercise.Duration, exercise.CaloriesPerHour) // Function to calculate calories
			}).ToList();

			// Save entries in batch
			await _exerciseDiaryRepo.AddExerciseDiaries(exerciseDiaries);

			return Ok("Exercise diary entries logged successfully from plan.");
		}

		private float CalculateCaloriesBurned(int duration, float caloriesPerHour)
{
    return (caloriesPerHour / 60) * duration;
}
	}
}

