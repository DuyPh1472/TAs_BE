using TAs.Infrastructure.Extensions;
using TAs.Application.Extensions;
using TAs.APi.Middlewares;
using TAs.APi.Extensions;
using TAs.Infrastructure.Seeder.Skills;
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
// Configure the HTTP request pipeline.

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
   {
       c.SwaggerEndpoint("/swagger/v1/swagger.json", "TAs API v1");
       c.RoutePrefix = string.Empty; // <--- Đây là điểm quan trọng

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

// Add CORS middleware
app.UseCors("AllowAll");

// app.MapGroup("/api/identity/").MapIdentityApi<User>();

app.UseAuthorization();

app.MapControllers();

app.Run();
