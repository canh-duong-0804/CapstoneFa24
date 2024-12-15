using BusinessObject.Dto.ExerciseTrainer;
using HealthTrackingManageAPI.Authorize;
using HealthTrackingManageAPI.NewFolder.Image;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepo;
using System.Security.Claims;

namespace HealthTrackingManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseTrainerController : ControllerBase
    {
        private readonly CloudinaryService _cloudinaryService;
        private readonly IExerciseTrainerRepository _exerciseRepository;
        public ExerciseTrainerController(IExerciseTrainerRepository exerciseRepository, CloudinaryService cloudinaryService)
        {
            _exerciseRepository = exerciseRepository;
            _cloudinaryService = cloudinaryService;
        }

        [RoleLessThanOrEqualTo(3)]
        [HttpPost("create-exercise")]
        public async Task<IActionResult> CreateExercise([FromBody] CreateExerciseRequestDTO request)
        {
            try
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

                int result = await _exerciseRepository.CreateExerciseTrainerAsync(request, memberId);

                if (result!=0)
                    return Ok(result);

                return BadRequest("Failed to create exercise.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        /*[HttpGet("get-all-exercises-trainer")]
        [RoleLessThanOrEqualTo(1)]
        public async Task<IActionResult> GetAllExercisePlans([FromQuery] int page)
        {
            try
            {
                int pageSize = 5;
                if (page <= 0 || pageSize <= 0)
                {
                    return BadRequest("Page and pageSize must be greater than zero.");
                }

                // Lấy danh sách phân trang từ repository
                var pagedResult = await _exerciseRepository.GetAllExercisePlansAsync(page, pageSize);

                if (pagedResult == null || !pagedResult.Data.Any())
                {
                    return NotFound("No exercise plans found.");
                }

                return Ok(pagedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
*/

        [HttpGet("get-all-exercises-trainer")]
        [RoleLessThanOrEqualTo(3)]
        public async Task<IActionResult> GetAllExercisePlans([FromQuery] int? page, [FromQuery] string? SearchExercise)
        {
            try
            {
                int pageSize = 5; // Default page size

                if (page.HasValue && page <= 0)
                {
                    return BadRequest("Page number must be greater than zero.");
                }

                int currentPage = page ?? 1; // Default to page 1 if not provided

                // Retrieve the total count of exercise plans
                int totalExercisePlans = await _exerciseRepository.GetTotalExercisesAsync(SearchExercise);
                if (totalExercisePlans == 0)
                {
                    return NotFound("No exercise plans found.");
                }

                // Calculate the total number of pages
                int totalPages = (int)Math.Ceiling(totalExercisePlans / (double)pageSize);

                // Adjust the current page if it exceeds total pages
                if (currentPage > totalPages && totalPages > 0)
                {
                    currentPage = totalPages;
                }

                // Fetch paginated data
                var exercisePlans = await _exerciseRepository.GetAllExerciseAsync(currentPage, pageSize,SearchExercise);

                if (exercisePlans == null )
                {
                    return NotFound("No exercise plans found.");
                }

                // Prepare the response
                /*return Ok(new
                {
                    ExercisePlans = exercisePlans,
                    TotalPages = totalPages,
                    CurrentPage = currentPage,
                    PageSize = pageSize,
                    TotalExercisePlans = totalExercisePlans
                });*/
                return Ok(exercisePlans);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An internal server error occurred: {ex.Message}");
            }
        }


        [RoleLessThanOrEqualTo(3)]
        [HttpPut("upload-image-exercise")]
        public async Task<IActionResult> UploadImageMealPlan(IFormFile? imageFile, [FromForm] int exerciseId)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadResult = await _cloudinaryService.UploadImageAsync(
                    imageFile,
                    "user_uploads"
                );
                var check = await _exerciseRepository.UploadImageForMealMember(uploadResult.Url.ToString(), exerciseId);


                if (check) return Ok();

            }
            return BadRequest();
        }

        [HttpDelete("delete-exercise/{exerciseId}")]
        [RoleLessThanOrEqualTo(2)]
        public async Task<IActionResult> DeleteExercise(int exerciseId)
        {
            try
            {
                var result = await _exerciseRepository.DeleteExerciseAsync(exerciseId);

                if (result)
                    return Ok();

                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-exercise-detail/{exerciseId}")]
        public async Task<IActionResult> GetExerciseDetail(int exerciseId)
        {
            try
            {
                var exerciseDetail = await _exerciseRepository.GetExerciseDetailAsync(exerciseId);

                if (exerciseDetail == null)
                    return NotFound(new { Message = "Exercise not found." });

                return Ok(exerciseDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("update-exercise/{exerciseId}")]
        [RoleLessThanOrEqualTo(2)]
        public async Task<IActionResult> UpdateExercise( [FromBody] ExerciseRequestDTO updateRequest)
        {
            try
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

                var result = await _exerciseRepository.UpdateExerciseAsync(memberId, updateRequest);

                
                    //return NotFound(new { Message = "Exercise not found or update failed." });

                return Ok(new { Message = "Exercise updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }



    }
}
