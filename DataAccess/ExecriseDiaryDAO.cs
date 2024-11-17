﻿using AutoMapper.Execution;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio.TwiML.Messaging;

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

		//create method to get exercise diary by member id
		public async Task<List<ExerciseDiary>> GetExerciseDiaryByMemberId(int memberId)
		{
			try
			{
				using (var context = new HealthTrackingDBContext())
				{
					// Include related navigation properties
					var exerciseDiaries = await context.ExerciseDiaries
						.Include(ed => ed.ExerciseDiaryDetails) // Assuming Exercise is the related entity
						.Include(ed => ed.ExercisePlan) // Assuming ExercisePlan is the related entity
						.Include(ed => ed.Member) // Assuming Member is the related entity
						.Where(ed => ed.MemberId == memberId) // Filter by MemberId
						.ToListAsync();

					return exerciseDiaries;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		/*public async Task<ExerciseDiary> GetTodayExerciseDiaryByMemberId(int memberId, DateTime today)
		{
			return await _context.ExerciseDiaries
				.FirstOrDefaultAsync(d => d.MemberId == memberId && d.Date == today);
		}

		public async Task AddExerciseDiaryAsync(ExerciseDiary exerciseDiary)
		{
			await _context.ExerciseDiaries.AddAsync(exerciseDiary);
			await _context.SaveChangesAsync();
		}*/

		// rewrite using try catch block like above
		public async Task<ExerciseDiary> GetTodayExerciseDiaryByMemberId(int memberId, DateTime today)
		{
			try
			{
				using (var context = new HealthTrackingDBContext())
				{
					return await context.ExerciseDiaries
				.FirstOrDefaultAsync(d => d.MemberId == memberId && d.Date == today);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public async Task AddExerciseDiaryAsync(ExerciseDiary exerciseDiary)
		{
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
				throw ex;
			}
		}

		//rewrite using try catch block like above
		public async Task<ExerciseDiary> GetExerciseDiaryByDate(int memberId, DateTime date)
		{
			try
			{
				using (var context = new HealthTrackingDBContext())
				{
					return await context.ExerciseDiaries
				.Include(d => d.ExerciseDiaryDetails) // Include related details
				.FirstOrDefaultAsync(d => d.MemberId == memberId && d.Date == date);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task UpdateTotalDurationAndCaloriesAsync(int exerciseDiaryId)
		{
			try
			{
				using (var context = new HealthTrackingDBContext())
				{
					// Fetch the exercise diary and related details
					var exerciseDiary = await context.ExerciseDiaries
						.Include(ed => ed.ExerciseDiaryDetails)
						.FirstOrDefaultAsync(e => e.ExerciseDiaryId == exerciseDiaryId);

					if (exerciseDiary == null)
					{
						throw new Exception("Exercise diary not found.");
					}

					// Filter only practiced exercises
					var practicedExercises = exerciseDiary.ExerciseDiaryDetails
				.Where(detail => detail.IsPractice == true)
				.ToList();

					// Calculate totals
					double totalCaloriesBurned = practicedExercises.Sum(detail => detail.CaloriesBurned ?? 0);
					int totalDuration = practicedExercises.Sum(detail => detail.Duration ?? 0);

					// Update the exercise diary totals
					exerciseDiary.TotalCaloriesBurned = totalCaloriesBurned;
					exerciseDiary.TotalDuration = totalDuration;

					// Save changes
					await context.SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error updating totals: {ex.Message}");
				throw;
			}
		}

		public async Task<ExerciseDiary> GetExerciseDiaryById(int exerciseDiaryId)
		{
			try
			{
				using (var context = new HealthTrackingDBContext())
				{
					return await context.ExerciseDiaries
				.FirstOrDefaultAsync(d => d.ExerciseDiaryId == exerciseDiaryId);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

	}
}
