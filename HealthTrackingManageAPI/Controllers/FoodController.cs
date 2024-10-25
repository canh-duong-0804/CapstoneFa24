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

        [HttpGet("get-all-food-for-staff")]
        public async Task<IActionResult> GetAllFoodsForStaff()
        {
            var foods = await _foodRepository.GetAllFoodsAsync();


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
        public async Task<IActionResult> UpdateFoodStatus([FromBody] CreateFoodRequestDTO food,int id)
        {
            var mapper = MapperConfig.InitializeAutomapper();
            var foodModel = mapper.Map<BusinessObject.Models.Food>(food);
            await _foodRepository.UpdateFoodAsync(foodModel);

            return NoContent();
        }

        
        [HttpDelete("delete-food/{id}")]
        public async Task<IActionResult> DeleteFood(int id)
        {
            var food = await _foodRepository.GetFoodByIdAsync(id);
            if (food == null)
            {
                return NotFound("Food not found.");
            }

            food.Status = false; 
            await _foodRepository.DeleteFoodAsync(id);

            return NoContent();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFoodById(int id)
        {
            var food = await _foodRepository.GetFoodByIdAsync(id);

            if (food == null)
            {
                return NotFound("Food not found.");
            }

            return Ok(food);
        }
    }
}
   