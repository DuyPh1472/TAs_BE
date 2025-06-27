using Microsoft.AspNetCore.Identity;
using TAs.Domain.Constants;
using TAs.Infrastructure.Persistence;
using TAs.Infrastructure.Seeder.Skills;

namespace TAs.Infrastructure.Seeder.IdentityRoles
{
    public class IdentityRoleSeeder(TAsDbContext dbContext) : ISeeder
    {
        public async Task Seed()
        {
            if (!dbContext.Roles.Any())
            {
                var roles = GetRoles();
                await dbContext.Roles.AddRangeAsync(roles);
                await dbContext.SaveChangesAsync();

            }
        }
        private IEnumerable<IdentityRole<Guid>> GetRoles()
        {
            List<IdentityRole<Guid>> roles = new()
            {
                new IdentityRole<Guid>
                {
                    Id = Guid.Parse("e431731f-c907-40da-89f1-67554d400cf1"),
                    Name = UserRoles.Admin,
                    NormalizedName = UserRoles.Admin.ToUpper()
                },
                new IdentityRole<Guid>
                {
                   Id = Guid.Parse("a187c881-2edb-47d4-ade4-4867c4b0689e"),
                   Name = UserRoles.User,
                   NormalizedName = UserRoles.User.ToUpper()
                }
            };
            return roles;
        }

    }
}