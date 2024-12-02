using HealthTrackingManageAPI.Authorize;
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
    public class MainDashboardForAdminManageController : ControllerBase
    {

        private readonly IMainDashboardAdminRepository _mainDashBoardTrainerRepository;
        public MainDashboardForAdminManageController(IMainDashboardAdminRepository mainDashBoardTrainerRepository)
        {
            _mainDashBoardTrainerRepository = mainDashBoardTrainerRepository;
        }

        [RoleLessThanOrEqualTo(1)]
        [HttpGet("Get-main-dashboard-for-Main-Trainer")]
        public async Task<IActionResult> GetMainDashBoardForMemberById1(DateTime SelectDate)
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
    }
}
