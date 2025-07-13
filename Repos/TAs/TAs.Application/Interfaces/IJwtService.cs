using TAs.Domain.Entities;

namespace TAs.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(User user);
        string GenerateRefreshToken();
    }
} 