using System;
using System.Collections.Generic;

namespace BusinessObject.Entities
{
    public partial class WaterIntake
    {
        public int IntakeId { get; set; }
        public int MemberId { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }

        public virtual Member Member { get; set; } = null!;
    }
}
