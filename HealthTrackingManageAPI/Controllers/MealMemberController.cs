using BusinessObject;
using BusinessObject.Dto.CopyMeal;
using BusinessObject.Dto.Food;
using BusinessObject.Dto.MealDetailMember;
using BusinessObject.Dto.MealMember;
using BusinessObject.Dto.Member;
using BusinessObject.Models;
using DataAccess;
using HealthTrackingManageAPI.NewFolder.Image;
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
    public class MealMemberController : ControllerBase
    {
        private readonly CloudinaryService _cloudinaryService;
        private readonly IMealMemberRepository _mealPlanMemberRepository;
        public MealMemberController(IMealMemberRepository mealPlanMemberRepository, CloudinaryService cloudinaryService)
        {
            _mealPlanMemberRepository = mealPlanMemberRepository;
            _cloudinaryService = cloudinaryService;
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
        [HttpPut("upload-image-meal-plan")]
        public async Task<IActionResult> UploadImageMealPlan(IFormFile? imageFile, [FromForm] int mealMemberid)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadResult = await _cloudinaryService.UploadImageAsync(
                    imageFile,
                    "user_uploads"
                );
              var check =  await _mealPlanMemberRepository.UploadImageForMealMember(uploadResult.Url.ToString(), mealMemberid);
               
             
               if(check) return Ok();

            }
            return BadRequest();
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
            //mealMember.NameMealMember = "hii";

            var mealMemberId = await _mealPlanMemberRepository.CreateMealMemberAsync(mealMember);

            var mealMemberDetails = mapper.Map<List<MealMemberDetail>>(mealMemberDto.MealDetails);
            foreach (var detail in mealMemberDetails)
            {
                detail.MemberId = memberId;
                detail.MealMemberId = mealMemberId;
            }

            await _mealPlanMemberRepository.CreateMealMemberDetailsAsync(mealMemberDetails);


            await _mealPlanMemberRepository.UpdateMealMemberTotalCaloriesAsync(mealMemberId);

            return Ok(mealMemberId);
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
        [HttpPost("Add-meal-member-to-diary")]
        public async Task<IActionResult> AddMealMemberToDiary([FromBody] AddMealMemberToFoodDiaryDetailRequestDTO addMealMemberTOFoodDiary)
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

            var success = await _mealPlanMemberRepository.AddMealMemberToDiaryDetailAsync(addMealMemberTOFoodDiary, memberId);

            if (!success)
            {
                return StatusCode(500);
            }

            return Ok("Meal successfully added to the diary.");

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
        [Authorize]
        [HttpGet("Copy-previous-meal")]
        public async Task<IActionResult> CopyPreviousMeal(int dirayId, int Mealtype)
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
            var getMealCopy = await _mealPlanMemberRepository.CopyPreviousMeal(dirayId, Mealtype);

            if (getMealCopy == null) return NotFound();

            return Ok(getMealCopy);
        }
        
        [Authorize]
        [HttpGet("get-meal-before-by-meal-type")]
        public async Task<IActionResult> GetMealBeforeByMealType(int MealtypePrevious)
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
            var getMealCopy = await _mealPlanMemberRepository.GetMealBeforeByMealType(memberId, MealtypePrevious);

            if (getMealCopy == null) return NotFound();

            return Ok(getMealCopy);
        }




        [Authorize]
        [HttpPost("Insert-copy-previous-meal")]
        public async Task<IActionResult> InsertCopyPreviousMeal([FromBody]InsertCopyMealDTO request)
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
            var getMealCopy = await _mealPlanMemberRepository.InsertCopyPreviousMeal(request,memberId);

            if (getMealCopy == null) return BadRequest();

            return Ok();
        }
    }
}