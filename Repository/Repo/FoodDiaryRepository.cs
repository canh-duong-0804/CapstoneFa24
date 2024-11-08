using AutoMapper.Execution;
using BusinessObject.Dto.FoodDiary;
using BusinessObject.Dto.FoodDiaryDetails;
using BusinessObject.Dto.MainDashBoardMobile;
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

        public Task<MainDashResponseDTO> GetFoodDairyDetailById(int memberId, DateTime date) => FoodDiaryDAO.Instance.GetFoodDairyDetailById(memberId, date);


        //public Task<MainDashBoardMobileForMemberResponseDTO> GetMainDashBoardForMemberById(int id, DateTime date) => MainDashBoardMobileDAO.Instance.GetMainDashBoardForMemberById(id, date);

        public Task<FoodDiaryForMemberMobileResponse> GetFoodDairyByDate(int memberId, DateTime date) => FoodDiaryDAO.Instance.GetFoodDairyByDate(memberId, date);

    }
}
