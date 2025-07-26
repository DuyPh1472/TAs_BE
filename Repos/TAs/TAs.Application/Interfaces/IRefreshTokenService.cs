using System.Threading.Tasks;
using TAs.Domain.Entities;

namespace TAs.Application.Interfaces
{
    public interface IRefreshTokenService
    {
        Task SaveRefreshToken(User user, string refreshToken);
        Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken);
        Task MarkRefreshTokenAsUsed(RefreshToken refreshToken);
    }
} 