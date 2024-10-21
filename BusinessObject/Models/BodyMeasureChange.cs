using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class BodyMeasureChange
    {
        public int BodyMeasureId { get; set; }
        public DateTime? DateChange { get; set; }
        public double? Weight { get; set; }
        public double? BodyFat { get; set; }
        public double? Muscles { get; set; }
        public int MemberId { get; set; }

        public virtual Member Member { get; set; } = null!;
    }
}
