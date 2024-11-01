using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
	public class BodyMeasurementDAO
	{
		private static BodyMeasurementDAO instance = null;
		private static readonly object instanceLock = new object();

		public static BodyMeasurementDAO Instance
		{
			get
			{
				lock (instanceLock)
				{
					if (instance == null)
					{
						instance = new BodyMeasurementDAO();
					}
					return instance;
				}
			}
		}

		// Get all body measurements
		public async Task<IEnumerable<BodyMeasureChange>> GetAllMeasurementsAsync()
		{
			try
			{
				using var context = new HealthTrackingDBContext();
				return await context.BodyMeasureChanges.ToListAsync();
			}
			catch (Exception ex)
			{
				throw new Exception($"Error retrieving body measurements: {ex.Message}");
			}
		}

		// Get body measurements by Member ID
		public async Task<IEnumerable<BodyMeasureChange>> GetMeasurementsByMemberIdAsync(int memberId)
		{
			try
			{
				using var context = new HealthTrackingDBContext();
				return await context.BodyMeasureChanges
					.Where(m => m.MemberId == memberId)
					.ToListAsync();
			}
			catch (Exception ex)
			{
				throw new Exception($"Error retrieving body measurements for member ID {memberId}: {ex.Message}");
			}
		}

		// Get a specific body measurement by ID
		public async Task<BodyMeasureChange?> GetMeasurementByIdAsync(int bodyMeasureId)
		{
			try
			{
				using var context = new HealthTrackingDBContext();
				return await context.BodyMeasureChanges.FindAsync(bodyMeasureId);
			}
			catch (Exception ex)
			{
				throw new Exception($"Error retrieving body measurement with ID {bodyMeasureId}: {ex.Message}");
			}
		}

		// Create a new body measurement
		public async Task<BodyMeasureChange> CreateMeasurementAsync(BodyMeasureChange measurement)
		{
			try
			{
				using var context = new HealthTrackingDBContext();
				measurement.DateChange = DateTime.Now;
				await context.BodyMeasureChanges.AddAsync(measurement);
				await context.SaveChangesAsync();
				return measurement;
			}
			catch (Exception ex)
			{
				throw new Exception($"Error creating body measurement: {ex.Message}");
			}
		}

		// Update an existing body measurement
		public async Task<BodyMeasureChange?> UpdateMeasurementAsync(BodyMeasureChange updatedMeasurement)
		{
			try
			{
				using var context = new HealthTrackingDBContext();
				var measurement = await context.BodyMeasureChanges.FindAsync(updatedMeasurement.BodyMeasureId);
				if (measurement != null)
				{
					measurement.Weight = updatedMeasurement.Weight;
					measurement.BodyFat = updatedMeasurement.BodyFat;
					measurement.Muscles = updatedMeasurement.Muscles;
					measurement.DateChange = DateTime.Now;
					measurement.MemberId = updatedMeasurement.MemberId;

					await context.SaveChangesAsync();
				}
				return measurement;
			}
			catch (Exception ex)
			{
				throw new Exception($"Error updating body measurement with ID {updatedMeasurement.BodyMeasureId}: {ex.Message}");
			}
		}

		// Delete a body measurement by ID
		public async Task<bool> DeleteMeasurementAsync(int bodyMeasureId)
		{
			try
			{
				using var context = new HealthTrackingDBContext();
				var measurement = await context.BodyMeasureChanges.FindAsync(bodyMeasureId);
				if (measurement != null)
				{
					context.BodyMeasureChanges.Remove(measurement);
					await context.SaveChangesAsync();
					return true;
				}
				return false;
			}
			catch (Exception ex)
			{
				throw new Exception($"Error deleting body measurement with ID {bodyMeasureId}: {ex.Message}");
			}
		}
	}
}
