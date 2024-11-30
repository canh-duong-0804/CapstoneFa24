
using BusinessObject.Dto.ExecriseDiary;
using BusinessObject.DTOs;

using BusinessObject.Models;
using DataAccess;
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
        private readonly IExecriseDiaryDetailRepository _exerciseDiaryDetailRepo;

        public ExerciseDiaryController(HealthTrackingDBContext context, IExeriseDiaryRepository exerciseDiaryRepo, IExecriseDiaryDetailRepository exerciseDiaryDetailRepo)
        {
            _exerciseDiaryRepo = exerciseDiaryRepo;
            _context = context;
            _exerciseDiaryDetailRepo = exerciseDiaryDetailRepo;
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
                        ExerciseImage = ed.Exercise.ExerciseImage,
                        ExerciseName = ed.Exercise.ExerciseName,
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
        [HttpGet("member/exercise_diary_by_date")]
        public async Task<IActionResult> GetDiaryByDate(DateTime date)
        {
            var memberIdClaim = User.FindFirstValue("Id");
            if (memberIdClaim == null || !int.TryParse(memberIdClaim, out int memberId))
                return Unauthorized("Member ID not found in claims.");

            try
            {
                var targetDate = date.Date;

                // Fetch or create the diary
                var existingDiary = await _exerciseDiaryRepo.GetExerciseDiaryByDate(memberId, targetDate);

                if (existingDiary == null)
                {
                    var newDiary = new ExerciseDiary
                    {
                        MemberId = memberId,
                        Date = targetDate,
                        TotalDuration = null,
                        TotalCaloriesBurned = null,
                        ExercisePlanId = null
                    };

                    await _exerciseDiaryRepo.AddExerciseDiaryAsync(newDiary);

                    // Fetch the newly created diary
                    existingDiary = await _exerciseDiaryRepo.GetExerciseDiaryById(newDiary.ExerciseDiaryId);
                }



                // Update totals (duration and calories)
                await _exerciseDiaryRepo.UpdateTotalDurationAndCaloriesAsync(existingDiary.ExerciseDiaryId);

                // Reload the diary to reflect updated totals
                existingDiary = await _exerciseDiaryRepo.GetExerciseDiaryByDate(memberId, targetDate);

                // Map to DTO
                var diaryDto = new ExerciseDiaryDTO
                {
                    ExerciseDiaryId = existingDiary.ExerciseDiaryId,
                    MemberId = existingDiary.MemberId,
                    ExercisePlanId = existingDiary.ExercisePlanId,
                    Date = existingDiary.Date,
                    TotalDuration = existingDiary.TotalDuration,
                    TotalCaloriesBurned = existingDiary.TotalCaloriesBurned,
                    ExerciseDiaryDetails = existingDiary.ExerciseDiaryDetails.Select(ed => new ExerciseDiaryDetailDTO
                    {
                        ExerciseDiaryDetailsId = ed.ExerciseDiaryDetailsId,
                        ExerciseDiaryId = ed.ExerciseDiaryId,
                        IsPractice = ed.IsPractice,
                        ExerciseId = ed.ExerciseId,
                        Duration = ed.Duration,
                        CaloriesBurned = ed.CaloriesBurned,
                        ExerciseName = ed.Exercise.ExerciseName,
                        ExerciseImage = ed.Exercise.ExerciseImage
                    }).ToList()
                };

                return Ok(diaryDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [Authorize]
        [HttpPut("update_is_practice")]
        public async Task<IActionResult> UpdateIsPracticeStatus([FromBody] UpdateIsPracticeDTO request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid input data.");
                }

                var memberIdClaim = User.FindFirstValue("Id");
                if (memberIdClaim == null || !int.TryParse(memberIdClaim, out int memberId))
                {
                    return Unauthorized("Member ID not found in claims.");
                }

                // Validate and fetch the diary detail
                var exerciseDetail = await _exerciseDiaryDetailRepo.GetExerciseDiaryDetailById(request.ExerciseDiaryDetailId);
                if (exerciseDetail == null)
                {
                    return NotFound("Exercise detail not found.");
                }



                // Update the IsPractice field
                exerciseDetail.IsPractice = request.IsPractice;
                await _exerciseDiaryDetailRepo.UpdateExerciseDiaryDetailAsync(exerciseDetail);

                return Ok(new
                {
                    success = true,
                    message = "Updated IsPractice status successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error.",
                    error = ex.Message
                });
            }
        }


        [Authorize]
        [HttpGet("member/exercise_diary_streak")]
        public async Task<IActionResult> GetExerciseDiaryStreak()
        {
            var memberIdClaim = User.FindFirstValue("Id");
            if (memberIdClaim == null || !int.TryParse(memberIdClaim, out int memberId))
                return Unauthorized("Member ID not found in claims.");

            try
            {
                var (streakCount, streakDates) = await _exerciseDiaryRepo.GetExerciseDiaryStreakWithDates(memberId);

                return Ok(new
                {
                    streakCount,
                    streakDates
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

}


