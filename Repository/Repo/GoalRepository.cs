using BusinessObject.Dto.Goal;
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
    public class GoalRepository : IGoalRepository
    {
        public Task<bool> AddCurrentWeightAsync(int memberId, double weightCurrent) => GoalDAO.Instance.AddCurrentWeightAsync(memberId,weightCurrent);
        
        public Task AddGoalAsync(Goal goal) => GoalDAO.Instance.AddGoalAsync(goal);

        public Task<bool> AddGoalLevelExercise(int memberId, string goalLevelDaily) => GoalDAO.Instance.AddGoalLevelExercise(memberId, goalLevelDaily);
       

        public Task<bool> AddGoalWeekDaily(int memberId, string goalWeekDaily) => GoalDAO.Instance.AddGoalWeekDaily(memberId, goalWeekDaily);


        public Task<bool> AddGoalWeightAsync(int memberId, double weightCurrent) => GoalDAO.Instance.AddGoalWeightAsync(memberId, weightCurrent);


        public Task<GoalResponseDTO> GetGoalByIdAsync(int id) => GoalDAO.Instance.GetGoalByIdAsync(id);

       

        public Task<bool> UpdateGoalAsync(int memberId, GoalRequestDTO updatedGoal) => GoalDAO.Instance.updateGoal(memberId, updatedGoal);
        

      
    }
}
