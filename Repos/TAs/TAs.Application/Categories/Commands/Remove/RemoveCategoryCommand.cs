using MediatR;
using TAs.Domain.Result;

namespace TAs.Application.Categories.Commands.Remove
{
    public class RemoveCategoryCommand(Guid CategoryId) : IRequest<Result>
    {
        public Guid categoryId = CategoryId;
    }
}