using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TAs.Domain.Entities;
using TAs.Domain.IGenericRepo;
using TAs.Domain.Repositories;
using TAs.Infrastructure.Persistence;
using TAs.Infrastructure.Persistence.GenericRepo;
using TAs.Infrastructure.Repositories;
using TAs.Infrastructure.Seeder.IdentityRoles;
using TAs.Infrastructure.Seeder.IdentityUsers;
using TAs.Infrastructure.Seeder.IdentityUsersRoles;
using TAs.Infrastructure.Seeder.Skills;
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
        services.AddIdentityApiEndpoints<User>()
        .AddRoles<IdentityRole<Guid>>()
        .AddEntityFrameworkStores<TAsDbContext>();
        services.AddScoped<ISkillRepository, SkillRepository>();
        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    }
}