using HealthTrackingManageAPI.Authorize;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepo;
using System.Security.Claims;

namespace HealthTrackingManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainDashboardForTrainerManageController : ControllerBase
    {
        private readonly IMainDashboardTrainerManageRepository _mainDashBoardTrainerRepository;
        public MainDashboardForTrainerManageController(IMainDashboardTrainerManageRepository mainDashBoardTrainerRepository)
        {
            _mainDashBoardTrainerRepository = mainDashBoardTrainerRepository;
        }

        [RoleLessThanOrEqualTo(1)]
        [HttpGet("Get-main-dashboard-for-Main-Trainer")]
        public async Task<IActionResult> GetAllInformationForMainTrainer(DateTime SelectDate)
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


            var mainDashBoardInfo = await _mainDashBoardTrainerRepository.GetAllInformationForMainTrainer(SelectDate);


            return Ok(mainDashBoardInfo);
        }
        
        
        [RoleEqualToAttribute(2)]
        [HttpGet("Get-main-dashboard-for-Food-Trainer")]
        public async Task<IActionResult> GetMainDashBoardForFoodTrainer(DateTime SelectDate)
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


            var mainDashBoardInfo = await _mainDashBoardTrainerRepository.GetMainDashBoardForFoodTrainer(SelectDate);


            return Ok(mainDashBoardInfo);
        }
        
        
        [RoleEqualToAttribute(3)]
        [HttpGet("Get-main-dashboard-for-Exercise-Trainer")]
        public async Task<IActionResult> GetMainDashBoardForExerciseTrainer(DateTime SelectDate)
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


            var mainDashBoardInfo = await _mainDashBoardTrainerRepository.GetMainDashBoardForExerciseTrainer(SelectDate);


            return Ok(mainDashBoardInfo);
        }
    }
}
