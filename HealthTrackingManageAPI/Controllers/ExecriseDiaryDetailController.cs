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

                if (exercise.TypeExercise == 1)
                {

                    caloriesBurned = newExerciseDetail.CaloriesBurned;


                }

                // Prepare diary detail entry
                var diaryDetail = new ExerciseDiaryDetail
                {
                    ExerciseDiaryId = newExerciseDetail.ExerciseDiaryId,
                    ExerciseId = newExerciseDetail.ExerciseId,
                    Duration = newExerciseDetail.DurationInMinutes,
                    CaloriesBurned = caloriesBurned,
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


        [HttpDelete("DeleteExercise/{diaryDetailId}")]
        [Authorize]
        public async Task<IActionResult> DeleteExerciseFromDiaryDetail(int diaryDetailId)
        {
            try
            {
                // Get member ID from claims
                var memberId = User.FindFirstValue("Id");
                if (memberId == null || !int.TryParse(memberId, out int parsedMemberId))
                {
                    return Unauthorized("Member ID not found in claims.");
                }

                // Fetch the diary detail to delete
                var diaryDetail = await _exerciseDiaryDetailRepo.GetDiaryDetailByIdAsync(diaryDetailId);

                if (diaryDetail == null)
                {
                    return NotFound("Diary detail not found.");
                }

                // Ensure the diary belongs to the logged-in user
                var diary = await _context.ExerciseDiaries.FirstOrDefaultAsync(d => d.ExerciseDiaryId == diaryDetail.ExerciseDiaryId);
                if (diary == null || diary.MemberId != parsedMemberId)
                {
                    return Unauthorized("You do not have permission to delete this exercise.");
                }

                // Delete the diary detail
                await _exerciseDiaryDetailRepo.DeleteDiaryDetailAsync(diaryDetailId);

                return Ok("Exercise deleted successfully from diary detail.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpPut("UpdateExercise")]
        [Authorize]
        public async Task<IActionResult> UpdateExerciseDiaryDetail([FromBody] UpdateExerciseDetailDTO updateExerciseDetail)
        {
            try
            {
                // Get member ID from claims
                var memberId = User.FindFirstValue("Id");
                if (memberId == null || !int.TryParse(memberId, out int parsedMemberId))
                {
                    return Unauthorized("Member ID not found in claims.");
                }

                // Fetch the diary detail to update
                var diaryDetail = await _exerciseDiaryDetailRepo.GetDiaryDetailByIdAsync(updateExerciseDetail.ExerciseDiaryDetailId);

                if (diaryDetail == null)
                {
                    return NotFound("Diary detail not found.");
                }

                // Ensure the diary belongs to the logged-in user
                var diary = await _context.ExerciseDiaries.FirstOrDefaultAsync(d => d.ExerciseDiaryId == diaryDetail.ExerciseDiaryId);
                if (diary == null || diary.MemberId != parsedMemberId)
                {
                    return Unauthorized("You do not have permission to update this exercise.");
                }

                // Update the diary detail fields
                if (updateExerciseDetail.DurationInMinutes.HasValue)
                {
                    diaryDetail.Duration = updateExerciseDetail.DurationInMinutes;
                }

                if (updateExerciseDetail.CaloriesBurned.HasValue)
                {
                    diaryDetail.CaloriesBurned = updateExerciseDetail.CaloriesBurned;
                }

                if (updateExerciseDetail.IsPractice.HasValue)
                {
                    diaryDetail.IsPractice = updateExerciseDetail.IsPractice.Value;
                }

                // Save changes via repository
                await _exerciseDiaryDetailRepo.UpdateDiaryDetailAsync(diaryDetail);

                return Ok("Exercise diary detail updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpPost("AssignPlan")]
        [Authorize]
        public async Task<IActionResult> AssignPlanToUser([FromBody] AssignPlanDTO assignPlanDto)
        {
            try
            {
                // Get member ID from claims
                var memberId = User.FindFirstValue("Id");
                if (memberId == null || !int.TryParse(memberId, out int parsedMemberId))
                {
                    return Unauthorized("Member ID not found in claims.");
                }

                // Assign the plan and auto-populate diaries
                await _exerciseDiaryDetailRepo.AssignExercisePlanToUserAsync(parsedMemberId, assignPlanDto.PlanId, assignPlanDto.StartDate);

                return Ok("Plan assigned successfully, and diary entries populated.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpGet("get-all-diaries-for-month-of-exercise")]
        public async Task<IActionResult> GetAllDiariesForMonthOfExercise([FromQuery] DateTime date)
        {
            try
            {
                var memberIdClaim = User.FindFirstValue("Id");
                if (memberIdClaim == null)
                {
                    return Unauthorized("Member ID not found in claims.");
                }

                if (!int.TryParse(memberIdClaim, out int memberId))
                {
                    return BadRequest("Invalid member ID.");
                }
                var diaries = await _exerciseDiaryDetailRepo.GetAllDiariesForMonthOfExercise(date, memberId);

                if (diaries == null || !diaries.Any())
                {
                    return NotFound("No diaries found for the specified month.");
                }

                return Ok(diaries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpPost("addExerciseListToDiaryForWebsite")]
        [Authorize]
        public async Task<IActionResult> addExerciseListToDiaryForWebsite([FromBody] AddExerciseDiaryDetailForWebsiteRequestDTO request)
        {
            var memberIdClaim = User.FindFirstValue("Id");
            if (memberIdClaim == null || !int.TryParse(memberIdClaim, out int memberId))
                return Unauthorized("Member ID not found in claims.");
            if (request == null)
            {
                return BadRequest("Invalid request: No food details provided.");
            }

            var result = await _exerciseDiaryDetailRepo.addExerciseListToDiaryForWebsite(request, memberId);

            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }
        
        
        [HttpPost("DeleteExerciseDiaryDetailWebsite")]
        [Authorize]
        public async Task<IActionResult> DeleteExerciseDiaryDetailWebsite(DateTime selectDate)
        {
            var memberIdClaim = User.FindFirstValue("Id");
            if (memberIdClaim == null || !int.TryParse(memberIdClaim, out int memberId))
                return Unauthorized("Member ID not found in claims.");
           

            var result = await _exerciseDiaryDetailRepo.DeleteExerciseDiaryDetailWebsite(selectDate, memberId);

            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }

        [Authorize]
        [HttpGet("Get-Exercise-dairy-detail-website")]
        public async Task<IActionResult> GetExerciseDairyDetailWebsite(DateTime selectDate)
        {

            var memberIdClaim = User.FindFirstValue("Id");
            if (memberIdClaim == null)
            {
                return Unauthorized("Member ID not found in claims.");
            }

            if (!int.TryParse(memberIdClaim, out int memberId))
            {
                return BadRequest("Invalid member ID.");
            }
            var mainDashBoardInfo = await _exerciseDiaryDetailRepo.GetExerciseDairyDetailWebsite(memberId, selectDate);
            if (mainDashBoardInfo == null)
            {
                return NotFound(" not found.");
            }
            return Ok(mainDashBoardInfo);
        }

        [HttpGet("get-list-box-exercise")]
        public async Task<IActionResult> GetListBoxFoodForStaff()
        {
            var listBoxExercise = await _exerciseDiaryDetailRepo.GetListBoxExerciseForStaffAsync();


            if (listBoxExercise == null || !listBoxExercise.Any())
            {
                return NotFound("No listBoxExercise found.");
            }


            return Ok(listBoxExercise);
        }

    }
}
