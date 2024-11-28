﻿using BusinessObject.Dto.ExerciseTrainer;
using HealthTrackingManageAPI.Authorize;
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
        private readonly IExerciseTrainerRepository _exerciseRepository;
        public ExerciseTrainerController(IExerciseTrainerRepository exerciseRepository)
        {
            _exerciseRepository = exerciseRepository;
        }

        [RoleLessThanOrEqualTo(1)]
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

        [HttpDelete("delete-exercise/{exerciseId}")]
        [Authorize]
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

                
                    return NotFound(new { Message = "Exercise not found or update failed." });

                return Ok(new { Message = "Exercise updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }



    }
}