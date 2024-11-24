using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IExecrisePlanRepository
    {
        /*Task AddExecrisePlanAsync(ExercisePlan exercisePlan);*/
        Task<List<ExercisePlan>> GetExecrisePlansAsync();
        Task<ExercisePlan?> GetExecrisePlanByIdAsync(int planId);
        /*Task UpdateExecrisePlanAsync(ExercisePlan exercisePlan);*/

        Task<List<ExercisePlan>> SearchExercisePlansByNameAsync(string searchTerm);


    }
}
