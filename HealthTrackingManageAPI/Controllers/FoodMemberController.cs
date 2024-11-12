using BusinessObject;
using BusinessObject.Dto.FoodMember;
using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepo;
using System.Security.Claims;

namespace HealthTrackingManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodMemberController : ControllerBase
    {
        private readonly IFoodMemberRepository _foodMemberRepository;
        public FoodMemberController(IFoodMemberRepository foodMemberRepository)
        {
            _foodMemberRepository = foodMemberRepository;
        }

        [HttpGet("Get-all-food-member-by-id")]
        [Authorize]
        public async Task<IActionResult> GetAllFoodMemberById()
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
            var foodMember = await _foodMemberRepository.GetAllFoodMemberByIdAsync(memberId);

            if (foodMember == null)
            {
                return NotFound(new { Message = "Food member not found." });
            }

            return Ok(foodMember);
        }
        [HttpGet("Get-food-member-detail-by-food-member-id/{foodMemberId}")]
        [Authorize]
        public async Task<IActionResult> GetFoodMemberDetailById(int foodMemberId)
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
            var foodMember = await _foodMemberRepository.GetFoodMemberDetailByIdAsync(foodMemberId, memberId);
            if (foodMember == null)
            {
                return NotFound(new { Message = "Food member not found." });
            }

            return Ok(foodMember);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateFoodMember([FromBody] CreateFoodMemberRequestDTO createFoodMemberDto)
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

           





            var mapper = MapperConfig.InitializeAutomapper();
            var foodMemberModel = mapper.Map<BusinessObject.Models.FoodMember>(createFoodMemberDto);
            foodMemberModel.CreatedBy = memberId;
            foodMemberModel.CreatedAt = DateTime.Now;
            await _foodMemberRepository.CreateFoodMemberAsync(foodMemberModel);
            return Ok();

        }

        [HttpPut("Update-food-member")]
        [Authorize]
        public async Task<IActionResult> UpdateFoodMember([FromBody] UpdateFoodMemberRequestDTO updateFoodMemberDto)
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
            bool check = await _foodMemberRepository.UpdateFoodMemberAsync(updateFoodMemberDto, memberId);
            if (check) return NoContent();

            else return BadRequest();
        }



        // DELETE: api/FoodMember/{foodMemberId}
        [HttpDelete("{foodMemberId}")]
        [Authorize]
        public async Task<IActionResult> DeleteFoodMember(int foodMemberId)
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


            bool check = await _foodMemberRepository.DeleteFoodMemberAsync(foodMemberId, memberId);
            if (check) return NoContent();

            else return BadRequest();
        }
    }
}
