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
    public class MealMemberDetailsRepository : IMealMemberDetailsRepository
    {
        public Task CreateMealPlanDetailsOfMemberAsync(MealsMemberDetail mealMemberModel, int memberId,DateTime date) => MealPlanMemberDetailsDAO.Instance.CreateMealPlanDetailsOfMemberAsync(mealMemberModel,memberId,date);


    }
}
