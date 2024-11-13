using BusinessObject;
using BusinessObject.Dto.Goal;
using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepo;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace HealthTrackingManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoalController : ControllerBase
    {
        private readonly IGoalRepository _goalRepository;

        public GoalController(IGoalRepository goalRepository)
        {
            _goalRepository = goalRepository;
        }

        [HttpPost("add-goal")]
        [Authorize]
        public async Task<IActionResult> InsertGoal([FromBody] GoalRequestDTO goal)
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
            var mapper = MapperConfig.InitializeAutomapper();
            var goalModel = mapper.Map<Goal>(goal);
            goalModel.MemberId = memberId;

            await _goalRepository.AddGoalAsync(goalModel);
                return Ok(new { message = "Goal inserted successfully" });
          
        }

    }
}
