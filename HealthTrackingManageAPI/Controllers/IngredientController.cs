using BusinessObject.Dto.Food;
using BusinessObject;
using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepo;
using Repository.Repo;
using BusinessObject.Dto.Ingredient;

namespace HealthTrackingManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientRepository _ingredientRepository;
        public IngredientController(IIngredientRepository ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;
        }

        [HttpGet("get-all-ingredient")]
        public async Task<IActionResult> GetAllIngredient()
        {
            var ingredients = await _ingredientRepository.GetAllingredientsAsync();


            if (ingredients == null || !ingredients.Any())
            {
                return NotFound("No ingredients found.");
            }


            return Ok(ingredients);


        }


        [HttpGet("get-list-box-ingredient-for-staff")]
        public async Task<IActionResult> GetListBoxIngredientForStaff()
        {
            var listBoxIngredient = await _ingredientRepository.GetListBoxIngredientForStaffAsync();


            if (listBoxIngredient == null || !listBoxIngredient.Any())
            {
                return NotFound("No listBoxFood found.");
            }


            return Ok(listBoxIngredient);
        }
        //insert
        [HttpPost("create-ingredient")]
        public async Task<IActionResult> CreateFood([FromBody] CreateIngredientRequestDTO ingredient)
        {
            if (ingredient == null)
            {
                return BadRequest("Ingredient object is null.");
            }
            var mapper = MapperConfig.InitializeAutomapper();
            var ingredientModel = mapper.Map<BusinessObject.Models.Ingredient>(ingredient);


            var createdFood = await _ingredientRepository.CreateIngredientModelAsync(ingredientModel);
            return CreatedAtAction(nameof(GetAllIngredient), new { id = createdFood.IngredientId }, createdFood);
            //return Ok(createdFood); 
        }

        //put
        [HttpPut("update-ingredient-status")]
        public async Task<IActionResult> UpdateFoodStatus([FromBody] UpdateIngredientRequestDTO ingredient)
        {
            var mapper = MapperConfig.InitializeAutomapper();
            var ingredientModel = mapper.Map<BusinessObject.Models.Ingredient>(ingredient);
            var responseIngredient = await _ingredientRepository.UpdateIngredientAsync(ingredientModel);
            if (responseIngredient == null) return NotFound();
            return NoContent();
        }


        //delete


    }
}
