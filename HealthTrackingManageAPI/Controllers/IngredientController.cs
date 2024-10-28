using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepo;
using Repository.Repo;

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
    }
}
