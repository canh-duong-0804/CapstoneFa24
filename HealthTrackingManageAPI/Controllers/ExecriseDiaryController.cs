
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



	}
}

