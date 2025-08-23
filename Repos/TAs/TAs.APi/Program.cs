using TAs.Infrastructure.Extensions;
using TAs.Application.Extensions;
using TAs.APi.Extensions;
using TAs.Infrastructure.Seeder.Skills;
using TAs.APi.Middlewares;
using TAs.APi.Multiplayer;
using TAs.APi.Services;
using TAs.Application.GameRooms;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.AddPresentation();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddScoped<ErrorHandlingMiddle>();
builder.Services.AddProblemDetails();
// Add exception handlers
builder.Services.AddExceptionHandler<ErrorHandlingMiddle>();

builder.Services.AddSignalR();
builder.Services.AddScoped<IGameRoomEventService, GameRoomEventService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configure the HTTP request pipeline.

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
   {
       c.SwaggerEndpoint("/swagger/v1/swagger.json", "TAs API v1");
       c.RoutePrefix = string.Empty;

   });

}

var scope = app.Services.CreateScope();
var seeders = scope.ServiceProvider.GetServices<ISeeder>();
foreach (var seeder in seeders)
{
    await seeder.Seed();
}
// add problem details

app.UseHttpsRedirection();

// Sử dụng CORS cho frontend
app.UseCors("AllowFrontend");

// app.MapGroup("/api/identity/").MapIdentityApi<User>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<GameRoomHub>("/hubs/gameRoom");

app.Run();
