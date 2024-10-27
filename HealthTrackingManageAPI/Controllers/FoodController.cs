using BusinessObject;
using BusinessObject.Dto.Food;
using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepo;
using System.Reflection.Metadata;

namespace HealthTrackingManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IFoodRepository _foodRepository;
        public FoodController(IFoodRepository foodRepository)
        {
            _foodRepository = foodRepository;
        }

        [HttpGet("get-all-foods-for-staff")]
        public async Task<IActionResult> GetAllFoodsForStaff()
        {
            var foods = await _foodRepository.GetAllFoodsForStaffAsync();


            if (foods == null || !foods.Any())
            {
                return NotFound("No foods found.");
            }


            return Ok(foods);
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

        [HttpPost("create-food")]
        public async Task<IActionResult> CreateFood([FromBody] CreateFoodRequestDTO food)
        {
            if (food == null)
            {
                return BadRequest("Food object is null.");
            }
            var mapper = MapperConfig.InitializeAutomapper();
            var foodModel = mapper.Map<BusinessObject.Models.Food>(food);


            var createdFood = await _foodRepository.CreateFoodAsync(foodModel);
            return CreatedAtAction(nameof(GetAllFoodsForStaff), new { id = createdFood.FoodId }, createdFood);
        }

       
        [HttpPut("update-food-status/{id}")]
        public async Task<IActionResult> UpdateFoodStatus([FromBody] CreateFoodRequestDTO food)
        {
            var mapper = MapperConfig.InitializeAutomapper();
            var foodModel = mapper.Map<BusinessObject.Models.Food>(food);
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
        public async Task<IActionResult> GetFoodById(int id)
        {
            var food = await _foodRepository.GetFoodForStaffByIdAsync(id);

            if (food == null)
            {
                return NotFound("Food not found.");
            }

            return Ok(food);
        } 
        
        
        [HttpGet("get-food-for-member-by-id/{id}")]
        public async Task<IActionResult> GetFoodForMemberById(int id)
        {
            var food = await _foodRepository.GetFoodForMemberByIdAsync(id);

            if (food == null)
            {
                return NotFound("Food not found.");
            }

            return Ok(food);
        }
    }
}
   