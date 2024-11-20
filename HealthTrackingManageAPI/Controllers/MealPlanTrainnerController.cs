using BusinessObject;
using BusinessObject.Dto.MealPlan;
using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepo;
using System.Security.Claims;

namespace HealthTrackingManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealPlanTrainnerController : ControllerBase
    {
        private readonly IMealPlanRepository _mealPlanRepository;

        public MealPlanTrainnerController(IMealPlanRepository mealPlanRepository)
        {
            _mealPlanRepository = mealPlanRepository;
        }

        [HttpPost("create-meal-plan-by-trainner")]
        [Authorize]
        public async Task<IActionResult> CreateMealPlanTrainer([FromBody] CreateMealPlanRequestDTO request)
        {


            var memberIdClaim = User.FindFirstValue("Id");
            if (memberIdClaim == null)
            {
                return Unauthorized();
            }
            if (!int.TryParse(memberIdClaim, out int memberId))
            {
                return BadRequest();
            }
            var role = User.FindFirstValue(ClaimTypes.Role);

            var mapper = MapperConfig.InitializeAutomapper();
            var mealPlanModel = mapper.Map<BusinessObject.Models.MealPlan>(request);
            mealPlanModel.CreatedAt = DateTime.UtcNow;
            mealPlanModel.CreateBy = memberId;


            var success = await _mealPlanRepository.CreateMealPlanTrainerAsync(mealPlanModel);

            if (!success)
            {
                return StatusCode(500);
            }

            return Ok();
        } 
        
        
        
        [HttpPut("update-meal-plan-by-trainner")]
        [Authorize]
        public async Task<IActionResult> UpdateMealPlanTrainer([FromBody] UpdateMealPlanRequestDTO request)
        {


            var memberIdClaim = User.FindFirstValue("Id");
            if (memberIdClaim == null)
            {
                return Unauthorized();
            }
            if (!int.TryParse(memberIdClaim, out int memberId))
            {
                return BadRequest();
            }
            var role = User.FindFirstValue(ClaimTypes.Role);

            var mapper = MapperConfig.InitializeAutomapper();
            var mealPlanModel = mapper.Map<BusinessObject.Models.MealPlan>(request);
            mealPlanModel.ChangeDate = DateTime.UtcNow;
            mealPlanModel.ChangeBy = memberId;


            var success = await _mealPlanRepository.UpdateMealPlanTrainerAsync(mealPlanModel);

            if (!success)
            {
                return StatusCode(500);
            }

            return Ok();
        }

        [HttpDelete("delete-meal-plan-by-trainner")]
        [Authorize]
        public async Task<IActionResult> DeleteMealPlan(int mealPlanId)
        {


            var memberIdClaim = User.FindFirstValue("Id");
            if (memberIdClaim == null)
            {
                return Unauthorized();
            }
            if (!int.TryParse(memberIdClaim, out int memberId))
            {
                return BadRequest();
            }
            var role = User.FindFirstValue(ClaimTypes.Role);

            

            var success = await _mealPlanRepository.DeleteMealPlanAsync(mealPlanId);

            if (!success)
            {
                return StatusCode(500);
            }

            return Ok();
        }
        
        [HttpGet("get-meal-plan-by-trainner")]
        [Authorize]
        public async Task<IActionResult> GetMealPlan(int mealPlanId)
        {


            var memberIdClaim = User.FindFirstValue("Id");
            if (memberIdClaim == null)
            {
                return Unauthorized();
            }
            if (!int.TryParse(memberIdClaim, out int memberId))
            {
                return BadRequest();
            }
            var role = User.FindFirstValue(ClaimTypes.Role);

            

            var success = await _mealPlanRepository.GetMealPlanAsync(mealPlanId);

           /* if (!success)
            {
                return StatusCode(500);
            }*/

            return Ok();
        }



        [HttpGet("Get-all-meal-plan-for-staff")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> GetAllMealPlanForStaff([FromQuery] int? page)
        {


            var memberIdClaim = User.FindFirstValue("Id");
            if (memberIdClaim == null)
            {
                return Unauthorized();
            }
            if (!int.TryParse(memberIdClaim, out int memberId))
            {
                return BadRequest();
            }
            var role = User.FindFirstValue(ClaimTypes.Role);

            if (page.HasValue && page < 1)
            {
                return BadRequest("Page number must be greater than or equal to 1.");
            }

            int currentPage = page ?? 1;
            int currentPageSize = 5;

            int totalMealPlan = await _mealPlanRepository.GetTotalMealPlanAsync();

            if (totalMealPlan == 0)
            {
                return NotFound("No staff found.");
            }

            int totalPages = (int)Math.Ceiling(totalMealPlan / (double)currentPageSize);


            if (totalMealPlan < currentPageSize) currentPageSize = totalMealPlan;


            if (currentPage > totalPages && totalPages > 0) currentPage = totalPages;


            var mealPlans = await _mealPlanRepository.GetAllMealPlanForStaffsAsync(currentPage, currentPageSize);

            if (mealPlans == null || !mealPlans.Any())
            {
                return NotFound("No staff found.");
            }

            return Ok(new
            {
                mealPlans,
                TotalPages = totalPages,
                CurrentPage = currentPage,
                PageSize = currentPageSize
            });
        }

    }
}
