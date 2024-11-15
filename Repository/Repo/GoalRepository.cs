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
        public Task AddGoalAsync(Goal goal) => GoalDAO.Instance.AddGoalAsync(goal);

        public Task<GoalResponseDTO> GetGoalByIdAsync(int id) => GoalDAO.Instance.GetGoalByIdAsync(id);

       

        public Task<bool> UpdateGoalAsync(int memberId, GoalRequestDTO updatedGoal) => GoalDAO.Instance.updateGoal(memberId, updatedGoal);
        

        /* public Task<bool> updateGoal(int memberId, GoalResponseDTO updatedGoal) => GoalDAO.Instance.UpdateGoalAsync(updatedGoal,memberId);*/

    }
}
