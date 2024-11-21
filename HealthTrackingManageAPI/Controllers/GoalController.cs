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
            goalModel.TargetValue = goal.TargetWeight;
            goalModel.GoalType = goal.GoalType.ToString("F1");
            await _goalRepository.AddGoalAsync(goalModel, goal.Weight);
            return Ok(new { message = "Goal inserted successfully" });

        }

        [HttpGet("get-goal-detail")]
        [Authorize]
        public async Task<IActionResult> GetGoalDetail()
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

            var goal = await _goalRepository.GetGoalByIdAsync(memberId);
            /*  var body= goal.Member.BodyMeasureChanges.FirstOrDefault();
              var member= goal.Member.ExerciseLevel;*/
            if (goal == null)
            {
                return NotFound("Goal not found or does not belong to the authenticated user.");
            }



            return Ok(goal);
        }
        
        
        
        [HttpGet("get-info-goal-weight-member-for-graph-in-week")]
        [Authorize]
        public async Task<IActionResult> GetInforGoalWeightMemberForGraph(DateTime date)
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

            var goal = await _goalRepository.GetInforGoalWeightMemberForGraph(memberId,date);
           
        
            if (goal == null)
            {
                return NotFound("Goal not found or does not belong to the authenticated user.");
            }



            return Ok(goal);
        }
        
        
        [HttpGet("get-info-goal-weight-member-for-graph-in-month")]
        [Authorize]
        public async Task<IActionResult> GetInforGoalWeightMemberForGraphInMonth(DateTime date)
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

            var goal = await _goalRepository.GetInforGoalWeightMemberForGraphInMonth(memberId,date);
           
        
            if (goal == null)
            {
                return NotFound("Goal not found or does not belong to the authenticated user.");
            }



            return Ok(goal);
        }





        /*[HttpPost("update-goal")]
        [Authorize]
        public async Task<IActionResult> UpdateGoal([FromBody] GoalRequestDTO updatedGoal)
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


            var success = await _goalRepository.UpdateGoalAsync(memberId, updatedGoal);

            if (!success)
            {
                return StatusCode(500, "An error occurred while updating the goal.");
            }

            return Ok("Goal updated successfully.");
        }




        [HttpPost("add-current-weight")]
        [Authorize]
        public async Task<IActionResult> AddCurrentWeight(double weightCurrent)
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

            var success = await _goalRepository.AddCurrentWeightAsync(memberId, weightCurrent);

            if (!success)
            {
                return StatusCode(500);
            }

            return Ok();
        }


        [HttpPost("add-goal-weight")]
        [Authorize]
        public async Task<IActionResult> AddGoalWeight(double weightCurrent)
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

            var success = await _goalRepository.AddGoalWeightAsync(memberId, weightCurrent);

            if (!success)
            {
                return StatusCode(500);
            }

            return Ok();
        }
        [HttpPost("add-goal-week-daily")]
        [Authorize]
        public async Task<IActionResult> AddGoalWeekDaily(string goalWeekDaily)
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

            var success = await _goalRepository.AddGoalWeekDaily(memberId, goalWeekDaily);

            if (!success)
            {
                return StatusCode(500);
            }

            return Ok();
        }

        [HttpPost("add-level-goal-exercise")]
        [Authorize]
        public async Task<IActionResult> AddGoalLevelExercise(string goalWeekDaily)
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

            var success = await _goalRepository.AddGoalLevelExercise(memberId, goalWeekDaily);

            if (!success)
            {
                return StatusCode(500);
            }

            return Ok();
        }*/
    }
}