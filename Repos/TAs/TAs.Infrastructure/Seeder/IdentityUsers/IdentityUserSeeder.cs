using Microsoft.AspNetCore.Identity;
using TAs.Domain.Entities;
using TAs.Infrastructure.Persistence;
using TAs.Infrastructure.Seeder.Skills;

namespace TAs.Infrastructure.Seeder.IdentityUsers
{
    public class IdentityUserSeeder(TAsDbContext dbContext, IPasswordHasher<User> passwordHasher) : ISeeder
    {
        public async Task Seed()
        {
            if (!dbContext.Users.Any())
            {
                var users = GetUsers(passwordHasher);
                await dbContext.Users.AddRangeAsync(users);
                await dbContext.SaveChangesAsync();
            }
        }
        private IEnumerable<User> GetUsers(IPasswordHasher<User> passwordHasher)
        {
            var admin = new User
            {
                Id = Guid.Parse("ba02df20-a2ca-4f10-be79-8f5fc5bca1da"),
                UserName = "admin",
                Email = "admin@example.com",
                EmailConfirmed = true,
                NormalizedUserName = "ADMIN",
                NormalizedEmail = "ADMIN@EXAMPLE.COM"
            };
            admin.PasswordHash = passwordHasher.HashPassword(admin, "Admin@123");

            // ...existing code...
            var user1 = new User
            {
                Id = Guid.Parse("b26f222f-11c4-4453-9a1e-481fc87d0f0e"),
                UserName = "user1",
                Email = "user1@example.com",
                EmailConfirmed = true,
                NormalizedUserName = "USER1",
                NormalizedEmail = "USER1@EXAMPLE.COM"
            };
            user1.PasswordHash = passwordHasher.HashPassword(user1, "User1@123");
            // ...existing code...
            user1.PasswordHash = passwordHasher.HashPassword(user1, "User1@123");

            var user2 = new User
            {
                Id = Guid.Parse("61b0bcb6-4eaf-46cd-9970-a49d79c74fe4"),
                UserName = "user2",
                Email = "user2@example.com",
                EmailConfirmed = true,
                NormalizedUserName = "USER2",
                NormalizedEmail = "USER2@EXAMPLE.COM"
            };
            user2.PasswordHash = passwordHasher.HashPassword(user2, "User2@123");

            return new List<User> { admin, user1, user2 };
        }
    }
}