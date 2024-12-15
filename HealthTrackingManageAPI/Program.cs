
using BusinessObject.Models;
using FluentValidation.AspNetCore;
using FluentValidation;
using HealthTrackingManageAPI;
using HealthTrackingManageAPI.NewFolder.EsmsHelper;
using HealthTrackingManageAPI.NewFolder.Image;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository.IRepo;
using Repository.NewFolder;
using Repository.Repo;
using System.Text;
using Twilio;
using Twilio.Clients;
using HealthTrackingManageAPI.NewFolder.Chat;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<HealthTrackingDBContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));
builder.Services.Configure<AppSettingsKey>(builder.Configuration.GetSection("ApiSettings"));
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<IFoodRepository, FoodRepository>();
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
builder.Services.AddScoped<ICommunityPostRepository, CommunityPostRepository>();
builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();
builder.Services.AddScoped<IMainDashBoardRepository, MainDashBoardRepository>();
builder.Services.AddScoped<IExerciseCategoryRepository, ExerciseCategoryRepository>();
builder.Services.AddScoped<IMealMemberRepository, MealMemberRepository>();
builder.Services.AddScoped<IExecriseDiaryDetailRepository, ExecriseDiaryDetailRepository>();
builder.Services.AddScoped<ICommunityCategoryRepo, CommunityCategoryRepository>();
builder.Services.AddScoped<IBodyMesurementRepository, BodyMeasurementRepository>();
builder.Services.AddScoped<IExeriseDiaryRepository, ExecriseDiaryRepository>();
builder.Services.AddScoped<ICompanyInfoRepository, CompanyInfoRepository>();
builder.Services.AddScoped<IFoodDiaryRepository, FoodDiaryRepository>();
builder.Services.AddScoped<IExecrisePlanTrainerRepository, ExecrisePlanTrainerRepository>();
builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();
builder.Services.AddScoped<IMainDashBoardRepository, MainDashBoardRepository>();
builder.Services.AddScoped<IExerciseCategoryRepository, ExerciseCategoryRepository>();
builder.Services.AddScoped<IGoalRepository, GoalRepository>();
builder.Services.AddScoped<IMealMemberDetailsRepository, MealMemberDetailsRepository>();
builder.Services.AddScoped<IFoodMemberRepository, FoodMemberRepository>();
builder.Services.AddScoped<IMealPlanRepository, MealPlanRepository>();
builder.Services.AddScoped<IWaterLogRepository, WaterLogRepository>();
builder.Services.AddScoped<IExecrisePlanRepository, ExecrisePlanRepository>();
builder.Services.AddScoped<IMealPlanTrainnerRepository, MealPlanTrainnerRepository>();
builder.Services.AddScoped<IExerciseTrainerRepository, ExerciseTrainerRepository>();

builder.Services.AddScoped<IChatMemberRepository, ChatMemberRepository>();
builder.Services.AddScoped<IAdminChatRepository, AdminChatRepository>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IValidator<ImageUploadDto>, ImageUploadValidator>();
builder.Services.AddScoped<CloudinaryService>();
builder.Services.AddScoped<INutrientRepository, NutrientRepository>();
builder.Services.AddScoped<ICaloriesRepository, CaloriesRepository>();
builder.Services.AddScoped<IMacroRepository, MacroRepository>();
builder.Services.AddScoped<IMainDashboardAdminManageRepository, MainDashboardAdminManageRepository>();
builder.Services.AddScoped<IMainDashboardTrainerManageRepository, MainDashboardTrainerManageRepository>();

builder.Services.AddScoped<SpeedSMSService>();

//builder.Services.AddHttpClient<ITwilioRestClient, TwilloClient>();
builder.Services.Configure<SMSSetting>(builder.Configuration.GetSection("SMSSettingTwilio"));
builder.Services.AddTransient<ISMSService, SMSService>();

builder.Services.AddDistributedMemoryCache(); // Sử dụng bộ nhớ trong để lưu session

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian timeout của session
    options.Cookie.HttpOnly = true; // Chỉ truy cập qua HTTP
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllers();
builder.Services.AddSignalR();
var secretKey = builder.Configuration["ApiSettings:SecretKey"];
var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "jwtToken_AuthAPI",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        In = ParameterLocation.Header,
        Description = "Enter token"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            }, new string [] { }
        }
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(opt =>
	{
		opt.TokenValidationParameters = new TokenValidationParameters
		{
			//tự cấp token
			ValidateIssuer = false,
			ValidateAudience = false,

			//ký vào token
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

			ClockSkew = TimeSpan.Zero
		};
	});
builder.Services.AddCors(options =>
{
	options.AddPolicy("MyCorsPolicy", policy =>
	{
		policy.AllowAnyOrigin()   // Allow any origin
			  .AllowAnyHeader()   // Allow any header
			  .AllowAnyMethod();  // Allow any HTTP method (GET, POST, etc.)
	});
});
builder.Services.AddScoped<IUserRepository, UserRepository>();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();


app.UseCors("MyCorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ChatHub>("/chathub"); // Map the ChatHub
});
app.UseSession();

app.MapControllers();

app.Run();
