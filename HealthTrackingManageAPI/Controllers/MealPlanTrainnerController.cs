﻿using BusinessObject;
using BusinessObject.Dto.MealPlan;
using BusinessObject.Dto.MealPlanDetail;
using BusinessObject.Models;
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
    public class MealPlanTrainnerController : ControllerBase
    {
        private readonly IMealPlanTrainnerRepository _mealPlanRepository;

        public MealPlanTrainnerController(IMealPlanTrainnerRepository mealPlanRepository)
        {
            _mealPlanRepository = mealPlanRepository;
        }

        [HttpPost("create-meal-plan-by-trainner")]
        [RoleLessThanOrEqualTo(1)]
        public async Task<IActionResult> CreateMealPlanTrainer([FromBody] CreateMealPlanRequestDTO request)
        {


            var memberIdClaim = User.FindFirstValue("Id");
            if (memberIdClaim == null)
            {
                return Unauthorized();
            }
            if (!int.TryParse(memberIdClaim, out int memberId))
            {
                return BadRequest();
            }
            var role = User.FindFirstValue(ClaimTypes.Role);

            var mapper = MapperConfig.InitializeAutomapper();
            var mealPlanModel = mapper.Map<BusinessObject.Models.MealPlan>(request);
            mealPlanModel.CreatedAt = DateTime.UtcNow;
            mealPlanModel.CreateBy = memberId;


            var success = await _mealPlanRepository.CreateMealPlanTrainerAsync(mealPlanModel);

            if (!success)
            {
                return StatusCode(500);
            }

            return Ok();
        } 
        
        
        
        [HttpPut("update-meal-plan-by-trainner")]
        [RoleLessThanOrEqualTo(1)]
        public async Task<IActionResult> UpdateMealPlanTrainer([FromBody] UpdateMealPlanRequestDTO request)
        {


            var memberIdClaim = User.FindFirstValue("Id");
            if (memberIdClaim == null)
            {
                return Unauthorized();
            }
            if (!int.TryParse(memberIdClaim, out int memberId))
            {
                return BadRequest();
            }
            var role = User.FindFirstValue(ClaimTypes.Role);

            var mapper = MapperConfig.InitializeAutomapper();
            var mealPlanModel = mapper.Map<BusinessObject.Models.MealPlan>(request);
            mealPlanModel.ChangeDate = DateTime.UtcNow;
            mealPlanModel.ChangeBy = memberId;


            var success = await _mealPlanRepository.UpdateMealPlanTrainerAsync(mealPlanModel);

            if (!success)
            {
                return StatusCode(500);
            }

            return Ok();
        }

        [HttpDelete("delete-meal-plan-by-trainner")]
        [RoleLessThanOrEqualTo(1)]
        public async Task<IActionResult> DeleteMealPlan(int mealPlanId)
        {


            var memberIdClaim = User.FindFirstValue("Id");
            if (memberIdClaim == null)
            {
                return Unauthorized();
            }
            if (!int.TryParse(memberIdClaim, out int memberId))
            {
                return BadRequest();
            }
            var role = User.FindFirstValue(ClaimTypes.Role);

            

            var success = await _mealPlanRepository.DeleteMealPlanAsync(mealPlanId);

            if (!success)
            {
                return StatusCode(500);
            }

            return Ok();
        }
        
        [HttpGet("get-meal-plan-by-trainner")]
        [RoleLessThanOrEqualTo(1)]
        public async Task<IActionResult> GetMealPlan(int mealPlanId)
        {


            var memberIdClaim = User.FindFirstValue("Id");
            if (memberIdClaim == null)
            {
                return Unauthorized();
            }
            if (!int.TryParse(memberIdClaim, out int memberId))
            {
                return BadRequest();
            }
            var role = User.FindFirstValue(ClaimTypes.Role);

            

            var success = await _mealPlanRepository.GetMealPlanAsync(mealPlanId);

           /* if (!success)
            {
                return StatusCode(500);
            }*/

            return Ok(success);
        }
        [HttpGet("Get-all-meal-plan-for-staff")]
        [RoleLessThanOrEqualTo(1)]
        public async Task<IActionResult> GetAllMealPlanForStaff([FromQuery] int? page)
        {
            try
            {
                // Validate the page parameter
                if (page.HasValue && page < 1)
                {
                    return BadRequest("Page number must be greater than or equal to 1.");
                }

                int currentPage = page ?? 1; // Default to page 1 if not provided
                int pageSize = 5; // Number of items per page

                // Get total number of meal plans for staff
                int totalMealPlans = await _mealPlanRepository.GetTotalMealPlanAsync();

                if (totalMealPlans == 0)
                {
                    return NotFound("No meal plans found.");
                }

                // Calculate total pages
                int totalPages = (int)Math.Ceiling(totalMealPlans / (double)pageSize);

                // Adjust the current page if it exceeds the total pages
                if (currentPage > totalPages && totalPages > 0)
                {
                    currentPage = totalPages;
                }

                // Retrieve meal plans for the current page
                var mealPlans = await _mealPlanRepository.GetAllMealPlanForStaffsAsync(currentPage, pageSize);

                if (mealPlans == null || !mealPlans.Any())
                {
                    return NotFound("No meal plans found.");
                }

                // Return paginated results
                return Ok(new
                {
                    MealPlans = mealPlans,
                    TotalPages = totalPages,
                    CurrentPage = currentPage,
                    PageSize = pageSize,
                    TotalMealPlans = totalMealPlans
                });
            }
            catch (UnauthorizedAccessException)
            {
                // Handle unauthorized access
                return StatusCode(403, "Access denied.");
            }
            catch (KeyNotFoundException)
            {
                // Handle case where data is not found
                return NotFound("No meal plans found.");
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, new { Error = "An internal server error occurred.", Details = ex.Message });
            }
        }




        [HttpPost("create-meal-plan-detail")]
        [RoleLessThanOrEqualTo(1)]
        public async Task<IActionResult> CreateMealPlanDetail([FromBody] CreateMealPlanDetailRequestDTO request)
        {
            
               
                var memberIdClaim = User.FindFirstValue("Id");
                if (memberIdClaim == null)
                {
                    return Unauthorized();
                }
                if (!int.TryParse(memberIdClaim, out int memberId))
                {
                    return BadRequest("Invalid member ID.");
                }
                var success = await _mealPlanRepository.CreateMealPlanDetailAsync(request);

                if (!success)
                {
                    return StatusCode(500, "Failed to create meal plan detail.");
                }

                return Ok(new { Message = "Meal plan detail created successfully." });
           
        }
        [HttpGet("get-meal-plan-detail")]
        [RoleLessThanOrEqualTo(1)]
        public async Task<IActionResult> GetMealPlanDetail(int MealPlanId ,int Day )
        {
            try
            {
                var memberIdClaim = User.FindFirstValue("Id");
                if (memberIdClaim == null)
                {
                    return Unauthorized();
                }

                if (!int.TryParse(memberIdClaim, out int memberId))
                {
                    return BadRequest("Invalid member ID");
                }

               
                var mealPlanDetails = await _mealPlanRepository.GetMealPlanDetailAsync(MealPlanId,Day);

                if (mealPlanDetails == null )
                {
                    return NotFound(new { message = "Meal plan details not found." });
                }

                return Ok(mealPlanDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpPut("update-meal-plan-detail")]
        [RoleLessThanOrEqualTo(1)]
        public async Task<IActionResult> UpdateMealPlanDetail([FromBody] CreateMealPlanDetailRequestDTO request)
        {
            try
            {
                var memberIdClaim = User.FindFirstValue("Id");
                if (memberIdClaim == null)
                {
                    return Unauthorized();
                }

                if (!int.TryParse(memberIdClaim, out int memberId))
                {
                    return BadRequest("Invalid member ID");
                }

               
                var success = await _mealPlanRepository.UpdateMealPlanDetailAsync(request);

                if (!success)
                {
                    return StatusCode(500, new { message = "Failed to update meal plan detail." });
                }

                return Ok(new { message = "Meal plan detail updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}
    