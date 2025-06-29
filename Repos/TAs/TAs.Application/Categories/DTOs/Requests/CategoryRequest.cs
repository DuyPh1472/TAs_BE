namespace TAs.Application.Categories.DTOs.Requests
{
    public record CategoryRequest(
        string Title,
        string Description,
        string Difficult,
        string Accent,
        float Duration
    );
}