using AutoMapper.Execution;
using BusinessObject;
using BusinessObject.Dto.FoodDiary;
using BusinessObject.Dto.FoodDiaryDetails;
using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepo;

namespace HealthTrackingManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodDiaryController : ControllerBase
    {

        private readonly IFoodDiaryRepository _foodDiaryRepository;

        public FoodDiaryController(IFoodDiaryRepository foodDiaryService)
        {
            _foodDiaryRepository = foodDiaryService;
        }


/*
        [HttpGet("getOrCreateDailyFoodDiary")]
        public async Task<IActionResult> GetOrCreateDailyFoodDiary(int memberId)
        {
            DateTime date = DateTime.Now.Date;
            var foodDiary = await _foodDiaryRepository.GetOrCreateFoodDiaryByDate(memberId, date);

            var mapper = MapperConfig.InitializeAutomapper();
            var foodDiaryModel = mapper.Map<FoodDiaryResponseDTO>(foodDiary);
            return Ok(foodDiaryModel);
        }*/

       /* [HttpPut("updateDailyFoodDiary")]
        public async Task<IActionResult> UpdateDailyFoodDiary([FromBody] UpdateFoodDiaryRequestDTO updatedFoodDiary)
        {
            DateTime date = DateTime.Now.Date;

            var result = await _foodDiaryRepository.UpdateFoodDiary(updatedFoodDiary);
            if (result!=null)
            {
                return Ok();
            }
            return NotFound();
        }*/

        [HttpPost("addFoodListToDiary")]
        public async Task<IActionResult> AddFoodListToDiary([FromBody] FoodDiaryDetailRequestDTO request)
        {
            if (request == null )
            {
                return BadRequest("Invalid request: No food details provided.");
            }

            var result = await _foodDiaryRepository.AddFoodListToDiaryAsync(request);

            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("deleteFoodListFromDiary/{id}")]
        public async Task<IActionResult> DeleteFoodListFromDiary(int id)
        {
            
          

            
            var result = await _foodDiaryRepository.DeleteFoodListToDiaryAsync(id);

            if (!result)
            {
                return NotFound($"Food item with ID {id} not found.");
            }

            return NoContent();
        }



        /*   [HttpGet("getDailyFoodDiaryFollowMeal")]
           public async Task<IActionResult> getDailyFoodDiaryFollowMeal(int dairyID,int mealType)
           {
              *//* DateTime date = DateTime.Now.Date;
               var foodDiary = await _foodDiaryRepository.getDailyFoodDiaryFollowMeal(dairyID,mealType);*/

        /* var mapper = MapperConfig.InitializeAutomapper();
         var foodDiaryModel = mapper.Map<FoodDiaryResponseDTO>(foodDiary);*//*
         return Ok();
     }*/
    } 
}
