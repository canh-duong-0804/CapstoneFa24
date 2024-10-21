using BusinessObject;
using Microsoft.EntityFrameworkCore;
using Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<HealthTrackingDBContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));
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
app.UseAuthorization();

app.MapControllers();

app.Run();
