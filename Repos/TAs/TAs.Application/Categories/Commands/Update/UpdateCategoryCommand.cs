using MediatR;
using TAs.Application.Categories.DTOs.Requests;
using TAs.Domain.Result;

namespace TAs.Application.Categories.Commands.Update
{
    public class UpdateCategoryCommand(Guid Id,
    CategoryRequest CategoryRequest) : IRequest<Result>
    {
        public Guid categoryId = Id;
        public CategoryRequest categoryRequest = CategoryRequest;
    }
}