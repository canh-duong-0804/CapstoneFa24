using BusinessObject.Dto.Exericse;
using BusinessObject.Dto.MainDashBoardMobile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepo;
using Repository.Repo;

namespace HealthTrackingManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainDashBoardMobileController : ControllerBase
    {


        private readonly IMainDashBoardRepository _mainDashBoardRepository;
        public MainDashBoardMobileController(IMainDashBoardRepository mainDashBoardRepository)
        {
            _mainDashBoardRepository = mainDashBoardRepository;
        }
        [HttpGet("Get-main-dashboard-for-member-by-id/{id}")]
        public async Task<IActionResult> GetMainDashBoardForMemberById(int id)
        {

            var mainDashBoardInfo = await _mainDashBoardRepository.GetMainDashBoardForMemberById(id);
            if (mainDashBoardInfo == null)
            {
                return NotFound("Exercise not found.");
            }
            return Ok(mainDashBoardInfo);
        }
    }
}
