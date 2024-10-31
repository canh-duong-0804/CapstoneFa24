using BusinessObject;
using BusinessObject.Dto.CategoryExerice;
using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepo;

namespace HealthTrackingManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryExerciseController : ControllerBase
    {
        private readonly IExerciseCategoryRepository _categoryExerciseRepository;
        public CategoryExerciseController(IExerciseCategoryRepository categoryExerciseRepository)
        {
            _categoryExerciseRepository = categoryExerciseRepository;
        }



        [HttpGet("Get-all-category-exercise")]
        public async Task<IActionResult> GetAllCategoryExercises()
        {
            var categoryExercises = await _categoryExerciseRepository.GetAllCategoryExercisesAsync();
            return Ok(categoryExercises);
        }
        
        
        [HttpPost("Create-category-exercise")]
        public async Task<IActionResult> CreateCategoryExercises([FromBody] CreateCategoryExerciseRequestDTO cate)
        {
            var mapper = MapperConfig.InitializeAutomapper();
            var cateExerciseModel = mapper.Map<BusinessObject.Models.ExerciseCategory>(cate);
            await _categoryExerciseRepository.CreateExerciseCategoryAsync(cateExerciseModel);
            return Ok();
        }
        
        
        [HttpPut("Update-category-exercise")]
        public async Task<IActionResult> UpdateCategoryExercises([FromBody] UpdateCategoryExerciseRequestDTO cate)
        {
           /* var mapper = MapperConfig.InitializeAutomapper();
            var cateExerciseModel = mapper.Map<BusinessObject.Models.ExerciseCategory>(cate);*/
            await _categoryExerciseRepository.UpdateCategoryExercisesAsync(cate);
            return Ok();
        } 
        
        
        [HttpPut("Delete-category-exercise/{id}")]
        public async Task<IActionResult> DeleteCategoryExercises(int id)
        {
           
            await _categoryExerciseRepository.DeleteExerciseCategoryAsync(id);
            return Ok();
        }
    }
}
