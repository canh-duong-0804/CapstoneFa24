using BusinessObject;
using BusinessObject.Dto.Food;
using BusinessObject.Dto.MealDetailMember;
using BusinessObject.Dto.MealMember;
using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepo;
using System.Security.Claims;

namespace HealthTrackingManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealMemberController : ControllerBase
    {

        private readonly IMealMemberRepository _mealPlanMemberRepository;
        public MealMemberController(IMealMemberRepository mealPlanMemberRepository)
        {
            _mealPlanMemberRepository = mealPlanMemberRepository;
        }



        [Authorize]
        [HttpGet("get-all-meal-members")]
        public async Task<IActionResult> GetAllMealMembers()
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

          
            var mealMembers = await _mealPlanMemberRepository.GetAllMealMembersAsync(memberId);
            var mapper = MapperConfig.InitializeAutomapper();

           
            var mealMemberDtos = mapper.Map<List<GetAllMealMemberResonseDTO>>(mealMembers);

            if (mealMemberDtos == null || !mealMemberDtos.Any())
            {
                return NotFound();
            }

            return Ok(mealMemberDtos);
        }





        [Authorize]
        [HttpPost("create-meal-plan-for-member")]
        public async Task<IActionResult> CreateMealPlanForMember([FromBody] CreateMealMemberRequestDTO mealMemberDto)
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

            if (mealMemberDto == null)
            {
                return BadRequest();
            }

            var mapper = MapperConfig.InitializeAutomapper();

            var mealMember = mapper.Map<MealMember>(mealMemberDto);
            mealMember.MemberId = memberId;
            mealMember.MealDate = DateTime.Now;
            mealMember.NameMealMember = "hii";

            var mealMemberId = await _mealPlanMemberRepository.CreateMealMemberAsync(mealMember);

            var mealMemberDetails = mapper.Map<List<MealMemberDetail>>(mealMemberDto.MealDetails);
            foreach (var detail in mealMemberDetails)
            {
                detail.MemberId = memberId;
                detail.MealMemberId = mealMemberId;
            }

            await _mealPlanMemberRepository.CreateMealMemberDetailsAsync(mealMemberDetails);

            
            await _mealPlanMemberRepository.UpdateMealMemberTotalCaloriesAsync(mealMemberId);

            return Ok() ;
        }

        [Authorize]
        [HttpGet("get-meal-member-detail/{mealMemberId}")]
        public async Task<IActionResult> GetMealMemberDetail(int mealMemberId)
        {
            var mealMember = await _mealPlanMemberRepository.GetMealMemberDetailAsync(mealMemberId);

            if (mealMember == null)
            {
                return NotFound();
            }




            return Ok(mealMember);
        }




        [Authorize]
        [HttpDelete("delete-meal-member-detail/{detailId}")]
        public async Task<IActionResult> DeleteMealMemberDetail(int detailId)
        {
           

            await _mealPlanMemberRepository.DeleteMealMemberDetailAsync(detailId);
            return Ok();
        }


        [Authorize]
        [HttpPost("add-multiple-foods-to-meal/{mealMemberId}")]
        public async Task<IActionResult> AddMultipleFoodsToMealMember(int mealMemberId, [FromBody] AddMoreFoodToMealMemberRequestDTO mealDetails)
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
            if (mealDetails == null || mealDetails.MealDetails.Count == 0)
            {
                return BadRequest();
            }



            var mapper = MapperConfig.InitializeAutomapper();
            var mealMemberDetails = mapper.Map<List<MealMemberDetail>>(mealDetails.MealDetails);
            foreach (var detail in mealMemberDetails)
            {
                detail.MemberId = memberId;
                detail.MealMemberId = mealMemberId;

            }

            await _mealPlanMemberRepository.CreateMealMemberDetailsAsync(mealMemberDetails);


            await _mealPlanMemberRepository.UpdateMealMemberTotalCaloriesAsync(mealMemberId);

            return Ok();
        }

        [Authorize]
        [HttpDelete("delete-meal-member/{mealMemberId}")]
        public async Task<IActionResult> DeleteMealMember(int mealMemberId)
        {

            var memberIdClaim = User.FindFirstValue("Id");
            if (memberIdClaim == null || !int.TryParse(memberIdClaim, out int memberId))
            {
                return Unauthorized();
            }



            var mealMember = await _mealPlanMemberRepository.GetMealMemberDetailAsync(mealMemberId);
            if (mealMember == null)
            {
                return NotFound();
            }


            await _mealPlanMemberRepository.DeleteMealMemberDetailsByMealMemberIdAsync(mealMemberId);


            await _mealPlanMemberRepository.DeleteMealMemberAsync(mealMemberId);

            return Ok();

        }
    }
}