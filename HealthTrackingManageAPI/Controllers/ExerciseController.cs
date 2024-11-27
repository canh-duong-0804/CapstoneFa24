﻿using BusinessObject;
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
        public async Task<IActionResult> GetAllExercises([FromQuery] string? search, [FromQuery] bool? isCardioFilter)
        {
            var exercises = await _exerciseRepository.GetAllExercisesForMemberAsync(search, isCardioFilter);
            return Ok(exercises);
        }


        [HttpGet("Get-exercise-cardio-detail-for-member/{ExerciseId}")]
        [Authorize]
        public async Task<IActionResult> GetExercisesDetailForMember(int ExerciseId)
        {
            
            var exercises = await _exerciseRepository.GetExercisesCardioDetailForMemberrAsync(ExerciseId);

            
            if (exercises == null)
            {
                return NotFound("No exercises found for the member.");
            }

           
            return Ok(exercises);
        }


        [HttpGet("Get-exercise-resistance-detail-for-member/{ExerciseId}")]
        [Authorize]
        public async Task<IActionResult> GetExercisesResistanceDetailForMember(int ExerciseId)
        {

            var exercises = await _exerciseRepository.GetExercisesResistanceDetailForMemberAsync(ExerciseId);


            if (exercises == null)
            {
                return NotFound("No exercises found for the member.");
            }


            return Ok(exercises);
        }
    }
}