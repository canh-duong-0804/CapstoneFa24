﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepo;

namespace HealthTrackingManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DietController : ControllerBase
    {
        private readonly IFoodRepository _dietRepository;
        public DietController(IFoodRepository dietdRepository)
        {
            _dietRepository = dietdRepository;
        }



        [HttpGet("Get-all-diet")]
        public async Task<IActionResult> GetAllExercises()
        {
            var exercises = await _dietRepository.GetAllDietAsync();
            return Ok(exercises);
        }
    }
}