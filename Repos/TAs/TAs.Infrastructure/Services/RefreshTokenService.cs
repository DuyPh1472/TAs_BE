using System.Threading.Tasks;
using TAs.Application.Interfaces;
using TAs.Domain.Entities;
using TAs.Infrastructure.Persistence;

namespace TAs.Infrastructure.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly TAsDbContext _context;
        public RefreshTokenService(TAsDbContext context)
        {
            _context = context;
        }

        public async Task SaveRefreshToken(User user, string refreshToken)
        {
            var token = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                Expires = System.DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                IsUsed = false,
                CreatedAt = System.DateTime.UtcNow
            };
            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();
        }
    }
} 