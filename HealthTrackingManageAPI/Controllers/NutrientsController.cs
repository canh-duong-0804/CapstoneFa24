using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepo;
using System.Security.Claims;

namespace HealthTrackingManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NutrientsController : ControllerBase
    {
        private readonly INutrientRepository _nutrientRepository;
        public NutrientsController(INutrientRepository nutrientRepository)
        {
            _nutrientRepository = nutrientRepository;
        }



        [HttpGet("daily-nutrition")]
        [Authorize]
        public async Task<IActionResult> GetDailyNutrition(DateTime date)
        {
            var memberIdClaim = User.FindFirstValue("Id");
            if (memberIdClaim == null || !int.TryParse(memberIdClaim, out int memberId))
                return Unauthorized("Member ID not found in claims.");

            try
            {
                // Retrieve daily nutrition for the specified member ID and date
                var dailyNutrition = await _nutrientRepository.CalculateDailyNutrition(memberId, date);

                if (dailyNutrition == null)
                    return NotFound("No daily nutrition found for the specified member and date.");

                return Ok(dailyNutrition);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
