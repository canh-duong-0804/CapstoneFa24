using BusinessObject.Dto.ExecrisePlan;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio.Http;

namespace DataAccess
{
    public class ExercisePlanTrainerDAO
    {
        private static ExercisePlanTrainerDAO instance = null;
        private static readonly object instanceLock = new object();

        public static ExercisePlanTrainerDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ExercisePlanTrainerDAO();
                    }
                    return instance;
                }
            }
        }

        // Add Exercise Plan
        public async Task<bool> AddExercisePlanAsync(ExercisePlan exercisePlan)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    await context.ExercisePlans.AddAsync(exercisePlan);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Get Exercise Plan by ID (Active Only)
        public async Task<ExercisePlan?> GetExercisePlanByIdAsync(int id)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    return await context.ExercisePlans
                        .Include(p => p.ExercisePlanDetails)
                        .ThenInclude(d => d.Exercise)
                        .FirstOrDefaultAsync(p => p.ExercisePlanId == id && p.Status == true);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        // Get All Active Exercise Plans
        public async Task<List<ExercisePlan>> GetExercisePlansAsync()
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    return await context.ExercisePlans
                        .Include(p => p.ExercisePlanDetails)
                        .ThenInclude(d => d.Exercise)
                        .Where(p => p.Status == true)
                        .ToListAsync();
                }
            }
            catch (Exception)
            {
                return new List<ExercisePlan>();
            }
        }

        // Update Exercise Plan
        public async Task<bool> UpdateExercisePlanAsync(ExercisePlan exercisePlan)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    context.ExercisePlans.Update(exercisePlan);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Soft Delete Exercise Plan
        public async Task<bool> SoftDeleteExercisePlanAsync(int planId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var exercisePlan = await context.ExercisePlans
                        .Include(p => p.ExercisePlanDetails)
                        .FirstOrDefaultAsync(p => p.ExercisePlanId == planId);

                    if (exercisePlan == null)
                        return false;

                    // Mark Exercise Plan and its details as inactive
                    exercisePlan.Status = false;
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Get Exercise Plan Details by Plan ID
        public async Task<List<ExercisePlanDetail>> GetExercisePlanDetailsByPlanIdAsync(int planId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    return await context.ExercisePlanDetails
                        .Where(d => d.ExercisePlanId == planId)
                        .ToListAsync();
                }
            }
            catch (Exception)
            {
                return new List<ExercisePlanDetail>();
            }
        }

        // Add Exercise Plan Detail
        // Repository Method to Add Multiple Exercise Plan Details
        public async Task<bool> AddExercisePlanDetailAsync(List<ExercisePlanDetail> details)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    await context.ExercisePlanDetails.AddRangeAsync(details);  // Add multiple records at once
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        // Update Exercise Plan Detail
        public async Task<bool> UpdateExercisePlanDetailAsync(ExercisePlanDetail detail)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    context.ExercisePlanDetails.Update(detail);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Delete Exercise Plan Detail
        public async Task<bool> DeleteExercisePlanDetailAsync(int detailId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var detail = await context.ExercisePlanDetails.FindAsync(detailId);
                    if (detail == null)
                        return false;

                    context.ExercisePlanDetails.Remove(detail);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Get Exercise Plan Detail by ID
        public async Task<ExercisePlanDetail?> GetExercisePlanDetailByIdAsync(int detailId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    return await context.ExercisePlanDetails
                        .FirstOrDefaultAsync(d => d.ExercisePlanDetailId == detailId);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<GetExercisePlanResponseForTrainerDTO> GetAllExercisePlansAsync(int page, int pageSize)
        {
            try
            {
                using var context = new HealthTrackingDBContext();

                // Tính tổng số bản ghi
                var totalRecords = await context.ExercisePlans.CountAsync();

                // Lấy danh sách bài tập theo trang
                var exercisePlans = await context.ExercisePlans

                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Where(p=>p.Status==true)
                    .OrderByDescending(p=>p.ExercisePlanId)
                    .Select(ep => new ExercisePlanDTO
                    {
                        ExercisePlanId = ep.ExercisePlanId,
                        Name = ep.Name,
                        TotalCaloriesBurned = ep.TotalCaloriesBurned,
                        ExercisePlanImage = ep.ExercisePlanImage,
                        TotalDay= context.ExercisePlanDetails
                    .Where(detail => detail.ExercisePlanId == ep.ExercisePlanId)
                    .Max(detail => (int?)detail.Day) ?? 0,

                        AvgDuration = context.ExercisePlanDetails
                    .Where(detail => detail.ExercisePlanId == ep.ExercisePlanId)
                    .Average(detail => (double?)detail.Duration) ?? 0
                    })
                    .ToListAsync();

                // Tạo phản hồi
                return new GetExercisePlanResponseForTrainerDTO
                {
                    Data = exercisePlans, // Đây là List<ExercisePlanDTO>
                    TotalRecords = totalRecords,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize)
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching exercise plans: {ex.Message}", ex);
            }

        }

        public async Task<GetExercisePlanDetailDTO> GetExercisePlanDetailAsync(int exercisePlanId, int day)
        {
            try
            {
                using var context = new HealthTrackingDBContext();


                var exercises = await context.ExercisePlanDetails
                    .Where(epd => epd.ExercisePlanId == exercisePlanId && epd.Day == day)
                    .Select(epd => new DayExerciseDTO
                    {
                        ExerciseId = epd.ExerciseId,
                        ExerciseName = epd.Exercise.ExerciseName,
                        Duration = epd.Duration,
                    })
                    .ToListAsync();

                if (!exercises.Any())
                {
                    return null;
                }

                // Tạo phản hồi DTO
                return new GetExercisePlanDetailDTO
                {
                    ExercisePlanId = exercisePlanId,
                    Day = (byte)day,
                    execriseInPlans = exercises
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching exercise plans: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateExercisePlanDetailOfTrainerAsync(GetExercisePlanDetailDTO request)
        {
            try
            {
                using var _context = new HealthTrackingDBContext();

                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    // Xóa danh sách bài tập cũ
                    var existingDetails = _context.ExercisePlanDetails
                    .Where(epd => epd.ExercisePlanId == request.ExercisePlanId && epd.Day == request.Day);

                    _context.ExercisePlanDetails.RemoveRange(existingDetails);

                    // Thêm danh sách bài tập mới
                    var newDetails = request.execriseInPlans.Select(ex => new ExercisePlanDetail
                    {
                        ExercisePlanId = request.ExercisePlanId,
                        Day = request.Day,
                        ExerciseId = ex.ExerciseId,
                        Duration = ex.Duration
                    });

                    await _context.ExercisePlanDetails.AddRangeAsync(newDetails);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating exercise plan details: {ex.Message}", ex);
            }
        }
    }
}
