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
                    ExercisePlanImage = plan.ExercisePlanImage,
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

        [HttpGet("Search")]
        public async Task<IActionResult> SearchExercisePlansByName([FromQuery] string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest("Search term cannot be empty.");
                }

                // Fetch matching exercise plans and map to DTOs
                var plans = await _repository.SearchExercisePlansByNameAsync(name);

                if (!plans.Any())
                {
                    return NotFound("No matching exercise plans found.");
                }

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


    }
}
