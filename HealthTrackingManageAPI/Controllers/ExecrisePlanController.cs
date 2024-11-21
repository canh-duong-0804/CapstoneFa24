using BusinessObject.Dto.ExecrisePlan;
using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepo;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HealthTrackingManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExercisePlanController : ControllerBase
    {
        private readonly IExecrisePlanRepository _repository;

        public ExercisePlanController(IExecrisePlanRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("Add")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> AddExercisePlan([FromBody] AddExercisePlanDTO addDto)
        {
            try
            {
                // Retrieve the user ID from claims
                var userIdString = User.FindFirstValue("Id");

                if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                {
                    return Unauthorized("Invalid or missing User ID in claims.");
                }

                // Map DTO to domain model
                var plan = new ExercisePlan
                {
                    Name = addDto.Name,
                    ExercisePlanImage = addDto.ExercisePlanImage,
                    CreateBy = userId, // Set CreateBy using the converted user ID
                    CreateDate = DateTime.Now,
                    TotalCaloriesBurned = addDto.TotalCaloriesBurned,
                    ExercisePlanDetails = addDto.Details.Select(d => new ExercisePlanDetail
                    {
                        ExerciseId = d.ExerciseId,
                        Day = d.Day,
                        Duration = d.Duration
                    }).ToList()
                };

                await _repository.AddExecrisePlanAsync(plan);
                return Ok("Exercise Plan added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllExercisePlans()
        {
            try
            {
                // Fetch domain models and map to DTOs
                var plans = await _repository.GetExecrisePlansAsync();
                var dtos = plans.Select(plan => new ExercisePlanDTO
                {
                    ExercisePlanId = plan.ExercisePlanId,
                    Name = plan.Name,
                    TotalCaloriesBurned = plan.TotalCaloriesBurned,
                    ExercisePlanImage = plan.ExercisePlanImage,
                    Details = plan.ExercisePlanDetails.Select(d => new ExercisePlanDetailDTO
                    {
                        ExercisePlanDetailId = d.ExercisePlanDetailId,
                        ExerciseId = d.ExerciseId,
                        ExerciseName = d.Exercise.ExerciseName, // Include related data
                        Day = d.Day,
                        Duration = d.Duration
                    }).ToList()
                }).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("Get/{planId}")]
        public async Task<IActionResult> GetExercisePlanById(int planId)
        {
            try
            {
                var plan = await _repository.GetExecrisePlanByIdAsync(planId);

                if (plan == null)
                {
                    return NotFound("Exercise Plan not found.");
                }

                // Map domain model to DTO
                var dto = new ExercisePlanDTO
                {
                    ExercisePlanId = plan.ExercisePlanId,
                    Name = plan.Name,
                    TotalCaloriesBurned = plan.TotalCaloriesBurned,
                    Details = plan.ExercisePlanDetails.Select(d => new ExercisePlanDetailDTO
                    {
                        ExercisePlanDetailId = d.ExercisePlanDetailId,
                        ExerciseId = d.ExerciseId,
                        ExerciseName = d.Exercise.ExerciseName,
                        Day = d.Day,
                        Duration = d.Duration
                    }).ToList()
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("Update/{planId}")]
        public async Task<IActionResult> UpdateExercisePlan(int planId, [FromBody] AddExercisePlanDTO updateDto)
        {
            try
            {
                var existingPlan = await _repository.GetExecrisePlanByIdAsync(planId);
                if (existingPlan == null)
                {
                    return NotFound("Exercise Plan not found.");
                }

                // Update domain model with DTO values
                existingPlan.Name = updateDto.Name;
                existingPlan.TotalCaloriesBurned = updateDto.TotalCaloriesBurned;
                existingPlan.ExercisePlanImage = updateDto.ExercisePlanImage;

                await _repository.UpdateExecrisePlanAsync(existingPlan);
                return Ok("Exercise Plan updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("Delete/{planId}")]
        public async Task<IActionResult> DeleteExercisePlan(int planId)
        {
            try
            {
                await _repository.SoftDeleteExecrisePlanAsync(planId);
                return Ok("Exercise Plan deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
