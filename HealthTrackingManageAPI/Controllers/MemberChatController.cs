using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using BusinessObject.Models;
using System.Security.Claims;
using Repository.IRepo;

namespace YourAPINamespace.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/member/[controller]")]
    public class MemberChatController : ControllerBase
    {
        private readonly IChatMemberRepository _chatRepository;

        public MemberChatController(IChatMemberRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }


        [HttpPost("create-chat")]
        public async Task<IActionResult> CreateChat([FromBody] MemberCreateChatRequest request)
        {
            try
            {

                var memberIdClaim = User.FindFirstValue("Id");
                if (memberIdClaim == null)
                {
                    return Unauthorized();
                }

                if (!int.TryParse(memberIdClaim, out int memberId))
                {
                    return BadRequest();
                }

                await _chatRepository.CreateChatAsync(memberId, request.InitialMessage);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequestMember request)
        {
            try
            {
                var memberIdClaim = User.FindFirstValue("Id");
                if (memberIdClaim == null)
                {
                    return Unauthorized();
                }

                if (!int.TryParse(memberIdClaim, out int memberId))
                {
                    return BadRequest("Invalid member ID.");
                }


                await _chatRepository.SendMessageMemberAsync(memberId, request.ChatId, request.MessageContent);

                return Ok(new { message = "Message sent successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        // Member retrieves their chat history
        [HttpGet("my-chats")]
        public async Task<ActionResult> GetMemberChats()
        {
            try
            {
                var memberIdClaim = User.FindFirstValue("Id");
                if (memberIdClaim == null)
                {
                    return Unauthorized();
                }

                if (!int.TryParse(memberIdClaim, out int memberId))
                {
                    return BadRequest();
                }

                var chats = await _chatRepository.GetMemberChatsAsync(memberId);
                return Ok(chats);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpGet("chat-details/{chatId}")]
        public async Task<ActionResult> GetChatDetails(int chatId)
        {
            try
            {
                var memberIdClaim = User.FindFirstValue("Id");
                if (memberIdClaim == null)
                {
                    return Unauthorized();
                }

                if (!int.TryParse(memberIdClaim, out int memberId))
                {
                    return BadRequest();
                }

                var chat = await _chatRepository.GetMemberChatDetailsAsync(memberId, chatId);
                if (chat == null)
                    return Unauthorized();

                return Ok(chat);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPost("rate-chat")]
        public async Task<ActionResult> RateChatInteraction([FromBody] MemberChatRatingRequest request)
        {
            try
            {
                var memberIdClaim = User.FindFirstValue("Id");
                if (memberIdClaim == null)
                {
                    return Unauthorized();
                }

                if (!int.TryParse(memberIdClaim, out int memberId))
                {
                    return BadRequest();
                }

                await _chatRepository.RateChatAsync(memberId, request.ChatId, request.RatingStar);
                return Ok(new { message = "Chat rated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



    }



    public class SendMessageRequestMember
    {
        public int ChatId { get; set; } // ID của đoạn chat
        public string MessageContent { get; set; } = null!; // Nội dung tin nhắn
    }

    public class MemberCreateChatRequest
    {
        public string InitialMessage { get; set; }
    }

    public class MemberChatRatingRequest
    {
        public int ChatId { get; set; }
        public double RatingStar { get; set; }
    }
}