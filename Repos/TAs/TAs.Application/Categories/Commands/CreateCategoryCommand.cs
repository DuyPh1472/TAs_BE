using MediatR;
using TAs.Domain.Result;

namespace TAs.Application.Categories.Commands
{
    public class CreateCategoryCommand : IRequest<Result>
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Difficult { get; set; } = string.Empty;
        public string Accent { get; set; } = string.Empty;
        public float Duration { get; set; }
    }
}