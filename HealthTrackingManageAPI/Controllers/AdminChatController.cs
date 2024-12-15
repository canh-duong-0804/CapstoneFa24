using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using BusinessObject.Models;
using Repository.IRepo;
using HealthTrackingManageAPI.Authorize;
using System.Security.Claims;

namespace YourAPINamespace.Controllers
{

    [ApiController]
    [Route("api/admin/[controller]")]
    public class AdminChatController : ControllerBase
    {
        private readonly IAdminChatRepository _adminChatRepository;

        public AdminChatController(IAdminChatRepository adminChatRepository)
        {
            _adminChatRepository = adminChatRepository;
        }


        [HttpPost("assign-staff")]
        [RoleLessThanOrEqualTo(1)]
        public async Task<IActionResult> AssignStaffToChat([FromBody] AssignStaffRequest request)
        {
            try
            {
                await _adminChatRepository.AssignStaffToChatAsync(request.ChatId, request.StaffId);
                return Ok(new { message = "Staff assigned to chat successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        
        [HttpGet("get-all-trainer-to-assign")]
        [RoleLessThanOrEqualTo(1)]
        public async Task<IActionResult> GetAllTrainerToAssign()
        {
            try
            {
              var allTrainer =  await _adminChatRepository.GetAllTrainerToAssign();
                return Ok(allTrainer);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpGet("get-all-message-for-trainer-to-asign")]
        [RoleLessThanOrEqualTo(3)]
        public async Task<IActionResult> GetAllMessageForTrainerToAsign(int ChatId)
        {
            try
            {
                var memberIdClaim = User.FindFirstValue("Id");
                if (memberIdClaim == null)
                {
                    return Unauthorized("Member ID not found in claims.");
                }

                if (!int.TryParse(memberIdClaim, out int StaffId))
                {
                    return BadRequest("Invalid member ID.");
                }
                var getAllMessageForAdmin = await _adminChatRepository.GetAllMessageForTrainerToAsign(ChatId, StaffId);
                return Ok(getAllMessageForAdmin);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        
        [HttpGet("overview-all-message-of trainer")]
        [RoleLessThanOrEqualTo(3)]
        public async Task<IActionResult> OverviewAllMessageOfTrainer()
        {
            try
            {
                var memberIdClaim = User.FindFirstValue("Id");
                if (memberIdClaim == null)
                {
                    return Unauthorized("Member ID not found in claims.");
                }

                if (!int.TryParse(memberIdClaim, out int StaffId))
                {
                    return BadRequest("Invalid member ID.");
                }
                var getAllMessageForAdmin = await _adminChatRepository.OverviewAllMessageOfTrainer(StaffId);
                return Ok(getAllMessageForAdmin);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        /*[HttpGet("get-all-message-chat-for-trainer-Asign")]
        [RoleLessThanOrEqualTo(3)]
        public async Task<IActionResult> GetAllMessageChatForTrainerToAsign(int ChatId)
        {
            try
            {
                var memberIdClaim = User.FindFirstValue("Id");
                if (memberIdClaim == null)
                {
                    return Unauthorized("Member ID not found in claims.");
                }

                if (!int.TryParse(memberIdClaim, out int StaffId))
                {
                    return BadRequest("Invalid member ID.");
                }
                var getAllMessageForAdmin = await _adminChatRepository.GetAllMessageForTrainerToAsign(ChatId, StaffId);
                return Ok(getAllMessageForAdmin);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }*/


        [HttpGet("get-all-message-chat-for-trainer-need-assign")]
        [RoleLessThanOrEqualTo(3)]
        public async Task<IActionResult> GetAllMessageChatForTrainerNeedAsign(int pageNumber)
        {
            try
            {
                int pageSize = 5;
                var memberIdClaim = User.FindFirstValue("Id");
                if (memberIdClaim == null)
                {
                    return Unauthorized("Member ID not found in claims.");
                }

                if (!int.TryParse(memberIdClaim, out int StaffId))
                {
                    return BadRequest("Invalid member ID.");
                }

                // Gọi phương thức repository với các tham số phân trang
                var getAllMessageForAdmin = await _adminChatRepository.GetAllMessageChatForTrainerNeedAsign(pageNumber, pageSize);

                // Trả về kết quả thành công
                return Ok(getAllMessageForAdmin);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        [HttpPost("send-message")]
        [RoleLessThanOrEqualTo(3)]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
        {
            try
            {

                var memberIdClaim = User.FindFirstValue("Id");
                if (memberIdClaim == null)
                {
                    return Unauthorized();
                }

                if (!int.TryParse(memberIdClaim, out int StaffId))
                {
                    return BadRequest();
                }
                await _adminChatRepository.SendMessageAsync(request.ChatId, StaffId, request.MessageContent);
                return Ok(new { message = "Message sent successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }


    public class AssignStaffRequest
    {
        public int ChatId { get; set; }
        public int StaffId { get; set; }
    }

    public class GetMessageForStaffRequest
    {
        public int ChatId { get; set; }
        // public int StaffId { get; set; }
    }

    public class SendMessageRequest
    {
        public int ChatId { get; set; }
        // public int StaffId { get; set; }
        public string MessageContent { get; set; }
    } 
    
    
}
