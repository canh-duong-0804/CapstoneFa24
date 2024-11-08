using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class MealMemberDetail
    {
        public int DetailId { get; set; }
        public int MealMemberId { get; set; }
        public int MemberId { get; set; }
        public int FoodId { get; set; }
        public int? Quantity { get; set; }

        public virtual Food Food { get; set; } = null!;
        public virtual MealMember MealMember { get; set; } = null!;
    }
}
