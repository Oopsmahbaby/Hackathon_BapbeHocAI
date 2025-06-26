using Hackathon_API.Extensions;
using Hackathon_API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
							.AddJsonOptions(options =>
							{
								options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
							});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplicationDatabase(builder.Configuration);

// Swagger Setup
builder.Services.AddSwaggerDocumentation();

// Jwt Setup
builder.Services.AddJwtAuthentication(builder.Configuration);

// Cors Setup
builder.Services.AddCorsPolicy();
builder.Services.AddAuthorization();

// Application Settings
builder.Services.AddApplicationSettings(builder.Configuration);

// Global Middleware
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
