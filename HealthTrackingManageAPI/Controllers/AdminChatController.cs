using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using BusinessObject.Models;
using Repository.IRepo;
using HealthTrackingManageAPI.Authorize;

namespace YourAPINamespace.Controllers
{
    [Authorize(Roles = "Admin")]
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


        [HttpGet("get-all-message-for-trainer-to-asign")]
        [RoleLessThanOrEqualTo(1)]
        public async Task<IActionResult> GetAllMessageForTrainerToAsign([FromBody] AssignStaffRequest request)
        {
            try
            {
                //var getAllMessageForAdmin= await _adminChatRepository.GetAllMessageForTrainerToAsign(request.ChatId, request.StaffId);
                return Ok(new { message = "Staff assigned to chat successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPost("send-message")]
        [RoleLessThanOrEqualTo(2)]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
        {
            try
            {
                await _adminChatRepository.SendMessageAsync(request.ChatId, request.StaffId, request.MessageContent);
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

    public class SendMessageRequest
    {
        public int ChatId { get; set; }
        public int StaffId { get; set; }
        public string MessageContent { get; set; }
    }
}
