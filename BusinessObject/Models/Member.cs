using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class Member
    {
        public Member()
        {
            BodyMeasureChanges = new HashSet<BodyMeasureChange>();
            Comments = new HashSet<Comment>();
            CommunityPosts = new HashSet<CommunityPost>();
            ExerciseDiaries = new HashSet<ExerciseDiary>();
            FoodDiaries = new HashSet<FoodDiary>();
            Goals = new HashSet<Goal>();
            MemberDiseases = new HashSet<MemberDisease>();
            MessageChats = new HashSet<MessageChat>();
            RefreshTokensMembers = new HashSet<RefreshTokensMember>();
            WaterIntakes = new HashSet<WaterIntake>();
        }

        public int MemberId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime Dob { get; set; }
        public string? PhoneNumber { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public bool Gender { get; set; }
        public int ExerciseLevel { get; set; }
        public string Goal { get; set; } = null!;
        public int? DietId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool? Status { get; set; }

        public virtual Diet? Diet { get; set; }
        public virtual ICollection<BodyMeasureChange> BodyMeasureChanges { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<CommunityPost> CommunityPosts { get; set; }
        public virtual ICollection<ExerciseDiary> ExerciseDiaries { get; set; }
        public virtual ICollection<FoodDiary> FoodDiaries { get; set; }
        public virtual ICollection<Goal> Goals { get; set; }
        public virtual ICollection<MemberDisease> MemberDiseases { get; set; }
        public virtual ICollection<MessageChat> MessageChats { get; set; }
        public virtual ICollection<RefreshTokensMember> RefreshTokensMembers { get; set; }
        public virtual ICollection<WaterIntake> WaterIntakes { get; set; }
    }
}
