using Microsoft.AspNetCore.Identity;
using TAs.Domain.Constants;
using TAs.Infrastructure.Persistence;
using TAs.Infrastructure.Seeder.Skills;

namespace TAs.Infrastructure.Seeder.IdentityUsersRoles
{
    public class IdentityUserRoleSeeder(TAsDbContext dbContext) : ISeeder
    {
        public async Task Seed()
        {
            if (!dbContext.UserRoles.Any())
            {
                var userRoles = GetUserRoles();
                await dbContext.UserRoles.AddRangeAsync(userRoles);
                await dbContext.SaveChangesAsync();
            }
        }

        private IEnumerable<IdentityUserRole<Guid>> GetUserRoles()
        {
            // Lấy Id của user và role đã seed trước đó
            var adminUser = dbContext.Users.FirstOrDefault(u => u.UserName == "admin");
            var adminRole = dbContext.Roles.FirstOrDefault(r => r.Name == UserRoles.Admin);
            var user1 = dbContext.Users.FirstOrDefault(u => u.UserName == "user1");
            var userRole = dbContext.Roles.FirstOrDefault(r => r.Name == UserRoles.User);
            var user2 = dbContext.Users.FirstOrDefault(u => u.UserName == "user2");
            var userRole_2 = dbContext.Roles.FirstOrDefault(r => r.Name == UserRoles.User);
            var list = new List<IdentityUserRole<Guid>>();

            if (adminUser != null && adminRole != null)
                list.Add(new IdentityUserRole<Guid> { UserId = adminUser.Id, RoleId = adminRole.Id });

            if (user1 != null && userRole != null)
                list.Add(new IdentityUserRole<Guid> { UserId = user1.Id, RoleId = userRole.Id });

            if (user2 != null && userRole_2 != null)
                list.Add(new IdentityUserRole<Guid> { UserId = user2.Id, RoleId = userRole_2.Id });

            return list;
        }
    }
}