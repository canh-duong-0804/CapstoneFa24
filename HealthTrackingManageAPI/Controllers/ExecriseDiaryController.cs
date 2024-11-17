
using BusinessObject.DTOs;
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

                // Map to DTOs (Assuming the repository returns ExerciseDiary objects)
                var diaryDtos = exerciseDiaries.Select(e => new ExerciseDiaryDTO
                {
                    ExerciseDiaryId = e.ExerciseDiaryId,
                    MemberId = e.MemberId,
                    ExercisePlanId = e.ExercisePlanId,
                    Date = e.Date,
                    TotalDuration = e.TotalDuration,
                    TotalCaloriesBurned = e.TotalCaloriesBurned,
                    ExerciseDiaryDetails = e.ExerciseDiaryDetails.Select(ed => new ExerciseDiaryDetailDTO
                    {
                        ExerciseDiaryDetailsId = ed.ExerciseDiaryDetailsId,
                        ExerciseDiaryId = ed.ExerciseDiaryId,
                        IsPractice = ed.IsPractice,
                        ExerciseId = ed.ExerciseId,
                        Duration = ed.Duration,
                        CaloriesBurned = ed.CaloriesBurned
                    }).ToList()
                }).ToList();

                return Ok(diaryDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


		[Authorize]
		[HttpPost("member/check_or_create_today_diary")]
		public async Task<IActionResult> CheckOrCreateTodayDiary()
		{
			var memberIdClaim = User.FindFirstValue("Id");
			if (memberIdClaim == null || !int.TryParse(memberIdClaim, out int memberId))
				return Unauthorized("Member ID not found in claims.");

			try
			{
				// Check if today's diary exists for the member
				var today = DateTime.UtcNow.Date; // Use UTC to ensure consistency
				var existingDiary = await _exerciseDiaryRepo.GetTodayExerciseDiaryByMemberId(memberId, today);

				if (existingDiary != null)
				{
					// If a diary for today already exists, return it
					return Ok(new
					{
						message = "Today's diary already exists.",
						diaryId = existingDiary.ExerciseDiaryId
					});
				}

				// If not, create a new diary
				var newDiary = new ExerciseDiary
				{
					MemberId = memberId,
					Date = today,
					TotalDuration = null, // No duration yet
					TotalCaloriesBurned = null, // No calories burned yet
					ExercisePlanId = null // Assuming it’s optional; set if applicable
				};

				await _exerciseDiaryRepo.AddExerciseDiaryAsync(newDiary);

				return Ok(new
				{
					message = "New diary created for today.",
					diaryId = newDiary.ExerciseDiaryId
				});
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}
		[Authorize]
		[HttpGet("member/exercise_diary_by_date")]
		public async Task<IActionResult> GetDiaryByDate(DateTime date)
		{
			var memberIdClaim = User.FindFirstValue("Id");
			if (memberIdClaim == null || !int.TryParse(memberIdClaim, out int memberId))
				return Unauthorized("Member ID not found in claims.");

			try
			{
				// Retrieve the exercise diary for the specified date
				var diary = await _exerciseDiaryRepo.GetExerciseDiaryByDate(memberId, date.Date);

				if (diary == null)
					return NotFound($"No exercise diary found for the date: {date.Date.ToShortDateString()}.");
				
				// Map to DTO (assuming the repository returns an ExerciseDiary object)
				var diaryDto = new ExerciseDiaryDTO
				{
					ExerciseDiaryId = diary.ExerciseDiaryId,
					MemberId = diary.MemberId,
					ExercisePlanId = diary.ExercisePlanId,
					Date = diary.Date,
					TotalDuration = diary.TotalDuration,
					TotalCaloriesBurned = diary.TotalCaloriesBurned,
					ExerciseDiaryDetails = diary.ExerciseDiaryDetails.Select(ed => new ExerciseDiaryDetailDTO
					{
						ExerciseDiaryDetailsId = ed.ExerciseDiaryDetailsId,
						ExerciseDiaryId = ed.ExerciseDiaryId,
						IsPractice = ed.IsPractice,
						ExerciseId = ed.ExerciseId,
						Duration = ed.Duration,
						CaloriesBurned = ed.CaloriesBurned
					}).ToList()
				};

				return Ok(diaryDto);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		[HttpPost("update_totals")]
		[Authorize]
		public async Task<IActionResult> UpdateDiaryTotals(int exerciseDiaryId)
		{
			try
			{
				// Fetch member ID from claims
				var memberIdClaim = User.FindFirstValue("Id");
				if (memberIdClaim == null || !int.TryParse(memberIdClaim, out int memberId))
				{
					return Unauthorized("Member ID not found in claims.");
				}

				// Fetch the diary to ensure it belongs to the logged-in member
				var diary = await _exerciseDiaryRepo.GetExerciseDiaryById(exerciseDiaryId);
				if (diary == null || diary.MemberId != memberId)
				{
					return NotFound("Exercise diary not found or does not belong to the logged-in user.");
				}

				// Update totals
				await _exerciseDiaryRepo.UpdateTotalDurationAndCaloriesAsync(exerciseDiaryId);

				return Ok("Totals updated successfully.");
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}
	}

}


