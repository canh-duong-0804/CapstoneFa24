using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class staff
    {
        public staff()
        {
            ExercisePlans = new HashSet<ExercisePlan>();
            Exercises = new HashSet<Exercise>();
            Foods = new HashSet<Food>();
            MealPlans = new HashSet<MealPlan>();
            MessageChats = new HashSet<MessageChat>();
            RefreshTokensStaffs = new HashSet<RefreshTokensStaff>();
        }

        public int StaffId { get; set; }
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public bool Sex { get; set; }
        public string Description { get; set; } = null!;
        public DateTime Dob { get; set; }
        public string StaffImage { get; set; } = null!;
        public string Email { get; set; } = null!;
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;
        public byte Role { get; set; }
        public DateTime StartWorkingDate { get; set; }
        public DateTime? EndWorkingDate { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<ExercisePlan> ExercisePlans { get; set; }
        public virtual ICollection<Exercise> Exercises { get; set; }
        public virtual ICollection<Food> Foods { get; set; }
        public virtual ICollection<MealPlan> MealPlans { get; set; }
        public virtual ICollection<MessageChat> MessageChats { get; set; }
        public virtual ICollection<RefreshTokensStaff> RefreshTokensStaffs { get; set; }
    }
}
