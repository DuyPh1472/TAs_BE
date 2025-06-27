using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TAs.Domain.Entities;

namespace TAs.Infrastructure.Persistence
{
    public class TAsDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public TAsDbContext(DbContextOptions<TAsDbContext> options)
            : base(options)
        {
        }

        internal DbSet<Skill> Skills { get; set; }
        
    }
}