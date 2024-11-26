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
        Task<bool> AddCurrentWeightAsync(int memberId, double weightCurrent);
        Task AddGoalAsync(Goal goal, double weight);
        Task<bool> AddGoalLevelExercise(int memberId, string goalWeekDaily);
        Task<bool> AddGoalWeekDaily(int memberId, string goalWeekDaily);
        Task<bool> AddGoalWeightAsync(int memberId, double weightCurrent);
        Task<GoalResponseDTO> GetGoalByIdAsync(int id);
        
        Task<GetInforGoalWeightMemberForGraphResponseDTO> GetInforGoalWeightMemberForGraph(int memberId);
        //Task<GetInforGoalWeightMemberForGraphResponseDTO> GetInforGoalWeightMemberForGraphInMonth(int memberId, DateTime date);

        /* Task<bool> updateGoal(int memberId, GoalRequestDTO updatedGoal);*/
        Task<bool> UpdateGoalAsync(int memberId, GoalRequestDTO updatedGoal);
        /*Task<bool> updateGoal(int memberId, GoalResponseDTO updatedGoal);*/
    }
}
