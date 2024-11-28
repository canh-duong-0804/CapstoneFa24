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
        public async Task<ActionResult<MessageChat>> CreateChat([FromBody] MemberCreateChatRequest request)
        {
            try
            {
               
                int memberId = GetCurrentMemberId();

                var chat = await _chatRepository.CreateChatAsync(memberId, request.InitialMessage);
                return Ok(new
                {
                    ChatId = chat.MessageChatId,
                    Message = "Chat created successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Member retrieves their chat history
        [HttpGet("my-chats")]
        public async Task<ActionResult<List<MessageChat>>> GetMemberChats()
        {
            try
            {
                int memberId = GetCurrentMemberId();

                var chats = await _chatRepository.GetMemberChatsAsync(memberId);
                return Ok(chats);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

       
        [HttpGet("chat-details/{chatId}")]
        public async Task<ActionResult<MessageChat>> GetChatDetails(int chatId)
        {
            try
            {
                int memberId = GetCurrentMemberId();

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
                int memberId = GetCurrentMemberId();

                await _chatRepository.RateChatAsync(memberId, request.ChatId, request.RatingStar);
                return Ok(new { message = "Chat rated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    
        private int GetCurrentMemberId()
        {
            
            var memberId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(memberId))
                throw new UnauthorizedAccessException("Member not authenticated");

            return int.Parse(memberId);
        }
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