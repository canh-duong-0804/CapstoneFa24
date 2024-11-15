using BusinessObject.Dto.Goal;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IGoalRepository
    {
        Task AddGoalAsync(Goal goal);
        Task<GoalResponseDTO> GetGoalByIdAsync(int id);
       /* Task<bool> updateGoal(int memberId, GoalRequestDTO updatedGoal);*/
        Task<bool> UpdateGoalAsync(int memberId, GoalRequestDTO updatedGoal);
        /*Task<bool> updateGoal(int memberId, GoalResponseDTO updatedGoal);*/
    }
}
