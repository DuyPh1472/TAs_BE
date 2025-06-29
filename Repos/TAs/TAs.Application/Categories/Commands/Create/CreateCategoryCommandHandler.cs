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
                Accent = request.categoryRequest.Accent,
                Description = request.categoryRequest.Description,
                Difficult = request.categoryRequest.Difficult,
                Duration = request.categoryRequest.Duration,
                Title = request.categoryRequest.Title,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = user!.Id
            };
            unitOfWork.CategoryRepository.Add(category);
            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}