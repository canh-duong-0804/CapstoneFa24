/*using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepo;
using System.Security.Claims;

namespace HealthTrackingManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WaterLogController : ControllerBase
    {
        private readonly IWaterLogRepository _waterLogRepository;
        public WaterLogController(IWaterLogRepository waterLogRepository)
        {
            _waterLogRepository = waterLogRepository;
        }

        [HttpPost("Add-200-ml-water")]
        public async Task<IActionResult> Add200ml()
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
            DateTime date = DateTime.Now;
            bool success = await _waterLogRepository.Add200mlAsync(memberId, date);
            if (success)
            {
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }

        }

        [HttpPost("Sub-200-ml-water")]
        public async Task<IActionResult> Subtract200ml()
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
            DateTime date = DateTime.Now;
            bool success = await _waterLogRepository.SubtractWaterIntakeAsync(memberId, date);

            if (success)
            {
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }

        }
    }
}
*/