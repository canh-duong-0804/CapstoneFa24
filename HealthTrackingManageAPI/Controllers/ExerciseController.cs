using BusinessObject;
using BusinessObject.Dto.Blog;
using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepo;

namespace HealthTrackingManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseRepository _exerciseRepository;
        public ExerciseController(IExerciseRepository exerciseRepository)
        {
            _exerciseRepository = exerciseRepository;
        }
        [HttpGet("GetAllExercises")]
        public async Task<IActionResult> GetAllExercises()
        {
            var exercises = await _exerciseRepository.GetAllExercisesAsync();
            return Ok(exercises);
        }

      
        [HttpGet("GetExerciseById/{id}")]
        public async Task<IActionResult> GetExerciseById(int id)
        {
            var exercise = await _exerciseRepository.GetExerciseByIdAsync(id);
            if (exercise == null)
            {
                return NotFound("Exercise not found.");
            }
            return Ok(exercise);
        }

     
        [HttpPost("CreateExercise")]
        public async Task<IActionResult> CreateExercise([FromBody] CreateExerciseRequestDTO exercise)
        {
            var mapper = MapperConfig.InitializeAutomapper();

            var modelExericse = mapper.Map<BusinessObject.Models.Exercise>(exercise);
            if (exercise == null)
            {
                return BadRequest("Exercise is null.");
            }

            var createdExercise = await _exerciseRepository.CreateExerciseAsync(modelExericse);
            return CreatedAtAction(nameof(GetExerciseById), new { id = createdExercise.ExerciseId }, createdExercise);
        }


        [HttpPut("UpdateExercise/{id}")]
        public async Task<IActionResult> UpdateExercise(int id, [FromBody] Exercise exercise)
        {
            if (exercise == null || exercise.ExerciseId != id)
            {
                return BadRequest("Exercise is null or ID mismatch.");
            }

            var updatedExercise = await _exerciseRepository.UpdateExerciseAsync(exercise);
            if (updatedExercise == null)
            {
                return NotFound("Exercise not found.");
            }

            return Ok(updatedExercise);
        }


        [HttpDelete("DeleteExercise/{id}")]
        public async Task<IActionResult> DeleteExercise(int id)
        {
            var result = await _exerciseRepository.DeleteExerciseAsync(id);
            if (!result)
            {
                return NotFound("Exercise not found.");
            }

            return NoContent();
        }







        [HttpGet("search-and-filter")]
        public async Task<IActionResult> SearchAndFilterExercise([FromQuery] string searchName, [FromQuery] string categoryExerciseName)
        {
            try
            {
                var exercises = await _exerciseRepository.SearchAndFilterExerciseByIdAsync(searchName, categoryExerciseName);
                if (exercises == null)
                {
                    return NotFound("No exercises found matching the criteria.");
                }

                return Ok(exercises);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}