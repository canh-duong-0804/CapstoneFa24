using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class Diet
    {
        public Diet()
        {
            Foods = new HashSet<Food>();
            MealPlans = new HashSet<MealPlan>();
            Members = new HashSet<Member>();
        }

        public int DietId { get; set; }
        public string DietName { get; set; } = null!;
        public DateTime? CreateDate { get; set; }
        public DateTime? ChangeDate { get; set; }
        public int CreatedBy { get; set; }
        public int? ChangeBy { get; set; }
        public string ShortDescription { get; set; } = null!;

        public virtual ICollection<Food> Foods { get; set; }
        public virtual ICollection<MealPlan> MealPlans { get; set; }
        public virtual ICollection<Member> Members { get; set; }
    }
}
