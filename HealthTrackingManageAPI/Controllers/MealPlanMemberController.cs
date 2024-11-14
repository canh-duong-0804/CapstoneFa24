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
    public class MealPlanMemberController : ControllerBase
    {
        private readonly IMealPlanRepository _mealPlanRepository;

        public MealPlanMemberController(IMealPlanRepository mealPlanRepository)
        {
            _mealPlanRepository = mealPlanRepository;
        }


        [HttpGet("get-all-meal-plans")]
        [Authorize]
        public async Task<IActionResult> GetAllMealPlansForMember()
        {


            var mealPlans = await _mealPlanRepository.GetAllMealPlansForMemberAsync();

            if (mealPlans == null || !mealPlans.Any())
            {
                return NotFound("No meal plans found for this member.");
            }

            return Ok(mealPlans);
        }


        [HttpPost("add-meal-plan-to-diary")]
        [Authorize]
        public async Task<IActionResult> AddMealPlanToDiary(int mealPlanId)
        {
            /* if (request == null || request.FoodDiaryDetails == null || !request.FoodDiaryDetails.Any())
             {
                 return BadRequest("Invalid request data.");
             }*/

            var memberIdClaim = User.FindFirstValue("Id");
            if (memberIdClaim == null)
            {
                return Unauthorized();
            }
            if (!int.TryParse(memberIdClaim, out int memberId))
            {
                return BadRequest();
            }

            var success = await _mealPlanRepository.AddMealPlanToFoodDiaryAsync(mealPlanId, memberId);

            if (!success)
            {
                return StatusCode(500, "An error occurred while adding the meal plan to the diary.");
            }

            return Ok("Meal plan successfully added to the diary.");
        }

        [HttpGet("get-meal-plan-detail-for-member")]
        [Authorize]
        public async Task<IActionResult> GetMealPlanDetailForMember(int mealPlanId, int day)
        {
            if (day <= 1) day = 1;
            var success = await _mealPlanRepository.GetMealPlanDetailForMemberAsync(mealPlanId, day);

            if (success==null)
            {
                return NotFound();
            }

            return Ok(success);

        }



        [HttpGet("search-meal-plan-for-member")]
        public async Task<IActionResult> SearchFoodsForMember(string mealPlanName)
        {
            var foods = await _mealPlanRepository.SearchMealPlanForMemberAsync(mealPlanName);

            return Ok(foods);
        }
    }
}
