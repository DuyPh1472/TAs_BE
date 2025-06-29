using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TAs.Application.Interfaces;
using TAs.Domain.Entities;
using TAs.Domain.IGenericRepo;
using TAs.Domain.Repositories;
using TAs.Infrastructure.Authorization;
using TAs.Infrastructure.Persistence;
using TAs.Infrastructure.Persistence.GenericRepo;
using TAs.Infrastructure.Repositories;
using TAs.Infrastructure.Seeder.Categories;
using TAs.Infrastructure.Seeder.CategoryLessons;
using TAs.Infrastructure.Seeder.IdentityRoles;
using TAs.Infrastructure.Seeder.IdentityUsers;
using TAs.Infrastructure.Seeder.IdentityUsersRoles;
using TAs.Infrastructure.Seeder.Lessons;
using TAs.Infrastructure.Seeder.Skills;
using TAs.Infrastructure.UOW;
namespace TAs.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<TAsDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<ISeeder, SkillSeeder>();
        services.AddScoped<ISeeder, IdentityUserSeeder>();
        services.AddScoped<ISeeder, IdentityRoleSeeder>();
        services.AddScoped<ISeeder, IdentityUserRoleSeeder>();
        services.AddScoped<ISeeder, CategorySeeder>();
        services.AddScoped<ISeeder, LessonSeeder>();
        services.AddScoped<ISeeder, CategoryLessonSeeder>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddIdentityApiEndpoints<User>()
         .AddRoles<IdentityRole<Guid>>()
         .AddClaimsPrincipalFactory<TAsUserClaimPrincipalFactory>()
         .AddEntityFrameworkStores<TAsDbContext>();
        services.AddScoped<ISkillRepository, SkillRepository>();
        services.AddScoped<IAchievementRepository, AchievementRepository>();
        services.AddScoped<CategoryLessonRepository, CategoryLessonRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ILessonRepository, LessonRepository>();
        services.AddScoped<IProgressRepository, ProgressRepository>();
        services.AddScoped<ISkillLessonRepository, SkillLessonRepository>();
        services.AddScoped<IUserAchievementRepository, UserAchievementRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    }
}