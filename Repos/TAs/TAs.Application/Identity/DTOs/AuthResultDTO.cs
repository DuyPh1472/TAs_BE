namespace TAs.Application.Identity.DTOs
{
    public class AuthResultDTO
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
} 