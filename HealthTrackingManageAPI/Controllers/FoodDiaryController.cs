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

        [HttpDelete("deleteFoodListFromDiary/{id}")]
        public async Task<IActionResult> DeleteFoodListFromDiary(int id)
        {




            var result = await _foodDiaryRepository.DeleteFoodListToDiaryAsync(id);

            if (!result)
            {
                return NotFound($"Food item with ID {id} not found.");
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

            if (foodHistory == null || !foodHistory.Any())
            {
                return NotFound("No food history found.");
            }

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

            if (foodHistory == null || !foodHistory.Any())
            {
                return NotFound("No food history found.");
            }

            return Ok(foodHistory);

        }
        [Authorize]
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
