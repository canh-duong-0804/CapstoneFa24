using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepo;
using System.Security.Claims;

namespace HealthTrackingManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MacroController : ControllerBase
    {
        private readonly IMacroRepository _macroRepository;
        public MacroController(IMacroRepository macroRepository)
        {
            _macroRepository = macroRepository;
        }


        [HttpGet("daily-macros")]
        [Authorize]
        public async Task<IActionResult> GetDailyMacros(DateTime date)
        {
            var memberIdClaim = User.FindFirstValue("Id");
            if (memberIdClaim == null || !int.TryParse(memberIdClaim, out int memberId))
                return Unauthorized("Member ID not found in claims.");

            try
            {
                // Retrieve daily macros for the specified member ID and date
                var dailyMacros = await _macroRepository.GetMacroNutrientsByDate(memberId, date);

                if (dailyMacros == null)
                    return NotFound("No daily macros found for the specified member and date.");

                return Ok(dailyMacros);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
