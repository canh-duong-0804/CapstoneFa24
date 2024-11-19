using AutoMapper;
using BusinessObject.Dto.Blog;
using BusinessObject.Dto.Blog.CreateBlog;
using BusinessObject.Dto.BodyMeasurement;
using BusinessObject.Dto.CategoryExerice;
using BusinessObject.Dto.CommunityPost;
using BusinessObject.Dto.Diet;
using BusinessObject.Dto.Exericse;
using BusinessObject.Dto.Food;
using BusinessObject.Dto.FoodDiary;
using BusinessObject.Dto.FoodMember;
using BusinessObject.Dto.Goal;
using BusinessObject.Dto.Ingredient;
using BusinessObject.Dto.Login;
using BusinessObject.Dto.MealDetailMember;
using BusinessObject.Dto.MealMember;
using BusinessObject.Dto.MealPlanDetailMember;
using BusinessObject.Dto.Member;
using BusinessObject.Dto.Recipe.CreateDTO;
using BusinessObject.Dto.Register;
using BusinessObject.Dto.Staff;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class MapperConfig
    {
        public static Mapper InitializeAutomapper()
        {

            var config = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<RegisterationRequestDTO, Member>()
                .ForMember(dest => dest.MemberId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender)).ReverseMap();
                cfg.CreateMap<RegisterationResponseDTO, Member>()
                .ReverseMap(); 
                
                cfg.CreateMap<RegisterationMobileRequestDTO, Member>()
                .ForMember(dest => dest.MemberId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender)).ReverseMap();
                cfg.CreateMap<RegisterationRequestStaffDTO, staff>()
                .ReverseMap();cfg.CreateMap<RegisterationResponseDTO, staff>()
                .ReverseMap();

                cfg.CreateMap<LoginRequestDTO, Member>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
                // .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));

                cfg.CreateMap<LoginRequestStaffDTO, staff>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
                //  .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));

                cfg.CreateMap<LoginRequestStaffDTO, staff>()
               .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
              // .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));
                cfg.CreateMap<CreateFoodRequestDTO, Food>().ReverseMap();

                cfg.CreateMap<RegisterationRequestStaffDTO, staff>()
            .ForMember(dest => dest.StaffId, opt => opt.Ignore())
            .ForMember(dest => dest.StartWorkingDate, opt => opt.MapFrom(src => DateTime.Now))
            //.ForMember(dest => dest.Status, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.StaffImage, opt => opt.MapFrom(src => src.StaffImage))
           // .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.Dob, opt => opt.MapFrom(src => src.Dob))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            ;
                cfg.CreateMap<StaffDTO, staff>()
           .ForMember(dest => dest.StaffId, opt => opt.Ignore())
           .ForMember(dest => dest.StartWorkingDate, opt => opt.MapFrom(src => DateTime.Now))
           .ForMember(dest => dest.Status, opt => opt.MapFrom(src => true))
           .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
           .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
           .ForMember(dest => dest.StaffImage, opt => opt.MapFrom(src => src.StaffImage))
           .ForMember(dest => dest.Dob, opt => opt.MapFrom(src => src.Dob))
           .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
           .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName)).ReverseMap();


                cfg.CreateMap<BlogRequestDTO, Blog>()
           .ForMember(dest => dest.BlogId, opt => opt.Ignore())
           .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => DateTime.Now))
           .ForMember(dest => dest.Status, opt => opt.MapFrom(src => true))
           .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))

           .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
           .ForMember(dest => dest.ThumbnailBlog, opt => opt.MapFrom(src => src.ThumbnailBlog))
           .ReverseMap();



                cfg.CreateMap<UpdateBlogRequestDTO, Blog>()
           .ForMember(dest => dest.BlogId, opt => opt.Ignore())
           .ForMember(dest => dest.ChangeDate, opt => opt.MapFrom(src => DateTime.Now))
           .ForMember(dest => dest.Status, opt => opt.MapFrom(src => true))
           .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))

           .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
           .ForMember(dest => dest.ThumbnailBlog, opt => opt.MapFrom(src => src.ThumbnailBlog))
           .ReverseMap();


                cfg.CreateMap<LoginResponseMemberDTO, Member>()
                .ForMember(dest => dest.MemberId, opt => opt.Ignore())

                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
               // .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.ExerciseLevel, opt => opt.MapFrom(src => src.ExerciseLevel))
                //.ForMember(dest => dest.Goal, opt => opt.MapFrom(src => src.Goal))
                .ReverseMap()
                ;
                cfg.CreateMap<MemberProfileDto, BusinessObject.Models.Member>() 
                   .ForMember(dest => dest.MemberId, opt => opt.Ignore())
                   .ReverseMap();

                cfg.CreateMap<NewCommunityPostDto, CommunityPost>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Titles)) // Mapping Titles to Title
            .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => DateTime.Now)) // Set CreateDate to now
            .ForMember(dest => dest.CreateBy, opt => opt.Ignore()) // Ignore CreateBy, should be set separately
            .ForMember(dest => dest.CommunityCategoryId, opt => opt.MapFrom(src => src.CommunityCategoryId)) // Map CommunityCategoryId
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => true)) // Set default status to true or modify as needed
            .ReverseMap();
                cfg.CreateMap<CommunityPostDto, CommunityPost>()
                .ForMember(dest => dest.CommunityPostId, opt => opt.MapFrom(src => src.PostId))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Titles)) // Mapping Titles to Title
            .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => DateTime.Now)) // Set CreateDate to now
            .ForMember(dest => dest.CreateBy, opt => opt.Ignore()) // Ignore CreateBy, should be set separately
            .ForMember(dest => dest.CommunityCategoryId, opt => opt.MapFrom(src => src.CommunityCategoryId)) // Map CommunityCategoryId
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => true)) // Set default status to true or modify as needed
            .ReverseMap();


                cfg.CreateMap<UpdatePostDTO, CommunityPost>()
                 .ForMember(dest => dest.CommunityPostId, opt => opt.MapFrom(src => src.CommunityPostId))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title)) // Map Title
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content)) // Map Content
            .ForMember(dest => dest.ChangeBy, opt => opt.Ignore()) // Map ChangeBy if provided
            .ForMember(dest => dest.ChangeDate, opt => opt.MapFrom(src => DateTime.Now));
				cfg.CreateMap<BodyMeasurementDTO, BodyMeasureChange>()
	.ForMember(dest => dest.BodyMeasureId, opt => opt.Ignore()) // Ignore ID for new measurements

	.ForMember(dest => dest.DateChange, opt => opt.MapFrom(src => src.DateChange)) // Map DateChange
	.ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight)) // Map Weight
	.ForMember(dest => dest.BodyFat, opt => opt.MapFrom(src => src.BodyFat)) // Map BodyFat
	.ForMember(dest => dest.Muscles, opt => opt.MapFrom(src => src.Muscles)) // Map Muscles
	.ForMember(dest => dest.MemberId, opt => opt.Ignore()) // Map MemberId
	.ReverseMap(); // Optional, if you want to map back


				
                cfg.CreateMap<GetAllExerciseResponseDTO, Exercise>().ReverseMap();
                cfg.CreateMap<RegisterationMobileRequestDTO, Member>().ReverseMap();
                cfg.CreateMap<DietResponseDTO, Diet>().ReverseMap();
                cfg.CreateMap<AllStaffsResponseDTO, staff>().ReverseMap();
                cfg.CreateMap<GetStaffByIdResponseDTO, staff>().ReverseMap();


                cfg.CreateMap<CreateRecipeRequestDTO, Recipe>().ReverseMap();

                cfg.CreateMap<CreateMealMemberRequestDTO, MealMember>()
               .ForMember(dest => dest.MealDate, opt => opt.Ignore())  // Thiết lập ngày trong controller
               .ForMember(dest => dest.MemberId, opt => opt.Ignore()); // Thiết lập MemberId sau

                cfg.CreateMap<CreateMealDetailMemberRequestDTO, MealMemberDetail>()
                    .ForMember(dest => dest.MealMemberId, opt => opt.Ignore()) // Thiết lập MealMemberId sau
                    .ForMember(dest => dest.MemberId, opt => opt.Ignore()); // Thiết lập MemberId sau

                cfg.CreateMap<RecipeIngredientRequestDTO, RecipeIngredient>().ReverseMap();
                cfg.CreateMap<CreateCategoryExerciseRequestDTO, ExerciseCategory>().ReverseMap();
                cfg.CreateMap<CreateIngredientRequestDTO, Ingredient>().ReverseMap();
                cfg.CreateMap<UpdateFoodRequestDTO, Food>().ReverseMap();
                cfg.CreateMap<FoodDiaryResponseDTO, FoodDiary>().ReverseMap();
                cfg.CreateMap<CreateFoodMemberRequestDTO, FoodMember>().ReverseMap();
                cfg.CreateMap<GoalResponseDTO, Goal>().ReverseMap();


                cfg.CreateMap<FoodDiaryDetail, FoodDiaryForMealResponseDTO>()
                .ForMember(dest => dest.Calories, opt => opt.MapFrom(src => src.Food.Calories))
                .ForMember(dest => dest.Carbs, opt => opt.MapFrom(src => src.Food.Carbs))
                .ForMember(dest => dest.Protein, opt => opt.MapFrom(src => src.Food.Protein))
                .ForMember(dest => dest.Fat, opt => opt.MapFrom(src => src.Food.Fat))
                .ForMember(dest => dest.Calories, opt => opt.MapFrom(src => src.Food.Calories));


                cfg.CreateMap<GoalRequestDTO, BusinessObject.Models.Goal>().ReverseMap();
                cfg.CreateMap<GetAllFoodMemberResponseDTO, FoodMember>().ReverseMap();
                cfg.CreateMap<CreateMealDetailMemberRequestDTO, BusinessObject.Models.MealsMemberDetail>().ReverseMap();
                cfg.CreateMap<MealMember, GetAllMealMemberResonseDTO>()
             .ForMember(dest => dest.NameMealPlanMember,
                        opt => opt.MapFrom(src => src.NameMealMember)).ReverseMap();

                cfg.CreateMap<AddMoreFoodToMealMemberRequestDTO, MealMemberDetail>()
           .ForMember(dest => dest.MealMemberId, opt => opt.Ignore())
           .ForMember(dest => dest.MemberId, opt => opt.Ignore());
            });

      




            var mapper = new Mapper(config);
            return mapper;
        }
}
}