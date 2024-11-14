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

        [HttpGet("get-goal-detail/{id}")]
        [Authorize]
        public async Task<IActionResult> GetGoalDetail(int id)
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

            var goal = await _goalRepository.GetGoalByIdAsync(id);
            if (goal == null || goal.MemberId != memberId)
            {
                return NotFound("Goal not found or does not belong to the authenticated user.");
            }

            var mapper = MapperConfig.InitializeAutomapper();
            var goalResponse = mapper.Map<GoalResponseDTO>(goal);

            return Ok(goalResponse);
        }


        [HttpPut("update-goal/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateGoal([FromBody] GoalResponseDTO updatedGoal)
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


            bool success = _goalRepository.updateGoal(memberId,updatedGoal);
            if (success)
                return Ok();
            else return BadRequest();



        }
    }
}
