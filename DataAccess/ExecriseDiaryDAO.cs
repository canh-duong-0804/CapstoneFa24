using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
	public class ExerciseDiaryDAO
	{
		private static ExerciseDiaryDAO instance = null;
		private static readonly object instanceLock = new object();

		public static ExerciseDiaryDAO Instance
		{
			get
			{
				lock (instanceLock)
				{
					if (instance == null)
					{
						instance = new ExerciseDiaryDAO();
					}
					return instance;
				}
			}
		}

		public async Task AddExerciseDiary(ExerciseDiary exerciseDiary)
		{
			if (exerciseDiary == null)
				throw new ArgumentNullException(nameof(exerciseDiary), "Exercise diary entry cannot be null");

			try
			{
				using (var context = new HealthTrackingDBContext())
				{
					await context.ExerciseDiaries.AddAsync(exerciseDiary);
					await context.SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{
				throw new Exception($"Error creating exercise diary entry: {ex.Message}", ex);
			}
		}

		public async Task<List<ExerciseDiary>> GetExerciseDiaryByMemberId(int memberId)
		{
			try
			{
				using (var context = new HealthTrackingDBContext())
				{
					return await context.ExerciseDiaries
						.Include(e => e.Exercise)
						.Include(e => e.ExercisePlan)
						.Include(e => e.Member)
						.Where(e => e.MemberId == memberId)
						.ToListAsync();
				}
			}
			catch (Exception ex)
			{
				throw new Exception($"Error getting exercise diary entries: {ex.Message}", ex);
			}
		}

		public async Task AddExerciseDiaries(List<ExerciseDiary> exerciseDiaries)
		{
			if (exerciseDiaries == null || !exerciseDiaries.Any())
				throw new ArgumentNullException(nameof(exerciseDiaries), "Exercise diary entries cannot be null or empty");

			try
			{
				using (var context = new HealthTrackingDBContext())
				{
					await context.ExerciseDiaries.AddRangeAsync(exerciseDiaries);
					await context.SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{
				throw new Exception($"Error creating exercise diary entries: {ex.Message}", ex);
			}
		}

		// New method to check if ExercisePlan exists
		public async Task<bool> CheckExercisePlanExistsAsync(int exercisePlanId)
		{
			try
			{
				using (var context = new HealthTrackingDBContext())
				{
					return await context.ExercisePlans.AnyAsync(ep => ep.ExercisePlanId == exercisePlanId);
				}
			}
			catch (Exception ex)
			{
				throw new Exception($"Error checking ExercisePlan existence: {ex.Message}", ex);
			}
		}

		// New method to check if Exercise exists
		public async Task<bool> CheckExerciseExistsAsync(int exerciseId)
		{
			try
			{
				using (var context = new HealthTrackingDBContext())
				{
					return await context.Exercises.AnyAsync(e => e.ExerciseId == exerciseId);
				}
			}
			catch (Exception ex)
			{
				throw new Exception($"Error checking Exercise existence: {ex.Message}", ex);
			}
		}
	}
}
