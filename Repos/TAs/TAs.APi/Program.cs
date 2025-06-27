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
var seeder = scope.ServiceProvider.GetRequiredService<ISkillSeeder>();
await seeder.Seed();
app.UseMiddleware<ErrorHandlingMiddle>();
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
// app.MapGroup("/api/identity/").MapIdentityApi<User>();

app.UseAuthorization();

app.MapControllers();

app.Run();
