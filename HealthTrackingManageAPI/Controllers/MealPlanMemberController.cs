using BusinessObject.Dto.MealPlan;
using BusinessObject.Models;
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
        public async Task<IActionResult> AddMealPlanToDiary([FromBody]int mealPlanId, DateTime selectDate)
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

            var success = await _mealPlanRepository.AddMealPlanToFoodDiaryAsync(mealPlanId, memberId, selectDate);

            if (!success)
            {
                return StatusCode(500);
            }

            return Ok("");
        }

        [HttpPost("get-meal-plan-detail-for-member")]
        [Authorize]
        public async Task<IActionResult> GetMealPlanDetailForMember(int MealPlanId ,int Day)
        {
          

            if (Day <= 1) Day = 1;
            var success = await _mealPlanRepository.GetMealPlanDetailForMemberAsync(MealPlanId, Day);

            if (success == null)
            {
                return NotFound();
            }

            return Ok(success);

        }



        [HttpGet("search-meal-plan-for-member")]
        public async Task<IActionResult> SearchFoodsForMember([FromQuery] string? mealPlanName)
        {
            var mealPlan = await _mealPlanRepository.SearchMealPlanForMemberAsync(mealPlanName);

            return Ok(mealPlan);
        }

        [HttpPost("add-meal-plan-detail-with-day-to-food-diary")]
        public async Task<IActionResult> AddMealPlanDetailWithDayToFoodDiary([FromBody] AddMealPlanDetailDayToFoodDiaryDetailRequestDTO addMealPlanDetail)
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

            var success = await _mealPlanRepository.AddMealPlanDetailWithDayToFoodDiaryAsync(addMealPlanDetail,memberId);

            if (!success)
            {
                return StatusCode(500);
            }

            return Ok("");
        }

        [HttpPost("add-meal-plan-detail-with-meal-type-day-to-food-diary")]
        public async Task<IActionResult> AddMealPlanDetailWithMealTypeDayToFoodDiary([FromBody] AddMealPlanDetailMealTypeDayToFoodDiaryDetailRequestDTO addMealPlanDetail)
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

            var success = await _mealPlanRepository.AddMealPlanDetailWithMealTypeDayToFoodDiary(addMealPlanDetail, memberId);

            if (!success)
            {
                return StatusCode(500);
            }

            return Ok("");
        }


    }
}
