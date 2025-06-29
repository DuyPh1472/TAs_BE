using MediatR;
using TAs.Application.Interfaces;
using TAs.Application.Users;
using TAs.Domain.Entities;
using TAs.Domain.Result;

namespace TAs.Application.Categories.Commands.Create
{
    public class CreateCategoryCommandHandler(IUnitOfWork unitOfWork,
    IUserContext userContext)
    : IRequestHandler<CreateCategoryCommand, Result>
    {
        public async Task<Result> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var user = userContext.GetCurrentUser();
            var category = new Category
            {
                Accent = request.categoryRequest?.Accent ?? string.Empty,
                Description = request.categoryRequest?.Description ?? string.Empty,
                Difficult = request.categoryRequest?.Difficult ?? string.Empty,
                Duration = request.categoryRequest?.Duration ?? 0,
                Title = request.categoryRequest?.Title ?? string.Empty,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = user!.Id
            };
            unitOfWork.CategoryRepository.Add(category);
            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}