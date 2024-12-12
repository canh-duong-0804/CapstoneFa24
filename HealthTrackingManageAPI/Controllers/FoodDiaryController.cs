using AutoMapper.Execution;
using BusinessObject;
using BusinessObject.Dto.FoodDiary;
using BusinessObject.Dto.FoodDiaryDetails;
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
    public class FoodDiaryController : ControllerBase
    {

        private readonly IFoodDiaryRepository _foodDiaryRepository;

        public FoodDiaryController(IFoodDiaryRepository foodDiaryService)
        {
            _foodDiaryRepository = foodDiaryService;
        }


        /*
                [HttpGet("getOrCreateDailyFoodDiary")]
                public async Task<IActionResult> GetOrCreateDailyFoodDiary(int memberId)
                {
                    DateTime date = DateTime.Now.Date;
                    var foodDiary = await _foodDiaryRepository.GetOrCreateFoodDiaryByDate(memberId, date);

                    var mapper = MapperConfig.InitializeAutomapper();
                    var foodDiaryModel = mapper.Map<FoodDiaryResponseDTO>(foodDiary);
                    return Ok(foodDiaryModel);
                }*/

        /* [HttpPut("updateDailyFoodDiary")]
         public async Task<IActionResult> UpdateDailyFoodDiary([FromBody] UpdateFoodDiaryRequestDTO updatedFoodDiary)
         {
             DateTime date = DateTime.Now.Date;

             var result = await _foodDiaryRepository.UpdateFoodDiary(updatedFoodDiary);
             if (result!=null)
             {
                 return Ok();
             }
             return NotFound();
         }*/

        [HttpPost("addFoodListToDiary")]
        [Authorize]
        public async Task<IActionResult> AddFoodListToDiary([FromBody] FoodDiaryDetailRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request: No food details provided.");
            }

            var result = await _foodDiaryRepository.AddFoodListToDiaryAsync(request);

            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }
        

        
        [HttpPost("addFoodListToDiaryForWebsite")]
        [Authorize]
        public async Task<IActionResult> addFoodListToDiaryForWebsite([FromBody] AddFoodDiaryDetailForWebsiteRequestDTO request)
        {
            var memberIdClaim = User.FindFirstValue("Id");
            if (memberIdClaim == null || !int.TryParse(memberIdClaim, out int memberId))
                return Unauthorized("Member ID not found in claims.");
            if (request == null)
            {
                return BadRequest("Invalid request: No food details provided.");
            }

            var result = await _foodDiaryRepository.addFoodListToDiaryForWebsite(request,memberId);

            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("Delete-food-diary-website")]
        [Authorize]
        public async Task<IActionResult> DeleteFoodDiaryWebsite(DateTime selectDate,int mealtype)
        {
            var memberIdClaim = User.FindFirstValue("Id");
            if (memberIdClaim == null || !int.TryParse(memberIdClaim, out int memberId))
                return Unauthorized("Member ID not found in claims.");
            

            var result = await _foodDiaryRepository.DeleteFoodDiaryWebsite(selectDate, memberId,mealtype);

            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }

        [Authorize]
        [HttpGet("Get-Food-dairy-detail-website")]
        public async Task<IActionResult> GetFoodDairyDetailWebsite(DateTime selectDate,int mealtype)
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
            var mainDashBoardInfo = await _foodDiaryRepository.GetFoodDairyDetailWebsite(memberId, selectDate,mealtype);
            if (mainDashBoardInfo == null)
            {
                return NotFound(" not found.");
            }
            return Ok(mainDashBoardInfo);
        }


        [HttpGet("get-all-diaries-for-month-with-meal-types")]
        public async Task<IActionResult> GetAllDiariesForMonthWithMealTypes([FromQuery] DateTime date)
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
                var diaries = await _foodDiaryRepository.GetAllDiariesForMonthWithMealTypesAsync(date,memberId);

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


        [HttpDelete("deleteFoodListFromDiary")]
        [Authorize]
        public async Task<IActionResult> DeleteFoodListFromDiary(int foodDiaryDetailId)
        {
            
            /*var currentUserId = User.FindFirstValue("Id");
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized();
            }

           
            var isOwner = await _foodDiaryRepository.CheckFoodDiaryOwnership(
                int.Parse(currentUserId),
                foodDiaryDetailId
            );

            if (!isOwner)
            {
                return Forbid(); 
            }*/

            var result = await _foodDiaryRepository.DeleteFoodListToDiaryAsync(foodDiaryDetailId);
            if (!result)
            {
                return NotFound($"Food item with ID {foodDiaryDetailId} not found.");
            }
            return NoContent();
        }

        [Authorize]
        [HttpGet("Get-Food-dairy-detail")]
        public async Task<IActionResult> GetFoodDairyDetailById(DateTime date)
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
            var mainDashBoardInfo = await _foodDiaryRepository.GetFoodDairyDetailById(memberId, date);
            if (mainDashBoardInfo == null)
            {
                return NotFound(" not found.");
            }
            return Ok(mainDashBoardInfo);
        }

        [Authorize]
        [HttpGet("getFoodHistory")]
        public async Task<IActionResult> GetFoodHistory()
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
            var foodHistory = await _foodDiaryRepository.GetFoodHistoryAsync(memberId);

           /* if (foodHistory == null || !foodHistory.Any())
            {
                return NotFound("No food history found.");
            }*/

            return Ok(foodHistory);

        }
        [Authorize]
        [HttpGet("get-food-suggestion")]
        public async Task<IActionResult> GetFoodSuggestion()
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
            var foodHistory = await _foodDiaryRepository.GetFoodSuggestionAsync(memberId);

           

            return Ok(foodHistory);

        }
        /*[Authorize]
        [HttpGet("Get-Food-dairy-date")]
        public async Task<IActionResult> GetFoodDairyDate()
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
            var mainDashBoardInfo = await _foodDiaryRepository.GetFoodDairyDateAsync(memberId);
            if (mainDashBoardInfo == null)
            {
                return NotFound(" not found.");
            }
            return Ok(mainDashBoardInfo);
        }
*/
        [HttpGet("get-streak")]
        [Authorize]
        public async Task<IActionResult> GetCalorieStreak(DateTime date)
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

           
            var streak = await _foodDiaryRepository.GetCalorieStreakAsync(memberId,date);
            if (streak == null)
            {
                return NotFound("No calorie streak found.");
            }
            return Ok(streak);
        }
        /* [Authorize]
         [HttpGet("Get-Food-dairy-detail-for-member-by-date")]
         public async Task<IActionResult> GetFoodDairyByDate(DateTime date)
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
             var mainDashBoardInfo = await _foodDiaryRepository.GetFoodDairyByDate(memberId, date);
             if (mainDashBoardInfo == null)
             {
                 return NotFound(" not found.");
             }
             return Ok(mainDashBoardInfo);
         }*/
    }
}
