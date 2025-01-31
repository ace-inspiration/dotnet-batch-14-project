using Microsoft.EntityFrameworkCore;
using TravelAgency.Database.AppDbContextModels;
using TravelAgency.Domain.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    //options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
	//options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnectionpkk"));
	options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnectionKZT"));
}, 
ServiceLifetime.Transient, 
ServiceLifetime.Transient);

builder.AddFeatureServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
