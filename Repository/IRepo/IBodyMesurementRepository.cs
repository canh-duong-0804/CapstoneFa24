using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
	public interface IBodyMesurementRepository
	{
		// Get all body measurements
		Task<IEnumerable<BodyMeasureChange>> GetAllMeasurementsAsync();

		// Get body measurements by Member ID
		Task<IEnumerable<BodyMeasureChange>> GetMeasurementsByMemberIdAsync(int memberId);

		// Get a specific body measurement by ID
		Task<BodyMeasureChange?> GetMeasurementByIdAsync(int bodyMeasureId);

		// Create a new body measurement
		Task<BodyMeasureChange> CreateMeasurementAsync(BodyMeasureChange measurement);

		// Update an existing body measurement
		Task<BodyMeasureChange?> UpdateMeasurementAsync(BodyMeasureChange updatedMeasurement);

		// Delete a body measurement by ID
		Task<bool> DeleteMeasurementAsync(int bodyMeasureId);
	}
}
