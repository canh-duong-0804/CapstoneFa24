using BusinessObject.Dto.ExecrisePlan;

using BusinessObject.Models;
using HealthTrackingManageAPI.Authorize;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepo;
using Repository.Repo;
using System.Security.Claims;

namespace HealthTrackingManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExecrisePlanTrainerController : ControllerBase
    {
        private readonly IExecrisePlanTrainerRepository _exercisePlanRepo;

        public ExecrisePlanTrainerController(IExecrisePlanTrainerRepository exercisePlanRepo)
        {
            _exercisePlanRepo = exercisePlanRepo;
        }
        [HttpGet("get-all-exercise-plans")]
        public async Task<IActionResult> GetAllExercisePlans([FromQuery] int page)
        {
            try
            {
                int pageSize = 10;
                if (page <= 0 || pageSize <= 0)
                {
                    return BadRequest("Page and pageSize must be greater than zero.");
                }

                // Lấy danh sách phân trang từ repository
                var pagedResult = await _exercisePlanRepo.GetAllExercisePlansAsync(page, pageSize);

                if (pagedResult == null || !pagedResult.Data.Any())
                {
                    return NotFound("No exercise plans found.");
                }

                return Ok(pagedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpPost("create-exercise-plan")]
        [RoleLessThanOrEqualTo(1)]
        [Authorize]
        public async Task<IActionResult> CreateExercisePlan([FromBody] CreateExercisePlanRequestDTO request)
        {
            var userId = User.FindFirstValue("Id");
            if (!int.TryParse(userId, out int memberId))
                return Unauthorized();

            var newPlan = new ExercisePlan
            {
                Name = request.Name,
                TotalCaloriesBurned = request.TotalCaloriesBurned,
                ExercisePlanImage = request.ExercisePlanImage,
                Status = request.Status,
                CreateBy = memberId,
                CreateDate = DateTime.Now
            };

            var success = await _exercisePlanRepo.AddExercisePlanAsync(newPlan);

            if (!success)
                return StatusCode(500, "Failed to create exercise plan.");

            return Ok(new { Message = "Exercise plan created successfully." });
        }

        [HttpPut("update-exercise-plan")]
        [RoleLessThanOrEqualTo(1)]
        [Authorize]
        public async Task<IActionResult> UpdateExercisePlan([FromBody] UpdateExercisePlanRequestDTO request)
        {
            var userId = User.FindFirstValue("Id");
            if (!int.TryParse(userId, out int memberId))
                return Unauthorized();

            var existingPlan = await _exercisePlanRepo.GetExercisePlanByIdAsync(request.ExercisePlanId);
            if (existingPlan == null)
                return NotFound("Exercise plan not found.");

            existingPlan.Name = request.Name;
            existingPlan.TotalCaloriesBurned = request.TotalCaloriesBurned;
            existingPlan.ExercisePlanImage = request.ExercisePlanImage;
            existingPlan.Status = request.Status;
            existingPlan.ChangeBy = memberId;
            existingPlan.ChangeDate = DateTime.Now;

            var success = await _exercisePlanRepo.UpdateExercisePlanAsync(existingPlan);

            if (!success)
                return StatusCode(500, "Failed to update exercise plan.");

            return Ok(new { Message = "Exercise plan updated successfully." });
        }



        [HttpGet("get-exercise-plan-detail")]
        [RoleLessThanOrEqualTo(1)]
        [Authorize]
        public async Task<IActionResult> GetExercisePlanDetail([FromQuery] int exercisePlanId, [FromQuery] int day)
        {
            try
            {
                if (exercisePlanId <= 0 || day <= 0)
                {
                    return BadRequest("Invalid exercise plan ID or day.");
                }

                // Lấy chi tiết kế hoạch bài tập từ repository
                var exercisePlanDetail = await _exercisePlanRepo.GetExercisePlanDetailAsync(exercisePlanId, day);

                if (exercisePlanDetail == null)
                {
                    return NotFound("Exercise plan detail not found.");
                }

                return Ok(exercisePlanDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }



        [HttpDelete("delete-exercise-plan/{planId}")]
        [RoleLessThanOrEqualTo(1)]
        [Authorize]
        public async Task<IActionResult> DeleteExercisePlan(int planId)
        {
            var success = await _exercisePlanRepo.SoftDeleteExercisePlanAsync(planId);
            if (!success)
                return StatusCode(500, "Failed to delete exercise plan.");

            return Ok(new { Message = "Exercise plan deleted successfully." });
        }

        [HttpPost("create-exercise-plan-detail")]
        [RoleLessThanOrEqualTo(1)]
        [Authorize]
        public async Task<IActionResult> CreateExercisePlanDetail([FromBody] CreateExercisePlanDetailRequestDTO request)
        {
            if (request.ExecriseInPlans == null || !request.ExecriseInPlans.Any())
            {
                return BadRequest("Exercise plan details cannot be empty.");
            }

            // Prepare list to store the details that will be added
            var detailsToAdd = new List<ExercisePlanDetail>();

            foreach (var exerciseInPlan in request.ExecriseInPlans)
            {
                var newDetail = new ExercisePlanDetail
                {
                    ExercisePlanId = request.ExercisePlanId,
                    ExerciseId = exerciseInPlan.ExerciseId,
                    Day = request.Day,
                    Duration = exerciseInPlan.Duration
                };

                detailsToAdd.Add(newDetail);
            }

            var success = await _exercisePlanRepo.AddExercisePlanDetailAsync(detailsToAdd);

            if (!success)
            {
                return StatusCode(500, "Failed to create exercise plan detail.");
            }

            return Ok(new { Message = "Exercise plan details created successfully." });
        }


        

        
        [HttpDelete("delete-exercise-plan-detail/{detailId}")]
        [RoleLessThanOrEqualTo(1)]
        [Authorize]
        public async Task<IActionResult> DeleteExercisePlanDetail(int detailId)
        {
            var success = await _exercisePlanRepo.DeleteExercisePlanDetailAsync(detailId);
            if (!success)
                return StatusCode(500, "Failed to delete exercise plan detail.");

            return Ok(new { Message = "Exercise plan detail deleted successfully." });
        }


        [HttpPut("update-exercise-plan-detail")]
        public async Task<IActionResult> UpdateExercisePlanDetail([FromBody] GetExercisePlanDetailDTO request)
        {
            try
            {
               

                var result = await _exercisePlanRepo.UpdateExercisePlanDetailAsync(request);

                if (!result)
                {
                    return NotFound("Failed to update exercise plan detail. The provided data may not exist.");
                }

                return Ok("Exercise plan detail updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }



    }
}
