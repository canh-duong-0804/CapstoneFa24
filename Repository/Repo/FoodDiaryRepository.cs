using AutoMapper.Execution;
using BusinessObject.Dto.FoodDiary;
using BusinessObject.Dto.FoodDiaryDetails;
using BusinessObject.Models;
using DataAccess;
using Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repo
{
    public class FoodDiaryRepository : IFoodDiaryRepository
    {
        public Task<bool> AddFoodListToDiaryAsync(FoodDiaryDetailRequestDTO listFoodDiaryDetail) => FoodDiaryDAO.Instance.AddFoodListToDiaryAsync(listFoodDiaryDetail);

        public Task<bool> DeleteFoodListToDiaryAsync(int id) => FoodDiaryDAO.Instance.DeleteFoodListToDiaryAsync(id);

        public Task getDailyFoodDiaryFollowMeal(int dairyID, int mealType) => FoodDiaryDAO.Instance.getDailyFoodDiaryFollowMeal(dairyID, mealType);


        public Task<FoodDiary> GetOrCreateFoodDiaryByDate(int memberId, DateTime date) => FoodDiaryDAO.Instance.GetOrCreateFoodDiaryByDate(memberId, date);

        public Task<UpdateFoodDiaryRequestDTO> UpdateFoodDiary(UpdateFoodDiaryRequestDTO updatedFoodDiary) => FoodDiaryDAO.Instance.UpdateFoodDiary(updatedFoodDiary);
        
    }
}
