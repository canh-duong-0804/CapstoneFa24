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
    public class ExecrisePlanRepository : IExecrisePlanRepository
    {
        public Task AddExecrisePlanAsync(ExercisePlan exercisePlan) => ExecrisePlanDAO.Instance.AddExecrisePlanAsync(exercisePlan);
        public Task<List<ExercisePlan>> GetExecrisePlansAsync() => ExecrisePlanDAO.Instance.GetExecrisePlansAsync();
        public Task<ExercisePlan?> GetExecrisePlanByIdAsync(int planId) => ExecrisePlanDAO.Instance.GetExecrisePlanByIdAsync(planId);
        public Task UpdateExecrisePlanAsync(ExercisePlan exercisePlan) => ExecrisePlanDAO.Instance.UpdateExecrisePlanAsync(exercisePlan);
        public Task SoftDeleteExecrisePlanAsync(int planid) => ExecrisePlanDAO.Instance.SoftDeleteExecrisePlanAsync(planid);
        
    }
}
