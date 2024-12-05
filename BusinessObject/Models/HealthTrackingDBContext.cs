using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BusinessObject.Models
{
    public partial class HealthTrackingDBContext : DbContext
    {
        public HealthTrackingDBContext()
        {
        }

        public HealthTrackingDBContext(DbContextOptions<HealthTrackingDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Blog> Blogs { get; set; } = null!;
        public virtual DbSet<BodyMeasureChange> BodyMeasureChanges { get; set; } = null!;
        public virtual DbSet<CategoryBlog> CategoryBlogs { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<CommunityPost> CommunityPosts { get; set; } = null!;
        public virtual DbSet<CommunityPostCategory> CommunityPostCategories { get; set; } = null!;
        public virtual DbSet<CompanyInfo> CompanyInfos { get; set; } = null!;
        public virtual DbSet<Diet> Diets { get; set; } = null!;
        public virtual DbSet<Disease> Diseases { get; set; } = null!;
        public virtual DbSet<Exercise> Exercises { get; set; } = null!;
        public virtual DbSet<ExerciseCardio> ExerciseCardios { get; set; } = null!;
        public virtual DbSet<ExerciseDiary> ExerciseDiaries { get; set; } = null!;
        public virtual DbSet<ExerciseDiaryDetail> ExerciseDiaryDetails { get; set; } = null!;
        public virtual DbSet<ExercisePlan> ExercisePlans { get; set; } = null!;
        public virtual DbSet<ExercisePlanDetail> ExercisePlanDetails { get; set; } = null!;
        public virtual DbSet<ExerciseResistance> ExerciseResistances { get; set; } = null!;
        public virtual DbSet<Faq> Faqs { get; set; } = null!;
        public virtual DbSet<Food> Foods { get; set; } = null!;
        public virtual DbSet<FoodDiary> FoodDiaries { get; set; } = null!;
        public virtual DbSet<FoodDiaryDetail> FoodDiaryDetails { get; set; } = null!;
        public virtual DbSet<FoodMember> FoodMembers { get; set; } = null!;
        public virtual DbSet<Goal> Goals { get; set; } = null!;
        public virtual DbSet<Ingredient> Ingredients { get; set; } = null!;
        public virtual DbSet<MealMember> MealMembers { get; set; } = null!;
        public virtual DbSet<MealMemberDetail> MealMemberDetails { get; set; } = null!;
        public virtual DbSet<MealPlan> MealPlans { get; set; } = null!;
        public virtual DbSet<MealPlanDetail> MealPlanDetails { get; set; } = null!;
        public virtual DbSet<Member> Members { get; set; } = null!;
        public virtual DbSet<MemberDisease> MemberDiseases { get; set; } = null!;
        public virtual DbSet<MemberNotification> MemberNotifications { get; set; } = null!;
        public virtual DbSet<MessageChat> MessageChats { get; set; } = null!;
        public virtual DbSet<MessageChatDetail> MessageChatDetails { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<Recipe> Recipes { get; set; } = null!;
        public virtual DbSet<RecipeIngredient> RecipeIngredients { get; set; } = null!;
        public virtual DbSet<RefreshTokensMember> RefreshTokensMembers { get; set; } = null!;
        public virtual DbSet<RefreshTokensStaff> RefreshTokensStaffs { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;
        public virtual DbSet<WaterIntake> WaterIntakes { get; set; } = null!;
        public virtual DbSet<staff> staffs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                //optionsBuilder.UseSqlServer("Server=tcp:healthtracking.database.windows.net,1433;Initial Catalog=healthtracking;Persist Security Info=False;User ID=healthtracking;Password=CanhDuong0804;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
                optionsBuilder.UseSqlServer("server =.; database =HealthTrackingDB;uid=sa;pwd=123;TrustServerCertificate=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>(entity =>
            {
                entity.ToTable("BLOG");

                entity.Property(e => e.BlogId).HasColumnName("blog_id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.ChangeBy).HasColumnName("change_by");

                entity.Property(e => e.ChangeDate)
                    .HasColumnType("datetime")
                    .HasColumnName("change_date");

                entity.Property(e => e.Content)
                    .HasColumnType("ntext")
                    .HasColumnName("content");

                entity.Property(e => e.CreateBy).HasColumnName("create_by");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("create_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Dislikes).HasColumnName("dislikes");

                entity.Property(e => e.Likes).HasColumnName("likes");

                entity.Property(e => e.ShortDescription)
                    .HasMaxLength(255)
                    .HasColumnName("short_description");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ThumbnailBlog)
                    .HasMaxLength(100)
                    .HasColumnName("thumbnail_blog");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .HasColumnName("title");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Blogs)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__BLOG__category_i__7D439ABD");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.Blogs)
                    .HasForeignKey(d => d.CreateBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BLOG__create_by__7C4F7684");
            });

            modelBuilder.Entity<BodyMeasureChange>(entity =>
            {
                entity.HasKey(e => e.BodyMeasureId)
                    .HasName("PK__BODY_MEA__3FCFA33BA0F15C14");

                entity.ToTable("BODY_MEASURE_CHANGE");

                entity.Property(e => e.BodyMeasureId).HasColumnName("body_measure_id");

                entity.Property(e => e.BodyFat).HasColumnName("body_fat");

                entity.Property(e => e.DateChange)
                    .HasColumnType("datetime")
                    .HasColumnName("date_change");

                entity.Property(e => e.MemberId).HasColumnName("member_id");

                entity.Property(e => e.Muscles).HasColumnName("muscles");

                entity.Property(e => e.Weight).HasColumnName("weight");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.BodyMeasureChanges)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BODY_MEAS__membe__6A30C649");
            });

            modelBuilder.Entity<CategoryBlog>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("PK__CATEGORY__D54EE9B4DBC8A381");

                entity.ToTable("CATEGORY_BLOG");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(255)
                    .HasColumnName("category_name");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("COMMENT");

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.Property(e => e.ChangeBy).HasColumnName("change_by");

                entity.Property(e => e.ChangeDate)
                    .HasColumnType("datetime")
                    .HasColumnName("change_date");

                entity.Property(e => e.Content)
                    .HasColumnType("ntext")
                    .HasColumnName("content");

                entity.Property(e => e.CreateBy).HasColumnName("create_by");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("create_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.CreateBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__COMMENT__create___0A9D95DB");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__COMMENT__post_id__09A971A2");
            });

            modelBuilder.Entity<CommunityPost>(entity =>
            {
                entity.ToTable("COMMUNITY_POST");

                entity.Property(e => e.CommunityPostId).HasColumnName("community_post_id");

                entity.Property(e => e.ChangeBy).HasColumnName("change_by");

                entity.Property(e => e.ChangeDate)
                    .HasColumnType("datetime")
                    .HasColumnName("change_date");

                entity.Property(e => e.CommunityCategoryId).HasColumnName("community_category_id");

                entity.Property(e => e.Content)
                    .HasColumnType("ntext")
                    .HasColumnName("content");

                entity.Property(e => e.CreateBy).HasColumnName("create_by");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("create_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .HasColumnName("title");

                entity.HasOne(d => d.CommunityCategory)
                    .WithMany(p => p.CommunityPosts)
                    .HasForeignKey(d => d.CommunityCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__COMMUNITY__commu__04E4BC85");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.CommunityPosts)
                    .HasForeignKey(d => d.CreateBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__COMMUNITY__creat__03F0984C");
            });

            modelBuilder.Entity<CommunityPostCategory>(entity =>
            {
                entity.HasKey(e => e.CommunityCategoryId)
                    .HasName("PK__COMMUNIT__14F36C7F376DA5B0");

                entity.ToTable("COMMUNITY_POST_CATEGORY");

                entity.Property(e => e.CommunityCategoryId).HasColumnName("community_category_id");

                entity.Property(e => e.CommunityCategoryName)
                    .HasMaxLength(255)
                    .HasColumnName("community_category_name");
            });

            modelBuilder.Entity<CompanyInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("COMPANY_INFO");

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .HasColumnName("address");

                entity.Property(e => e.AppLink)
                    .HasMaxLength(50)
                    .HasColumnName("app_link");

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(50)
                    .HasColumnName("company_name");

                entity.Property(e => e.DescriptionContactUs)
                    .HasMaxLength(50)
                    .HasColumnName("description_contact_us");

                entity.Property(e => e.DescriptionPrivacy)
                    .HasColumnType("ntext")
                    .HasColumnName("description_privacy");

                entity.Property(e => e.DescriptionTerm)
                    .HasColumnType("ntext")
                    .HasColumnName("description_term");

                entity.Property(e => e.Logo)
                    .HasMaxLength(50)
                    .HasColumnName("logo");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50)
                    .HasColumnName("phone_number");

                entity.Property(e => e.SocialLink)
                    .HasMaxLength(50)
                    .HasColumnName("social_link");
            });

            modelBuilder.Entity<Diet>(entity =>
            {
                entity.ToTable("DIETS");

                entity.Property(e => e.DietId).HasColumnName("diet_id");

                entity.Property(e => e.ChangeBy).HasColumnName("change_by");

                entity.Property(e => e.ChangeDate)
                    .HasColumnType("datetime")
                    .HasColumnName("change_date");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("create_date");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.DietName)
                    .HasMaxLength(50)
                    .HasColumnName("diet_name");

                entity.Property(e => e.ShortDescription)
                    .HasMaxLength(50)
                    .HasColumnName("short_description");
            });

            modelBuilder.Entity<Disease>(entity =>
            {
                entity.ToTable("DISEASES");

                entity.Property(e => e.DiseaseId).HasColumnName("disease_id");

                entity.Property(e => e.ChangeBy).HasColumnName("change_by");

                entity.Property(e => e.ChangeDate)
                    .HasColumnType("datetime")
                    .HasColumnName("change_date");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("create_date");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.DiseaseDescription)
                    .HasMaxLength(50)
                    .HasColumnName("disease_description");

                entity.Property(e => e.DiseaseName)
                    .HasMaxLength(50)
                    .HasColumnName("disease_name");
            });

            modelBuilder.Entity<Exercise>(entity =>
            {
                entity.ToTable("EXERCISE");

                entity.Property(e => e.ExerciseId).HasColumnName("exercise_id");

                entity.Property(e => e.ChangeBy).HasColumnName("change_by");

                entity.Property(e => e.ChangeDate)
                    .HasColumnType("datetime")
                    .HasColumnName("change_date");

                entity.Property(e => e.CreateBy).HasColumnName("create_by");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("create_date");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.ExerciseImage).HasColumnName("exercise_image");

                entity.Property(e => e.ExerciseName)
                    .HasMaxLength(100)
                    .HasColumnName("exercise_name");

                entity.Property(e => e.MetValue).HasColumnName("met_value");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.TypeExercise).HasColumnName("type_exercise");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.Exercises)
                    .HasForeignKey(d => d.CreateBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EXERCISE__create__6E01572D");
            });

            modelBuilder.Entity<ExerciseCardio>(entity =>
            {
                entity.HasKey(e => e.ExerciseDetailId)
                    .HasName("PK__EXERCISE__CF31D69CA932B27A");

                entity.ToTable("EXERCISE_CARDIO");

                entity.Property(e => e.ExerciseDetailId).HasColumnName("exercise_detail_id");

                entity.Property(e => e.Calories1).HasColumnName("calories1");

                entity.Property(e => e.Calories2).HasColumnName("calories2");

                entity.Property(e => e.Calories3).HasColumnName("calories3");

                entity.Property(e => e.ExerciseId).HasColumnName("exercise_id");

                entity.Property(e => e.Minutes1).HasColumnName("minutes1");

                entity.Property(e => e.Minutes2).HasColumnName("minutes2");

                entity.Property(e => e.Minutes3).HasColumnName("minutes3");

                entity.HasOne(d => d.Exercise)
                    .WithMany(p => p.ExerciseCardios)
                    .HasForeignKey(d => d.ExerciseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EXERCISE___exerc__73BA3083");
            });

            modelBuilder.Entity<ExerciseDiary>(entity =>
            {
                entity.ToTable("EXERCISE_DIARY");

                entity.Property(e => e.ExerciseDiaryId).HasColumnName("exercise_diary_id");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.ExercisePlanId).HasColumnName("exercise_plan_id");

                entity.Property(e => e.MemberId).HasColumnName("member_id");

                entity.Property(e => e.TotalCaloriesBurned).HasColumnName("total_calories_burned");

                entity.Property(e => e.TotalDuration).HasColumnName("total_duration");

                entity.HasOne(d => d.ExercisePlan)
                    .WithMany(p => p.ExerciseDiaries)
                    .HasForeignKey(d => d.ExercisePlanId)
                    .HasConstraintName("FK__EXERCISE___exerc__2CF2ADDF");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.ExerciseDiaries)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EXERCISE___membe__2DE6D218");
            });

            modelBuilder.Entity<ExerciseDiaryDetail>(entity =>
            {
                entity.HasKey(e => e.ExerciseDiaryDetailsId)
                    .HasName("PK__EXERCISE__28559DE0169DFF2B");

                entity.ToTable("EXERCISE_DIARY_DETAILS");

                entity.Property(e => e.ExerciseDiaryDetailsId).HasColumnName("exercise_diary_details_id");

                entity.Property(e => e.CaloriesBurned).HasColumnName("calories_burned");

                entity.Property(e => e.Duration).HasColumnName("duration");

                entity.Property(e => e.ExerciseDiaryId).HasColumnName("exercise_diary_id");

                entity.Property(e => e.ExerciseId).HasColumnName("exercise_id");

                entity.Property(e => e.IsPractice).HasColumnName("is_practice");

                entity.HasOne(d => d.ExerciseDiary)
                    .WithMany(p => p.ExerciseDiaryDetails)
                    .HasForeignKey(d => d.ExerciseDiaryId)
                    .HasConstraintName("FK__EXERCISE___exerc__30C33EC3");

                entity.HasOne(d => d.Exercise)
                    .WithMany(p => p.ExerciseDiaryDetails)
                    .HasForeignKey(d => d.ExerciseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EXERCISE___exerc__31B762FC");
            });

            modelBuilder.Entity<ExercisePlan>(entity =>
            {
                entity.ToTable("EXERCISE_PLAN");

                entity.Property(e => e.ExercisePlanId).HasColumnName("exercise_plan_id");

                entity.Property(e => e.ChangeBy).HasColumnName("change_by");

                entity.Property(e => e.ChangeDate)
                    .HasColumnType("datetime")
                    .HasColumnName("change_date");

                entity.Property(e => e.CreateBy).HasColumnName("create_by");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("create_date");

                entity.Property(e => e.ExercisePlanImage).HasColumnName("exercise_plan_image");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.TotalCaloriesBurned).HasColumnName("total_calories_burned");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.ExercisePlans)
                    .HasForeignKey(d => d.CreateBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EXERCISE___creat__25518C17");
            });

            modelBuilder.Entity<ExercisePlanDetail>(entity =>
            {
                entity.ToTable("EXERCISE_PLAN_DETAILS");

                entity.Property(e => e.ExercisePlanDetailId).HasColumnName("exercise_plan_detail_id");

                entity.Property(e => e.Day).HasColumnName("day");

                entity.Property(e => e.Duration).HasColumnName("duration");

                entity.Property(e => e.ExerciseId).HasColumnName("exercise_id");

                entity.Property(e => e.ExercisePlanId).HasColumnName("exercise_plan_id");

                entity.HasOne(d => d.Exercise)
                    .WithMany(p => p.ExercisePlanDetails)
                    .HasForeignKey(d => d.ExerciseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EXERCISE___exerc__2A164134");

                entity.HasOne(d => d.ExercisePlan)
                    .WithMany(p => p.ExercisePlanDetails)
                    .HasForeignKey(d => d.ExercisePlanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EXERCISE___exerc__29221CFB");
            });

            modelBuilder.Entity<ExerciseResistance>(entity =>
            {
                entity.HasKey(e => e.ExerciseDetailId)
                    .HasName("PK__EXERCISE__CF31D69C5302775F");

                entity.ToTable("EXERCISE_RESISTANCE");

                entity.Property(e => e.ExerciseDetailId).HasColumnName("exercise_detail_id");

                entity.Property(e => e.ExerciseId).HasColumnName("exercise_id");

                entity.Property(e => e.Minutes1).HasColumnName("minutes1");

                entity.Property(e => e.Minutes2).HasColumnName("minutes2");

                entity.Property(e => e.Minutes3).HasColumnName("minutes3");

                entity.Property(e => e.Reps1).HasColumnName("reps1");

                entity.Property(e => e.Reps2).HasColumnName("reps2");

                entity.Property(e => e.Reps3).HasColumnName("reps3");

                entity.Property(e => e.Sets1).HasColumnName("sets1");

                entity.Property(e => e.Sets2).HasColumnName("sets2");

                entity.Property(e => e.Sets3).HasColumnName("sets3");

                entity.HasOne(d => d.Exercise)
                    .WithMany(p => p.ExerciseResistances)
                    .HasForeignKey(d => d.ExerciseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EXERCISE___exerc__70DDC3D8");
            });

            modelBuilder.Entity<Faq>(entity =>
            {
                entity.HasKey(e => e.QuestionId)
                    .HasName("PK__FAQ__2EC21549EE166821");

                entity.ToTable("FAQ");

                entity.Property(e => e.QuestionId).HasColumnName("question_id");

                entity.Property(e => e.Answer)
                    .HasColumnType("ntext")
                    .HasColumnName("answer");

                entity.Property(e => e.ChangeDate)
                    .HasColumnType("datetime")
                    .HasColumnName("change_date");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("create_date");

                entity.Property(e => e.Question)
                    .HasMaxLength(100)
                    .HasColumnName("question");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<Food>(entity =>
            {
                entity.ToTable("FOOD");

                entity.Property(e => e.FoodId).HasColumnName("food_id");

                entity.Property(e => e.Calories).HasColumnName("calories");

                entity.Property(e => e.Carbs).HasColumnName("carbs");

                entity.Property(e => e.ChangeBy).HasColumnName("change_by");

                entity.Property(e => e.ChangeDate)
                    .HasColumnType("datetime")
                    .HasColumnName("change_date");

                entity.Property(e => e.CreateBy).HasColumnName("create_by");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("create_date");

                entity.Property(e => e.DietId).HasColumnName("diet_id");

                entity.Property(e => e.Fat).HasColumnName("fat");

                entity.Property(e => e.FoodImage).HasColumnName("food_image");

                entity.Property(e => e.FoodName)
                    .HasMaxLength(100)
                    .HasColumnName("food_name");

                entity.Property(e => e.Portion)
                    .HasMaxLength(50)
                    .HasColumnName("portion");

                entity.Property(e => e.Protein).HasColumnName("protein");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.VitaminA).HasColumnName("vitamin_A");

                entity.Property(e => e.VitaminB1).HasColumnName("vitamin_B1");

                entity.Property(e => e.VitaminB2).HasColumnName("vitamin_B2");

                entity.Property(e => e.VitaminB3).HasColumnName("vitamin_B3");

                entity.Property(e => e.VitaminC).HasColumnName("vitamin_C");

                entity.Property(e => e.VitaminD).HasColumnName("vitamin_D");

                entity.Property(e => e.VitaminE).HasColumnName("vitamin_E");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.Foods)
                    .HasForeignKey(d => d.CreateBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FOOD__create_by__52593CB8");

                entity.HasOne(d => d.Diet)
                    .WithMany(p => p.Foods)
                    .HasForeignKey(d => d.DietId)
                    .HasConstraintName("FK__FOOD__diet_id__534D60F1");

                entity.HasMany(d => d.Tags)
                    .WithMany(p => p.Foods)
                    .UsingEntity<Dictionary<string, object>>(
                        "FoodTag",
                        l => l.HasOne<Tag>().WithMany().HasForeignKey("TagId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__FOOD_TAG__tag_id__59063A47"),
                        r => r.HasOne<Food>().WithMany().HasForeignKey("FoodId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__FOOD_TAG__food_i__5812160E"),
                        j =>
                        {
                            j.HasKey("FoodId", "TagId").HasName("PK__FOOD_TAG__5B6527F34D644673");

                            j.ToTable("FOOD_TAG");

                            j.IndexerProperty<int>("FoodId").HasColumnName("food_id");

                            j.IndexerProperty<int>("TagId").HasColumnName("tag_id");
                        });
            });

            modelBuilder.Entity<FoodDiary>(entity =>
            {
                entity.HasKey(e => e.DiaryId)
                    .HasName("PK__FOOD_DIA__339232C8D911DBFD");

                entity.ToTable("FOOD_DIARY");

                entity.Property(e => e.DiaryId).HasColumnName("diary_id");

                entity.Property(e => e.Calories).HasColumnName("calories");

                entity.Property(e => e.Carbs).HasColumnName("carbs");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.Fat).HasColumnName("fat");

                entity.Property(e => e.GoalCalories).HasColumnName("goal_calories");

                entity.Property(e => e.MealPlanId).HasColumnName("meal_plan_id");

                entity.Property(e => e.MemberId).HasColumnName("member_id");

                entity.Property(e => e.Protein).HasColumnName("protein");

                entity.HasOne(d => d.MealPlan)
                    .WithMany(p => p.FoodDiaries)
                    .HasForeignKey(d => d.MealPlanId)
                    .HasConstraintName("FK__FOOD_DIAR__meal___1DB06A4F");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.FoodDiaries)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FOOD_DIAR__membe__1CBC4616");
            });

            modelBuilder.Entity<FoodDiaryDetail>(entity =>
            {
                entity.HasKey(e => e.DiaryDetailId)
                    .HasName("PK__FOOD_DIA__2B203A1F149E2B27");

                entity.ToTable("FOOD_DIARY_DETAIL");

                entity.Property(e => e.DiaryDetailId).HasColumnName("diary_detail_id");

                entity.Property(e => e.DiaryId).HasColumnName("diary_id");

                entity.Property(e => e.FoodId).HasColumnName("food_id");

                entity.Property(e => e.MealType).HasColumnName("meal_type");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Diary)
                    .WithMany(p => p.FoodDiaryDetails)
                    .HasForeignKey(d => d.DiaryId)
                    .HasConstraintName("FK__FOOD_DIAR__diary__208CD6FA");

                entity.HasOne(d => d.Food)
                    .WithMany(p => p.FoodDiaryDetails)
                    .HasForeignKey(d => d.FoodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FOOD_DIAR__food___2180FB33");
            });

            modelBuilder.Entity<FoodMember>(entity =>
            {
                entity.HasKey(e => e.FoodId)
                    .HasName("PK__FOOD_MEM__2F4C4DD802DB7504");

                entity.ToTable("FOOD_MEMBER");

                entity.Property(e => e.FoodId).HasColumnName("food_id");

                entity.Property(e => e.Calories).HasColumnName("calories");

                entity.Property(e => e.Carbs).HasColumnName("carbs");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.Fat).HasColumnName("fat");

                entity.Property(e => e.FoodImage).HasColumnName("food_image");

                entity.Property(e => e.FoodName)
                    .HasMaxLength(100)
                    .HasColumnName("food_name");

                entity.Property(e => e.FormattedId)
                    .HasMaxLength(50)
                    .HasColumnName("formatted_id");

                entity.Property(e => e.Portion)
                    .HasMaxLength(50)
                    .HasColumnName("portion");

                entity.Property(e => e.Protein).HasColumnName("protein");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.VitaminA).HasColumnName("vitamin_A");

                entity.Property(e => e.VitaminB1).HasColumnName("vitamin_B1");

                entity.Property(e => e.VitaminB2).HasColumnName("vitamin_B2");

                entity.Property(e => e.VitaminB3).HasColumnName("vitamin_B3");

                entity.Property(e => e.VitaminC).HasColumnName("vitamin_C");

                entity.Property(e => e.VitaminD).HasColumnName("vitamin_D");

                entity.Property(e => e.VitaminE).HasColumnName("vitamin_E");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.FoodMembers)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FOOD_MEMB__creat__3E52440B");
            });

            modelBuilder.Entity<Goal>(entity =>
            {
                entity.ToTable("GOAL");

                entity.Property(e => e.GoalId).HasColumnName("goal_id");

                entity.Property(e => e.ChangeDate)
                    .HasColumnType("datetime")
                    .HasColumnName("change_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ExerciseLevel).HasColumnName("exercise_level");

                entity.Property(e => e.GoalType)
                    .HasMaxLength(50)
                    .HasColumnName("goal_type");

                entity.Property(e => e.MemberId).HasColumnName("member_id");

                entity.Property(e => e.TargetDate)
                    .HasColumnType("date")
                    .HasColumnName("target_date");

                entity.Property(e => e.TargetValue).HasColumnName("target_value");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Goals)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GOAL__member_id__6754599E");
            });

            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.ToTable("INGREDIENT");

                entity.Property(e => e.IngredientId).HasColumnName("ingredient_id");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<MealMember>(entity =>
            {
                entity.ToTable("MEAL_MEMBER");

                entity.Property(e => e.MealMemberId).HasColumnName("meal_member_id");

                entity.Property(e => e.Image).HasColumnName("image");

                entity.Property(e => e.MealDate)
                    .HasColumnType("datetime")
                    .HasColumnName("meal_date");

                entity.Property(e => e.MemberId).HasColumnName("member_id");

                entity.Property(e => e.NameMealMember)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("name_meal_member");

                entity.Property(e => e.TotalCalories).HasColumnName("total_calories");

                entity.Property(e => e.TotalCarb).HasColumnName("total_carb");

                entity.Property(e => e.TotalFat).HasColumnName("total_fat");

                entity.Property(e => e.TotalProtein).HasColumnName("total_protein");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.MealMembers)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MEAL_MEMB__membe__3A4CA8FD");
            });

            modelBuilder.Entity<MealMemberDetail>(entity =>
            {
                entity.HasKey(e => e.DetailId)
                    .HasName("PK__MEAL_MEM__38E9A224099D52DF");

                entity.ToTable("MEAL_MEMBER_DETAILS");

                entity.Property(e => e.DetailId).HasColumnName("detail_id");

                entity.Property(e => e.FoodId).HasColumnName("food_id");

                entity.Property(e => e.MealMemberId).HasColumnName("meal_member_id");

                entity.Property(e => e.MemberId).HasColumnName("member_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Food)
                    .WithMany(p => p.MealMemberDetails)
                    .HasForeignKey(d => d.FoodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MEAL_MEMB__food___3E1D39E1");

                entity.HasOne(d => d.MealMember)
                    .WithMany(p => p.MealMemberDetails)
                    .HasForeignKey(d => d.MealMemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MEAL_MEMB__meal___3D2915A8");
            });

            modelBuilder.Entity<MealPlan>(entity =>
            {
                entity.ToTable("MEAL_PLAN");

                entity.Property(e => e.MealPlanId).HasColumnName("meal_plan_id");

                entity.Property(e => e.ChangeBy).HasColumnName("change_by");

                entity.Property(e => e.ChangeDate)
                    .HasColumnType("datetime")
                    .HasColumnName("change_date");

                entity.Property(e => e.CreateBy).HasColumnName("create_by");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DietId).HasColumnName("diet_id");

                entity.Property(e => e.LongDescription).HasColumnName("long_description");

                entity.Property(e => e.MealPlanImage).HasColumnName("meal_plan_image");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.ShortDescription)
                    .HasMaxLength(255)
                    .HasColumnName("short_description");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.TotalCalories).HasColumnName("total_calories");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.MealPlans)
                    .HasForeignKey(d => d.CreateBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MEAL_PLAN__creat__123EB7A3");

                entity.HasOne(d => d.Diet)
                    .WithMany(p => p.MealPlans)
                    .HasForeignKey(d => d.DietId)
                    .HasConstraintName("FK__MEAL_PLAN__diet___1332DBDC");
            });

            modelBuilder.Entity<MealPlanDetail>(entity =>
            {
                entity.ToTable("MEAL_PLAN_DETAILS");

                entity.Property(e => e.MealPlanDetailId).HasColumnName("meal_plan_detail_id");

                entity.Property(e => e.Day).HasColumnName("day");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.FoodId).HasColumnName("food_id");

                entity.Property(e => e.MealDate)
                    .HasColumnType("date")
                    .HasColumnName("meal_date");

                entity.Property(e => e.MealPlanId).HasColumnName("meal_plan_id");

                entity.Property(e => e.MealType).HasColumnName("meal_type");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Food)
                    .WithMany(p => p.MealPlanDetails)
                    .HasForeignKey(d => d.FoodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MEAL_PLAN__food___19DFD96B");

                entity.HasOne(d => d.MealPlan)
                    .WithMany(p => p.MealPlanDetails)
                    .HasForeignKey(d => d.MealPlanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MEAL_PLAN__meal___18EBB532");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.ToTable("MEMBER");

                entity.HasIndex(e => e.Email, "UQ__MEMBER__AB6E6164F223719E")
                    .IsUnique();

                entity.Property(e => e.MemberId).HasColumnName("member_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DietId).HasColumnName("diet_id");

                entity.Property(e => e.Dob)
                    .HasColumnType("datetime")
                    .HasColumnName("dob");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.ExerciseLevel).HasColumnName("exercise_level");

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.Height).HasColumnName("height");

                entity.Property(e => e.ImageMember).HasColumnName("image_member");

                entity.Property(e => e.IsComment)
                    .HasColumnName("is_comment")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50)
                    .HasColumnName("phone_number");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .HasColumnName("username");

                entity.HasOne(d => d.Diet)
                    .WithMany(p => p.Members)
                    .HasForeignKey(d => d.DietId)
                    .HasConstraintName("FK__MEMBER__diet_id__30F848ED");
            });

            modelBuilder.Entity<MemberDisease>(entity =>
            {
                entity.HasKey(e => e.IdMemberDisease)
                    .HasName("PK__MEMBER_D__9AA485E46D46D38C");

                entity.ToTable("MEMBER_DISEASE");

                entity.Property(e => e.IdMemberDisease).HasColumnName("id_member_disease");

                entity.Property(e => e.DiagnosedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("diagnosed_date");

                entity.Property(e => e.DiseaseId).HasColumnName("disease_id");

                entity.Property(e => e.MemberId).HasColumnName("member_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Disease)
                    .WithMany(p => p.MemberDiseases)
                    .HasForeignKey(d => d.DiseaseId)
                    .HasConstraintName("FK__MEMBER_DI__disea__4E88ABD4");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.MemberDiseases)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK__MEMBER_DI__membe__4D94879B");
            });

            modelBuilder.Entity<MemberNotification>(entity =>
            {
                entity.ToTable("MEMBER_NOTIFICATION");

                entity.Property(e => e.MemberNotificationId).HasColumnName("member_notification_id");

                entity.Property(e => e.MemberId).HasColumnName("member_id");

                entity.Property(e => e.NotificationId).HasColumnName("notification_id");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.MemberNotifications)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MEMBER_NO__membe__38996AB5");

                entity.HasOne(d => d.Notification)
                    .WithMany(p => p.MemberNotifications)
                    .HasForeignKey(d => d.NotificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MEMBER_NO__notif__398D8EEE");
            });

            modelBuilder.Entity<MessageChat>(entity =>
            {
                entity.ToTable("MESSAGE_CHAT");

                entity.Property(e => e.MessageChatId).HasColumnName("message_chat_id");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MemberId).HasColumnName("member_id");

                entity.Property(e => e.RateStar).HasColumnName("rate_star");

                entity.Property(e => e.StaffId).HasColumnName("staff_id");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.MessageChats)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK__MESSAGE_C__membe__440B1D61");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.MessageChats)
                    .HasForeignKey(d => d.StaffId)
                    .HasConstraintName("FK__MESSAGE_C__creat__4316F928");
            });

            modelBuilder.Entity<MessageChatDetail>(entity =>
            {
                entity.HasKey(e => e.MessageChatDetailsId)
                    .HasName("PK__MESSAGE___5A7341B8B4A686FD");

                entity.ToTable("MESSAGE_CHAT_DETAIL");

                entity.Property(e => e.MessageChatDetailsId).HasColumnName("message_chat_details_id");

                entity.Property(e => e.MessageChatId).HasColumnName("message_chat_id");

                entity.Property(e => e.MessageContent).HasColumnName("message_content");

                entity.Property(e => e.SenderType)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("sender_type");

                entity.Property(e => e.SentAt)
                    .HasColumnType("datetime")
                    .HasColumnName("sent_at")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.MessageChat)
                    .WithMany(p => p.MessageChatDetails)
                    .HasForeignKey(d => d.MessageChatId)
                    .HasConstraintName("FK__MESSAGE_C__sent___48CFD27E");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("NOTIFICATION");

                entity.Property(e => e.NotificationId).HasColumnName("notification_id");

                entity.Property(e => e.ContentNotification).HasColumnName("content_notification");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NotificationType).HasColumnName("notification_type");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.TitleNotification)
                    .HasMaxLength(100)
                    .HasColumnName("title_notification");
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.ToTable("RECIPE");

                entity.Property(e => e.RecipeId).HasColumnName("recipe_id");

                entity.Property(e => e.ChangeBy).HasColumnName("change_by");

                entity.Property(e => e.ChangeDate)
                    .HasColumnType("datetime")
                    .HasColumnName("change_date");

                entity.Property(e => e.CookTime).HasColumnName("cook_time");

                entity.Property(e => e.CreateBy).HasColumnName("create_by");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("create_date");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.DifficultyOfRecipe).HasColumnName("difficulty_of_recipe");

                entity.Property(e => e.FoodId).HasColumnName("food_id");

                entity.Property(e => e.Instructions).HasColumnName("instructions");

                entity.Property(e => e.PrepTime).HasColumnName("prep_time");

                entity.Property(e => e.RecipeImage).HasColumnName("recipe_image");

                entity.Property(e => e.RecipeName)
                    .HasMaxLength(100)
                    .HasColumnName("recipe_name");

                entity.Property(e => e.Servings).HasColumnName("servings");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.Recipes)
                    .HasForeignKey(d => d.CreateBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RECIPE__create_b__5CD6CB2B");

                entity.HasOne(d => d.Food)
                    .WithMany(p => p.Recipes)
                    .HasForeignKey(d => d.FoodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RECIPE__food_id__5DCAEF64");
            });

            modelBuilder.Entity<RecipeIngredient>(entity =>
            {
                entity.ToTable("RECIPE_INGREDIENT");

                entity.Property(e => e.RecipeIngredientId).HasColumnName("recipe_ingredient_id");

                entity.Property(e => e.IngredientId).HasColumnName("ingredient_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.RecipeId).HasColumnName("recipe_id");

                entity.Property(e => e.Unit)
                    .HasMaxLength(50)
                    .HasColumnName("unit");

                entity.HasOne(d => d.Ingredient)
                    .WithMany(p => p.RecipeIngredients)
                    .HasForeignKey(d => d.IngredientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RECIPE_IN__ingre__6383C8BA");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.RecipeIngredients)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RECIPE_IN__recip__628FA481");
            });

            modelBuilder.Entity<RefreshTokensMember>(entity =>
            {
                entity.ToTable("Refresh_Tokens_Member");

                entity.Property(e => e.RefreshTokensMemberId).HasColumnName("refresh_tokens_member_id");

                entity.Property(e => e.ExpiredAt).HasColumnType("datetime");

                entity.Property(e => e.IssuedAt).HasColumnType("datetime");

                entity.Property(e => e.MemberId).HasColumnName("member_id");

                entity.Property(e => e.Token).HasColumnName("token");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.RefreshTokensMembers)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Refresh_T__membe__3493CFA7");
            });

            modelBuilder.Entity<RefreshTokensStaff>(entity =>
            {
                entity.ToTable("Refresh_Tokens_Staff");

                entity.Property(e => e.RefreshTokensStaffId).HasColumnName("refresh_tokens_staff_id");

                entity.Property(e => e.ExpiredAt)
                    .HasColumnType("datetime")
                    .HasColumnName("expired_at");

                entity.Property(e => e.IsRevoked).HasColumnName("is_revoked");

                entity.Property(e => e.IsUsed).HasColumnName("is_used");

                entity.Property(e => e.IssuedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("issued_at");

                entity.Property(e => e.JwtId).HasColumnName("jwt_id");

                entity.Property(e => e.StaffId).HasColumnName("staff_id");

                entity.Property(e => e.Token).HasColumnName("token");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.RefreshTokensStaffs)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Refresh_T__staff__37703C52");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("TAG");

                entity.Property(e => e.TagId)
                    .ValueGeneratedNever()
                    .HasColumnName("tag_id");

                entity.Property(e => e.FoodTagName)
                    .HasMaxLength(50)
                    .HasColumnName("food_tag_name");
            });

            modelBuilder.Entity<WaterIntake>(entity =>
            {
                entity.HasKey(e => e.IntakeId)
                    .HasName("PK__WATER_IN__A10485F01CFBC3F5");

                entity.ToTable("WATER_INTAKE");

                entity.Property(e => e.IntakeId).HasColumnName("intake_id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.MemberId).HasColumnName("member_id");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.WaterIntakes)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__WATER_INT__membe__0D7A0286");
            });

            modelBuilder.Entity<staff>(entity =>
            {
                entity.ToTable("STAFF");

                entity.HasIndex(e => e.Email, "UQ__STAFF__AB6E616494C4033A")
                    .IsUnique();

                entity.Property(e => e.StaffId).HasColumnName("staff_id");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");

                entity.Property(e => e.Dob)
                    .HasColumnType("datetime")
                    .HasColumnName("dob");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.EndWorkingDate)
                    .HasColumnType("datetime")
                    .HasColumnName("end_working_date");

                entity.Property(e => e.FullName)
                    .HasMaxLength(255)
                    .HasColumnName("full_name");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50)
                    .HasColumnName("phone_number");

                entity.Property(e => e.Role).HasColumnName("role");

                entity.Property(e => e.Sex).HasColumnName("sex");

                entity.Property(e => e.StaffImage).HasColumnName("staff_image");

                entity.Property(e => e.StartWorkingDate)
                    .HasColumnType("datetime")
                    .HasColumnName("start_working_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
