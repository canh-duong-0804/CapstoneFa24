using BusinessObject.Dto.ExerciseDiary;
using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Repository.IRepo;
using System.Threading.Tasks;
using BusinessObject.Dto.ExecriseDiary;
using Repository.Repo;

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
	}
}
