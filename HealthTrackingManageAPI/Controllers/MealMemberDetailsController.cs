using BusinessObject.Dto.Food;
using BusinessObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepo;
using Repository.Repo;
using BusinessObject.Dto.MealPlanDetailMember;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace HealthTrackingManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealMemberDetailsController : ControllerBase
    {
        private readonly IMealMemberDetailsRepository _mealPlanMemberRepository;
        public MealMemberDetailsController(IMealMemberDetailsRepository mealPlanMemberRepository)
        {
            _mealPlanMemberRepository = mealPlanMemberRepository;
        }
       /* [Authorize]
        [HttpPost("Create-meal-plan-details-of-member")]
        public async Task<IActionResult> CreateFood([FromBody] CreateMealDetailMemberRequestDTO mealMember,DateTime date)
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
            if (mealMember == null)
            {
                return BadRequest("Food object is null.");
            }
            var mapper = MapperConfig.InitializeAutomapper();
            var mealMemberModel = mapper.Map<BusinessObject.Models.MealsMemberDetail>(mealMember);


            await _mealPlanMemberRepository.CreateMealPlanDetailsOfMemberAsync(mealMemberModel,memberId,date);
            // return CreatedAtAction(nameof(GetAllFoodsForStaff), new { id = createdFood.FoodId }, createdFood);
            return Ok();
        }
*/
    }
}
