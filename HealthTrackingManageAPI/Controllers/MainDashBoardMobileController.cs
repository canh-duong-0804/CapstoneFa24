using BusinessObject.Dto.Exericse;
using BusinessObject.Dto.MainDashBoardMobile;
using BusinessObject.Models;
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
    public class MainDashBoardMobileController : ControllerBase
    {


        private readonly IMainDashBoardRepository _mainDashBoardRepository;
        public MainDashBoardMobileController(IMainDashBoardRepository mainDashBoardRepository)
        {
            _mainDashBoardRepository = mainDashBoardRepository;
        }

        [Authorize]
        [HttpGet("Get-main-dashboard-for-member-by-id")]
        public async Task<IActionResult> GetMainDashBoardForMemberById()
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
            DateTime date = DateTime.Now.Date;

            var mainDashBoardInfo = await _mainDashBoardRepository.GetMainDashBoardForMemberById(memberId, date);
            if (mainDashBoardInfo == null)
            {
                return NotFound("Exercise not found.");
            }
            return Ok(mainDashBoardInfo);
        }

        [Authorize]
        [HttpGet("Get-info-calo-in-of-dashboard-for-member")]
        public async Task<IActionResult> GetInfoCaloInDashBoardForMemberById()
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
            DateTime date = DateTime.Now.Date;

            var mainDashBoardInfo = await _mainDashBoardRepository.GetInfoCaloInDashBoardForMemberById(memberId, date);
            if (mainDashBoardInfo == null)
            {
                return NotFound("Exercise not found.");
            }
            return Ok(mainDashBoardInfo);
        }

        /* [Authorize]
         [HttpGet("Get-Food-dairy-detail-for-member-by-id")]
         public async Task<IActionResult> GetFoodDairyDetailById()
         {
             DateTime date = DateTime.Now.Date;
             var memberIdClaim = User.FindFirstValue("Id");
             if (memberIdClaim == null)
             {
                 return Unauthorized("Member ID not found in claims.");
             }

             if (!int.TryParse(memberIdClaim, out int memberId))
             {
                 return BadRequest("Invalid member ID.");
             }
             var mainDashBoardInfo = await _mainDashBoardRepository.GetFoodDairyDetailById(memberId, date);
             if (mainDashBoardInfo == null)
             {
                 return NotFound(" not found.");
             }
             return Ok(mainDashBoardInfo);
         }


         [Authorize]
         [HttpGet("Get-Food-dairy-detail-for-member-by-date")]
         public async Task<IActionResult> GetFoodDairyByDate(DateTime date)
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
             var mainDashBoardInfo = await _mainDashBoardRepository.GetFoodDairyByDate(memberId, date);
             if (mainDashBoardInfo == null)
             {
                 return NotFound(" not found.");
             }
             return Ok(mainDashBoardInfo);
         }*/
    }
}
