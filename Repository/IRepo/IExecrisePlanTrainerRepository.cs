using BusinessObject.Dto.ExecrisePlan;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IExecrisePlanTrainerRepository
    {
        Task<GetExercisePlanResponseForTrainerDTO> GetAllExercisePlansAsync(int page, int pageSize);
        Task<bool> AddExercisePlanAsync(ExercisePlan exercisePlan);
        Task<ExercisePlan?> GetExercisePlanByIdAsync(int id);
        Task<List<ExercisePlan>> GetExercisePlansAsync();
        Task<bool> UpdateExercisePlanAsync(ExercisePlan exercisePlan);
        Task<bool> SoftDeleteExercisePlanAsync(int planId);
        Task<bool> AddExercisePlanDetailAsync(List<ExercisePlanDetail> details);
        Task<List<ExercisePlanDetail>> GetExercisePlanDetailsAsync(int planId);
        Task<bool> UpdateExercisePlanDetailAsync(ExercisePlanDetail detail);

        Task<bool> DeleteExercisePlanDetailAsync(int detailId);

        Task<ExercisePlanDetail?> GetExercisePlanDetailByIdAsync(int detailId);

    }
}
