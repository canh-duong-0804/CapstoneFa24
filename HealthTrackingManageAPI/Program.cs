
using BusinessObject.Models;
using HealthTrackingManageAPI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<HealthTrackingDBContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));
builder.Services.Configure<AppSettingsKey>(builder.Configuration.GetSection("ApiSettings"));

var secretKey = builder.Configuration["ApiSettings:SecretKey"];
var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);


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
app.UseCors("MyCorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
