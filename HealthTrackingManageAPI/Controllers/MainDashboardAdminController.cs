using HealthTrackingManageAPI.Authorize;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepo;

namespace HealthTrackingManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainDashboardAdminController : ControllerBase
    {

        private readonly IMainDashboardAdminRepository _mainDashBoardAdminRepository;
        public MainDashboardAdminController(IMainDashboardAdminRepository mainDashBoardAdminRepository)
        {
            _mainDashBoardAdminRepository = mainDashBoardAdminRepository;
        }

        [RoleLessThanOrEqualTo(1)]
        [HttpGet("Get-main-dashboard-for-Admin")]
        public async Task<IActionResult> GetMainDashBoardForMemberById(DateTime SelectDate)
        {
            return Ok();
        }
    }
}
