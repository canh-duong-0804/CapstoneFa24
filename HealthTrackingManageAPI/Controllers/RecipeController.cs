using BusinessObject.Dto.Food;
using BusinessObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepo;
using Repository.Repo;
using BusinessObject.Models;
using Twilio.TwiML.Fax;
using BusinessObject.Dto.Recipe.CreateDTO;
using DataAccess;
using BusinessObject.Dto.Recipe;
using BusinessObject.Dto.Recipe.UpdateDTO;

namespace HealthTrackingManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeRepository _recipeRepository;
        public RecipeController(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        [HttpGet("get-all-recipe-for-staff")]
        public async Task<IActionResult> GetAllFoodsForStaff()
        {
            var recipes = await _recipeRepository.GetAllRecipesForStaffAsync();


            if (recipes == null || !recipes.Any())
            {
                return NotFound("No foods found.");
            }


            return Ok(recipes);
        }
        
        
        [HttpGet("get-all-recipes-for-member")]
        public async Task<IActionResult> GetAllFoodsForMember()
        {
            var recipes = await _recipeRepository.GetAllRecipesForMemberAsync();


            if (recipes == null || !recipes.Any())
            {
                return NotFound("No recipes found.");
            }


            return Ok(recipes);
        }
        
        
        [HttpGet("get-recipe-for-staff-by-id/{id}")]
        public async Task<IActionResult> GetRecipeForStaffById(int id)
        {
            var recipeDetails = await _recipeRepository.GetRecipeForStaffByIdAsync(id);

            if (recipeDetails == null)
            {
                return NotFound($"Recipe with ID {id} not found.");
            }

            return Ok(recipeDetails);

        }



        [HttpPost("create-recipe")]
        public async Task<IActionResult> CreateFood([FromBody] CreateRecipeRequestDTO recipe)
        {
            if (recipe == null)
            {
                return BadRequest("recipe object is null.");
            }
            var mapper = MapperConfig.InitializeAutomapper();
            var recipeModel = mapper.Map<BusinessObject.Models.Recipe>(recipe);


            var createdRecipe = await _recipeRepository.CreateRecipeAsync(recipeModel);
            //return CreatedAtAction(nameof(GetAllFoodsForStaff), new { id = createdFood.FoodId }, createdFood);
            //var recipeDTO = mapper.Map<RecipeDTO>(createdRecipe);
            var recipeDTO = mapper.Map<CreateRecipeRequestDTO>(createdRecipe);
            return Ok(recipeDTO);
        }




        [HttpDelete("delete-recipe/{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            var existingRecipe = await _recipeRepository.DeleteRecipeAsync(id);
            if (existingRecipe == null)
            {
                return NotFound();
            }

            //await _recipeRepository.DeleteRecipeAsync(id);
            return NoContent();
        }
        [HttpPut("update-recipe")]
        public async Task<IActionResult> UpdateRecipe([FromBody] UpdateRecipeRequestDTO updateRecipe)
        {
            var existingRecipe = await _recipeRepository.UpdateRecipeAsync(updateRecipe);
            if (existingRecipe == null)
            {
                return NotFound();
            }

            //await _recipeRepository.DeleteRecipeAsync(id);
            return NoContent();


        }




        }
}
