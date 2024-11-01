using System;

namespace BusinessObject.Dto.BodyMeasurement
{
	public class BodyMeasurementDTO
	{
		public int BodyMeasureId { get; set; }
		public DateTime? DateChange { get; set; }
		public double? Weight { get; set; }
		public double? BodyFat { get; set; }
		public double? Muscles { get; set; }
		public int MemberId { get; set; }
	}
}
