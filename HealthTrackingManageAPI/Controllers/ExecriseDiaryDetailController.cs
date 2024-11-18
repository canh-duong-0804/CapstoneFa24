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



    }
}
