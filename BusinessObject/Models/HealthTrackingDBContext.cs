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
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<CommunityPost> CommunityPosts { get; set; } = null!;
        public virtual DbSet<CompanyInfo> CompanyInfos { get; set; } = null!;
        public virtual DbSet<Diet> Diets { get; set; } = null!;
        public virtual DbSet<Disease> Diseases { get; set; } = null!;
        public virtual DbSet<Exercise> Exercises { get; set; } = null!;
        public virtual DbSet<ExerciseCategory> ExerciseCategories { get; set; } = null!;
        public virtual DbSet<ExerciseDiary> ExerciseDiaries { get; set; } = null!;
        public virtual DbSet<ExercisePlan> ExercisePlans { get; set; } = null!;
        public virtual DbSet<ExercisePlanDetail> ExercisePlanDetails { get; set; } = null!;
        public virtual DbSet<Faq> Faqs { get; set; } = null!;
        public virtual DbSet<Food> Foods { get; set; } = null!;
        public virtual DbSet<FoodDiary> FoodDiaries { get; set; } = null!;
        public virtual DbSet<FoodDiaryDetail> FoodDiaryDetails { get; set; } = null!;
        public virtual DbSet<Goal> Goals { get; set; } = null!;
        public virtual DbSet<MealPlan> MealPlans { get; set; } = null!;
        public virtual DbSet<MealPlanDetail> MealPlanDetails { get; set; } = null!;
        public virtual DbSet<Member> Members { get; set; } = null!;
        public virtual DbSet<MemberDisease> MemberDiseases { get; set; } = null!;
        public virtual DbSet<MessageChat> MessageChats { get; set; } = null!;
        public virtual DbSet<Recipe> Recipes { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;
        public virtual DbSet<WaterIntake> WaterIntakes { get; set; } = null!;
        public virtual DbSet<staff> staff { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=DESKTOP-5DR1P1T;database=HealthTrackingDB;uid=sa;pwd=123;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>(entity =>
            {
                entity.ToTable("BLOG");

                entity.Property(e => e.BlogId).HasColumnName("blog_id");

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

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.Blogs)
                    .HasForeignKey(d => d.CreateBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BLOG__create_by__5BE2A6F2");
            });

            modelBuilder.Entity<BodyMeasureChange>(entity =>
            {
                entity.HasKey(e => e.BodyMeasureId)
                    .HasName("PK__BODY_MEA__3FCFA33B5746A884");

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
                    .HasConstraintName("FK__BODY_MEAS__membe__4F7CD00D");
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
                    .HasConstraintName("FK__COMMENT__create___66603565");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__COMMENT__post_id__656C112C");
            });

            modelBuilder.Entity<CommunityPost>(entity =>
            {
                entity.ToTable("COMMUNITY_POST");

                entity.Property(e => e.CommunityPostId).HasColumnName("community_post_id");

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

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .HasColumnName("title");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.CommunityPosts)
                    .HasForeignKey(d => d.CreateBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__COMMUNITY__creat__60A75C0F");
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

                entity.Property(e => e.CaloriesPerHour).HasColumnName("calories_per_hour");

                entity.Property(e => e.ChangeBy).HasColumnName("change_by");

                entity.Property(e => e.ChangeDate)
                    .HasColumnType("datetime")
                    .HasColumnName("change_date");

                entity.Property(e => e.CreateBy).HasColumnName("create_by");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("create_date");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.ExerciseCategoryId).HasColumnName("exercise_category_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.Exercises)
                    .HasForeignKey(d => d.CreateBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EXERCISE__create__5441852A");

                entity.HasOne(d => d.ExerciseCategory)
                    .WithMany(p => p.Exercises)
                    .HasForeignKey(d => d.ExerciseCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EXERCISE__exerci__5535A963");
            });

            modelBuilder.Entity<ExerciseCategory>(entity =>
            {
                entity.ToTable("EXERCISE_CATEGORIES");

                entity.Property(e => e.ExerciseCategoryId).HasColumnName("exercise_category_id");

                entity.Property(e => e.ExerciseCategoryName)
                    .HasMaxLength(50)
                    .HasColumnName("exercise_category_name");
            });

            modelBuilder.Entity<ExerciseDiary>(entity =>
            {
                entity.ToTable("EXERCISE_DIARY");

                entity.Property(e => e.ExerciseDiaryId).HasColumnName("exercise_diary_id");

                entity.Property(e => e.CaloriesBurned).HasColumnName("calories_burned");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.Duration).HasColumnName("duration");

                entity.Property(e => e.ExerciseId).HasColumnName("exercise_id");

                entity.Property(e => e.ExercisePlanId).HasColumnName("exercise_plan_id");

                entity.Property(e => e.MemberId).HasColumnName("member_id");

                entity.HasOne(d => d.Exercise)
                    .WithMany(p => p.ExerciseDiaries)
                    .HasForeignKey(d => d.ExerciseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EXERCISE___exerc__05D8E0BE");

                entity.HasOne(d => d.ExercisePlan)
                    .WithMany(p => p.ExerciseDiaries)
                    .HasForeignKey(d => d.ExercisePlanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EXERCISE___exerc__03F0984C");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.ExerciseDiaries)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EXERCISE___membe__04E4BC85");
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

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.TotalCaloriesBurned).HasColumnName("total_calories_burned");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.ExercisePlans)
                    .HasForeignKey(d => d.CreateBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EXERCISE___creat__7D439ABD");
            });

            modelBuilder.Entity<ExercisePlanDetail>(entity =>
            {
                entity.ToTable("EXERCISE_PLAN_DETAILS");

                entity.Property(e => e.ExercisePlanDetailId).HasColumnName("exercise_plan_detail_id");

                entity.Property(e => e.Duration).HasColumnName("duration");

                entity.Property(e => e.ExerciseId).HasColumnName("exercise_id");

                entity.Property(e => e.ExercisePlanId).HasColumnName("exercise_plan_id");

                entity.HasOne(d => d.Exercise)
                    .WithMany(p => p.ExercisePlanDetails)
                    .HasForeignKey(d => d.ExerciseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EXERCISE___exerc__01142BA1");

                entity.HasOne(d => d.ExercisePlan)
                    .WithMany(p => p.ExercisePlanDetails)
                    .HasForeignKey(d => d.ExercisePlanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EXERCISE___exerc__00200768");
            });

            modelBuilder.Entity<Faq>(entity =>
            {
                entity.HasKey(e => e.QuestionId)
                    .HasName("PK__FAQ__2EC2154970461874");

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

                entity.Property(e => e.Diet)
                    .HasMaxLength(50)
                    .HasColumnName("diet");

                entity.Property(e => e.Fat).HasColumnName("fat");

                entity.Property(e => e.FoodImage)
                    .HasMaxLength(100)
                    .HasColumnName("food_image");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Portion)
                    .HasMaxLength(50)
                    .HasColumnName("portion");

                entity.Property(e => e.Protein).HasColumnName("protein");

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
                    .HasConstraintName("FK__FOOD__create_by__3F466844");

                entity.HasMany(d => d.Tags)
                    .WithMany(p => p.Foods)
                    .UsingEntity<Dictionary<string, object>>(
                        "FoodTag",
                        l => l.HasOne<Tag>().WithMany().HasForeignKey("TagId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__FOOD_TAG__tag_id__44FF419A"),
                        r => r.HasOne<Food>().WithMany().HasForeignKey("FoodId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__FOOD_TAG__food_i__440B1D61"),
                        j =>
                        {
                            j.HasKey("FoodId", "TagId").HasName("PK__FOOD_TAG__5B6527F39BD304C6");

                            j.ToTable("FOOD_TAG");

                            j.IndexerProperty<int>("FoodId").HasColumnName("food_id");

                            j.IndexerProperty<int>("TagId").HasColumnName("tag_id");
                        });
            });

            modelBuilder.Entity<FoodDiary>(entity =>
            {
                entity.HasKey(e => e.DiaryId)
                    .HasName("PK__FOOD_DIA__339232C84BE920A0");

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
                    .HasConstraintName("FK__FOOD_DIAR__meal___76969D2E");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.FoodDiaries)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FOOD_DIAR__membe__75A278F5");
            });

            modelBuilder.Entity<FoodDiaryDetail>(entity =>
            {
                entity.HasKey(e => e.DiaryDetailId)
                    .HasName("PK__FOOD_DIA__2B203A1FB79F7A34");

                entity.ToTable("FOOD_DIARY_DETAIL");

                entity.Property(e => e.DiaryDetailId).HasColumnName("diary_detail_id");

                entity.Property(e => e.DiaryId).HasColumnName("diary_id");

                entity.Property(e => e.FoodId).HasColumnName("food_id");

                entity.Property(e => e.MealType).HasColumnName("meal_type");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.StatusFoodDiary).HasColumnName("status_food_diary");

                entity.HasOne(d => d.Diary)
                    .WithMany(p => p.FoodDiaryDetails)
                    .HasForeignKey(d => d.DiaryId)
                    .HasConstraintName("FK__FOOD_DIAR__diary__797309D9");

                entity.HasOne(d => d.Food)
                    .WithMany(p => p.FoodDiaryDetails)
                    .HasForeignKey(d => d.FoodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FOOD_DIAR__food___7A672E12");
            });

            modelBuilder.Entity<Goal>(entity =>
            {
                entity.ToTable("GOAL");

                entity.Property(e => e.GoalId).HasColumnName("goal_id");

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
                    .HasConstraintName("FK__GOAL__member_id__4CA06362");
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

                entity.Property(e => e.MealPlanImage)
                    .HasMaxLength(100)
                    .HasColumnName("meal_plan_image");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.TotalCalories).HasColumnName("total_calories");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.MealPlans)
                    .HasForeignKey(d => d.CreateBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MEAL_PLAN__creat__6D0D32F4");

                entity.HasOne(d => d.Diet)
                    .WithMany(p => p.MealPlans)
                    .HasForeignKey(d => d.DietId)
                    .HasConstraintName("FK__MEAL_PLAN__diet___6E01572D");
            });

            modelBuilder.Entity<MealPlanDetail>(entity =>
            {
                entity.ToTable("MEAL_PLAN_DETAILS");

                entity.Property(e => e.MealPlanDetailId).HasColumnName("meal_plan_detail_id");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .HasColumnName("description");

                entity.Property(e => e.FoodId).HasColumnName("food_id");

                entity.Property(e => e.MealDate)
                    .HasColumnType("date")
                    .HasColumnName("meal_date");

                entity.Property(e => e.MealPlanId).HasColumnName("meal_plan_id");

                entity.Property(e => e.MealType)
                    .HasMaxLength(50)
                    .HasColumnName("meal_type");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Food)
                    .WithMany(p => p.MealPlanDetails)
                    .HasForeignKey(d => d.FoodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MEAL_PLAN__food___72C60C4A");

                entity.HasOne(d => d.MealPlan)
                    .WithMany(p => p.MealPlanDetails)
                    .HasForeignKey(d => d.MealPlanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MEAL_PLAN__meal___71D1E811");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.ToTable("MEMBER");

                entity.HasIndex(e => e.Email, "UQ__MEMBER__AB6E6164E56CA26C")
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

                entity.Property(e => e.Goal)
                    .HasMaxLength(100)
                    .HasColumnName("goal");

                entity.Property(e => e.Height).HasColumnName("height");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password");

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

                entity.Property(e => e.Weight).HasColumnName("weight");

                entity.HasOne(d => d.Diet)
                    .WithMany(p => p.Members)
                    .HasForeignKey(d => d.DietId)
                    .HasConstraintName("FK__MEMBER__diet_id__300424B4");
            });

            modelBuilder.Entity<MemberDisease>(entity =>
            {
                entity.HasKey(e => e.IdMemberDisease)
                    .HasName("PK__MEMBER_D__9AA485E4FFA611DC");

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
                    .HasConstraintName("FK__MEMBER_DI__disea__3C69FB99");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.MemberDiseases)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK__MEMBER_DI__membe__3B75D760");
            });

            modelBuilder.Entity<MessageChat>(entity =>
            {
                entity.ToTable("MESSAGE_CHAT");

                entity.Property(e => e.MessageChatId).HasColumnName("message_chat_id");

                entity.Property(e => e.MemberId).HasColumnName("member_id");

                entity.Property(e => e.MessageContent).HasColumnName("message_content");

                entity.Property(e => e.SenderType)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("sender_type");

                entity.Property(e => e.SentAt)
                    .HasColumnType("datetime")
                    .HasColumnName("sent_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.StaffId).HasColumnName("staff_id");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.MessageChats)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK__MESSAGE_C__membe__36B12243");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.MessageChats)
                    .HasForeignKey(d => d.StaffId)
                    .HasConstraintName("FK__MESSAGE_C__sent___35BCFE0A");
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

                entity.Property(e => e.FoodId).HasColumnName("food_id");

                entity.Property(e => e.Instructions).HasColumnName("instructions");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.PrepTime).HasColumnName("prep_time");

                entity.Property(e => e.Servings).HasColumnName("servings");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.Recipes)
                    .HasForeignKey(d => d.CreateBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RECIPE__create_b__48CFD27E");

                entity.HasOne(d => d.Food)
                    .WithMany(p => p.Recipes)
                    .HasForeignKey(d => d.FoodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RECIPE__food_id__49C3F6B7");
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
                    .HasName("PK__WATER_IN__A10485F0CEA597CE");

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
                    .HasConstraintName("FK__WATER_INT__membe__693CA210");
            });

            modelBuilder.Entity<staff>(entity =>
            {
                entity.ToTable("STAFF");

                entity.HasIndex(e => e.Email, "UQ__STAFF__AB6E6164E4D276EA")
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

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50)
                    .HasColumnName("phone_number");

                entity.Property(e => e.Role).HasColumnName("role");

                entity.Property(e => e.Sex).HasColumnName("sex");

                entity.Property(e => e.StaffImage)
                    .HasMaxLength(255)
                    .HasColumnName("staff_image");

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
