using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepo;
using System.Security.Claims;

namespace HealthTrackingManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaloriesController : ControllerBase
    {
        private readonly ICaloriesRepository _caloriesRepository;
        public CaloriesController(ICaloriesRepository caloriesRepository)
        {
            _caloriesRepository = caloriesRepository;
        }

        [HttpGet("daily-calories")]
        [Authorize]
        public async Task<IActionResult> GetDailyCalories(DateTime date)
        {
            var memberIdClaim = User.FindFirstValue("Id");
            if (memberIdClaim == null || !int.TryParse(memberIdClaim, out int memberId))
                return Unauthorized("Member ID not found in claims.");

            try
            {
                // Retrieve daily calories for the specified member ID and date
                var dailyCalories = await _caloriesRepository.CalculateDailyCalories(memberId, date);

                if (dailyCalories == null)
                    return NotFound("No daily calories found for the specified member and date.");

                return Ok(dailyCalories);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
