using BusinessObject;
using BusinessObject.Dto.Blog;
using BusinessObject.Dto.Exericse;
using BusinessObject.Dto.SearchFilter;
using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepo;
using System.Security.Claims;

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

        [HttpGet("Get-all-exercises-for-member")]
        [Authorize]
        public async Task<IActionResult> GetAllExercises()
        {
           /* var memberIdClaim = User.FindFirstValue("Id");
            if (memberIdClaim == null)
            {
                return Unauthorized("Member ID not found in claims.");
            }

            if (!int.TryParse(memberIdClaim, out int memberId))
            {
                return BadRequest("Invalid member ID.");
            }*/
            var exercises = await _exerciseRepository.GetAllExercisesForMemberAsync();
            return Ok(exercises);
        }









        /*[HttpGet("Get-all-exercises")]
        public async Task<IActionResult> GetAllExercises()
        {
            var exercises = await _exerciseRepository.GetAllExercisesAsync();
            return Ok(exercises);
        }



        [HttpGet("Get-exercise-by-id/{id}")]
        public async Task<IActionResult> GetExerciseById(int id)
        {
            var exercise = await _exerciseRepository.GetExerciseByIdAsync(id);
            if (exercise == null)
            {
                return NotFound("Exercise not found.");
            }
            return Ok(exercise);
        }


        [HttpPost("Create-exercise")]
        public async Task<IActionResult> CreateExercise([FromBody] CreateExerciseRequestDTO exercise)
        {
            var mapper = MapperConfig.InitializeAutomapper();

            var modelExericse = mapper.Map<BusinessObject.Models.Exercise>(exercise);
            if (exercise == null)
            {
                return BadRequest("Exercise is null.");
            }

            var createdExercise = await _exerciseRepository.CreateExerciseAsync(modelExericse);
            // return CreatedAtAction(nameof(GetExerciseById), new { id = createdExercise.ExerciseId }, createdExercise);
            return Ok();
        }


        [HttpPut("Update-exercise")]
        public async Task<IActionResult> UpdateExercise([FromBody] UpdateExerciseRequestDTO exercise)
        {


            var updatedExercise = await _exerciseRepository.UpdateExerciseAsync(exercise);
            if (updatedExercise == null)
            {
                return NotFound("Exercise not found.");
            }

            return Ok(updatedExercise);
        }


        [HttpDelete("Delete-exercise/{id}")]
        public async Task<IActionResult> DeleteExercise(int id)
        {
            var result = await _exerciseRepository.DeleteExerciseAsync(id);
            if (!result)
            {
                return NotFound("Exercise not found.");
            }

            return NoContent();
        }

        [HttpPost("search-and-filter")]
        public async Task<IActionResult> SearchAndFilterExercise([FromBody] SearchFilterObjectDTO search)
        {
            try
            {
                var exercises = await _exerciseRepository.SearchAndFilterExerciseByIdAsync(search);
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
        }*/


    }
}