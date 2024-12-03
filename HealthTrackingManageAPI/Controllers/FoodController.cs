using BusinessObject;
using BusinessObject.Dto.Food;
using BusinessObject.Models;
using HealthTrackingManageAPI.Authorize;
using HealthTrackingManageAPI.NewFolder.Image;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Repository.IRepo;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace HealthTrackingManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly CloudinaryService _cloudinaryService;
        private readonly IFoodRepository _foodRepository;
        public FoodController(IFoodRepository foodRepository, CloudinaryService cloudinaryService)
        {
            _foodRepository = foodRepository;
            _cloudinaryService = cloudinaryService;
        }

        /*[HttpGet("get-all-foods-for-staff")]
        [RoleLessThanOrEqualTo(2)]
        public async Task<IActionResult> GetAllFoodsForStaff()
        {
            var foods = await _foodRepository.GetAllFoodsForStaffAsync();


            if (foods == null || !foods.Any())
            {
                return NotFound("No foods found.");
            }


            return Ok(foods);
        }*/

        [HttpGet("get-all-foods-for-staff")]
        [RoleLessThanOrEqualTo(2)]
        public async Task<IActionResult> GetAllFoodsForStaff([FromQuery] int? page)
        {
            try
            {
                // Validate the page parameter
                if (page.HasValue && page < 1)
                {
                    return BadRequest("Page number must be greater than or equal to 1.");
                }

                int currentPage = page ?? 1; // Default to page 1 if not provided
                int pageSize = 5; // Number of items per page

                // Get total number of foods
                int totalFoods = await _foodRepository.GetTotalFoodsForStaffAsync();

                if (totalFoods == 0)
                {
                    return NotFound("No foods found.");
                }

                // Calculate total pages
                int totalPages = (int)Math.Ceiling(totalFoods / (double)pageSize);

                // Adjust the current page if it exceeds the total pages
                if (currentPage > totalPages && totalPages > 0)
                {
                    currentPage = totalPages;
                }

                // Retrieve foods for the current page
                var foods = await _foodRepository.GetAllFoodsForStaffAsync(currentPage, pageSize);

                if (foods == null || !foods.Any())
                {
                    return NotFound("No foods found.");
                }

                // Return paginated results
                return Ok(new
                {
                    Foods = foods,
                    TotalPages = totalPages,
                    CurrentPage = currentPage,
                    PageSize = pageSize,
                    TotalFoods = totalFoods
                });
            }
            catch (UnauthorizedAccessException)
            {
                // Handle unauthorized access
                return StatusCode(403, "Access denied.");
            }
            catch (KeyNotFoundException)
            {
                // Handle case where data is not found
                return NotFound("No foods found.");
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, new { Error = "An internal server error occurred.", Details = ex.Message });
            }
        }



        [HttpGet("get-all-foods-for-member")]
       
        public async Task<IActionResult> GetAllFoodsForMember()
        {
            var foods = await _foodRepository.GetAllFoodsForMemberAsync();


            if (foods == null || !foods.Any())
            {
                return NotFound("No foods found.");
            }


            return Ok(foods);
        }
        
        
        [HttpGet("search-foods-for-member")]
        public async Task<IActionResult> SearchFoodsForMember(string foodName)
        {
            var foods = await _foodRepository.SearchFoodsForMemberAsync(foodName);

            return Ok(foods);
        }
        
        
        
        [HttpGet("get-list-box-food-for-staff")]
        public async Task<IActionResult> GetListBoxFoodForStaff()
        {
            var listBoxFood = await _foodRepository.GetListBoxFoodForStaffAsync();


            if (listBoxFood == null || !listBoxFood.Any())
            {
                return NotFound("No listBoxFood found.");
            }


            return Ok(listBoxFood);
        }

        [HttpPost("create-food")]
        [RoleLessThanOrEqualTo(2)]
        public async Task<IActionResult> CreateFood([FromBody] CreateFoodRequestDTO food)
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
            if (food == null)
            {
                return BadRequest("Food object is null.");
            }
            var mapper = MapperConfig.InitializeAutomapper();
            var foodModel = mapper.Map<BusinessObject.Models.Food>(food);
            foodModel.CreateBy = memberId;
            if (!food.Serving.IsNullOrEmpty())
            {
                foodModel.Portion = food.Portion + " (" + food.Serving + ")";
            }
            else
            {
                foodModel.Portion = food.Portion;
            }


            var createdFood = await _foodRepository.CreateFoodAsync(foodModel);
            // return CreatedAtAction(nameof(GetAllFoodsForStaff), new { id = createdFood.FoodId }, createdFood);
            return Ok(createdFood.FoodId);
        }

        [Authorize]
        [HttpPut("upload-image-meal-plan")]
        public async Task<IActionResult> UploadImageMealPlan(IFormFile? imageFile, [FromForm] int FoodId)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadResult = await _cloudinaryService.UploadImageAsync(
                    imageFile,
                    "user_uploads"
                );
                var check = await _foodRepository.UploadImageFood(uploadResult.Url.ToString(), FoodId);


                if (check) return Ok();

            }
            return BadRequest();
        }


        [HttpPut("update-food")]
        [RoleLessThanOrEqualTo(2)]
        public async Task<IActionResult> UpdateFoodStatus([FromBody] UpdateFoodRequestDTO food)
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
            var mapper = MapperConfig.InitializeAutomapper();
            var foodModel = mapper.Map<BusinessObject.Models.Food>(food);
            foodModel.ChangeDate = DateTime.Now;
            foodModel.ChangeBy = memberId;
            foodModel.CreateBy = memberId;
            if (!food.Serving.IsNullOrEmpty())
            {
                foodModel.Portion = food.Portion + " (" + food.Serving + ")";
            }
            else
            {
                foodModel.Portion = food.Portion;
            }


            await _foodRepository.UpdateFoodAsync(foodModel);

            return NoContent();
        }

        
        [HttpDelete("delete-food/{id}")]
        public async Task<IActionResult> DeleteFood(int id)
        {

            
            bool deleteSuccess = await _foodRepository.DeleteFoodAsync(id);

          
            if (deleteSuccess)
            {
                return NoContent(); 
            }
            else
            {
                return NotFound("Food item not found or already deleted."); 
            }
        }
        [HttpGet("get-food-for-staff-by-id/{id}")]
        [RoleLessThanOrEqualTo(2)]
        public async Task<IActionResult> GetFoodById(int id)
        {
            var food = await _foodRepository.GetFoodForStaffByIdAsync(id);

            if (food == null)
            {
                return NotFound("Food not found.");
            }

            return Ok(food);
        }


        /* [HttpGet("get-food-for-member-by-id/{id}")]
         public async Task<IActionResult> GetFoodForMemberById(int id)
         {
             var food = await _foodRepository.GetFoodForMemberByIdAsync(id);

             if (food == null)
             {
                 return NotFound("Food not found.");
             }

             return Ok(food);
         }*/

        [HttpGet("get-food-for-member-by-id")]
        [Authorize]
        public async Task<IActionResult> GetFoodForMemberById(int FoodId, DateTime SelectDate)
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
           

            var food = await _foodRepository.GetFoodForMemberByIdAsync(FoodId, SelectDate, memberId);

            if (food == null)
            {
                return NotFound("Food not found.");
            }

            return Ok(food);
        }
    }
}
   