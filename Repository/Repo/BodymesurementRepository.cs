using BusinessObject.Models;
using DataAccess;
using Repository.IRepo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Repo
{
	public class BodyMeasurementRepository : IBodyMesurementRepository
	{
		public Task<BodyMeasureChange> CreateMeasurementAsync(BodyMeasureChange measurement) => BodyMeasurementDAO.Instance.CreateMeasurementAsync(measurement);

		public Task<bool> DeleteMeasurementAsync(int bodyMeasureId) => BodyMeasurementDAO.Instance.DeleteMeasurementAsync(bodyMeasureId);

		public Task<IEnumerable<BodyMeasureChange>> GetAllMeasurementsAsync() => BodyMeasurementDAO.Instance.GetAllMeasurementsAsync();

		public Task<BodyMeasureChange?> GetMeasurementByIdAsync(int bodyMeasureId) => BodyMeasurementDAO.Instance.GetMeasurementByIdAsync(bodyMeasureId);

		public Task<IEnumerable<BodyMeasureChange>> GetMeasurementsByMemberIdAsync(int memberId) => BodyMeasurementDAO.Instance.GetMeasurementsByMemberIdAsync(memberId);

		public Task<BodyMeasureChange?> UpdateMeasurementAsync(BodyMeasureChange updatedMeasurement) => BodyMeasurementDAO.Instance.UpdateMeasurementAsync(updatedMeasurement);
	}
}
