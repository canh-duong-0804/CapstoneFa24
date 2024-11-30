using BusinessObject.Dto.ExecrisePlan;
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
    public class ExecrisePlanTrainerRepository : IExecrisePlanTrainerRepository
    {
        public Task<GetExercisePlanResponseForTrainerDTO> GetAllExercisePlansAsync(int page, int pageSize) => ExercisePlanTrainerDAO.Instance.GetAllExercisePlansAsync(page, pageSize);
        public Task<bool> AddExercisePlanAsync(ExercisePlan exercisePlan) => ExercisePlanTrainerDAO.Instance.AddExercisePlanAsync(exercisePlan);
       public Task<ExercisePlan?> GetExercisePlanByIdAsync(int id) => ExercisePlanTrainerDAO.Instance.GetExercisePlanByIdAsync(id);
        public Task<List<ExercisePlan>> GetExercisePlansAsync() => ExercisePlanTrainerDAO.Instance.GetExercisePlansAsync();
        public Task<bool> UpdateExercisePlanAsync(ExercisePlan exercisePlan) => ExercisePlanTrainerDAO.Instance.UpdateExercisePlanAsync(exercisePlan);
        public Task<bool> SoftDeleteExercisePlanAsync(int planId) => ExercisePlanTrainerDAO.Instance.SoftDeleteExercisePlanAsync(planId);
        public Task<List<ExercisePlanDetail>> GetExercisePlanDetailsAsync(int planId) => ExercisePlanTrainerDAO.Instance.GetExercisePlanDetailsByPlanIdAsync(planId);

        public Task<bool> AddExercisePlanDetailAsync(List<ExercisePlanDetail> details) => ExercisePlanTrainerDAO.Instance.AddExercisePlanDetailAsync(details);
      //  public Task<bool> UpdateExercisePlanAsync(ExercisePlanDetail detail) => ExercisePlanTrainerDAO.Instance.UpdateExercisePlanDetailAsync(detail);
        public Task<bool> DeleteExercisePlanDetailAsync(int detailId) => ExercisePlanTrainerDAO.Instance.DeleteExercisePlanDetailAsync(detailId);

        public Task<ExercisePlanDetail?> GetExercisePlanDetailByIdAsync(int detailId) => ExercisePlanTrainerDAO.Instance.GetExercisePlanDetailByIdAsync(detailId);

        public Task<GetExercisePlanDetailDTO> GetExercisePlanDetailAsync(int exercisePlanId, int day) => ExercisePlanTrainerDAO.Instance.GetExercisePlanDetailAsync(exercisePlanId,day);

        public Task<bool> UpdateExercisePlanDetailAsync(GetExercisePlanDetailDTO detail) => ExercisePlanTrainerDAO.Instance.UpdateExercisePlanDetailOfTrainerAsync(detail);

        
    }
}
