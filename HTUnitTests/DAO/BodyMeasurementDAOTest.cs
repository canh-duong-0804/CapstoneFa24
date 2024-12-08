/*using DataAccess;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HealthTrackingUnitTests.DAO
{
	public class BodyMeasurementDAOTest
	{
		private readonly BodyMeasurementDAO _dao;
		private readonly HealthTrackingDBContext _dbContext;

		public BodyMeasurementDAOTest()
		{
			*//*// Setup in-memory database specifically for this test
			var options = new DbContextOptionsBuilder<HealthTrackingDBContext>()
				.UseInMemoryDatabase(databaseName: "TestDatabase")
				.Options;
			_dbContext = new HealthTrackingDBContext(options);
			_dao = new BodyMeasurementDAO(_dbContext);

			// Seed the database with test data
			_dbContext.BodyMeasureChanges.AddRange(new List<BodyMeasureChange>
			{
				new BodyMeasureChange { BodyMeasureId = 1, Weight = 70, MemberId = 1 },
				new BodyMeasureChange { BodyMeasureId = 2, Weight = 75, MemberId = 2 }
			});
			_dbContext.SaveChanges();*//*
		}

		[Fact]
		public async Task GetAllMeasurementsAsync_ShouldReturnAllMeasurements()
		{
			// Act
			var result = await _dao.GetAllMeasurementsAsync();

			// Assert
			Assert.NotNull(result);
			Assert.NotEmpty(result);
			Assert.Equal(2, result.Count()); // Confirm the expected number of records
		}
	}
}
*/