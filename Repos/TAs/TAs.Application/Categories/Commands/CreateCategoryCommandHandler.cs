using MediatR;
using TAs.Application.Interfaces;
using TAs.Application.Users;
using TAs.Domain.Entities;
using TAs.Domain.Result;

namespace TAs.Application.Categories.Commands
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
                Accent = request.Accent,
                Description = request.Description,
                Difficult = request.Difficult,
                Duration = request.Duration,
                Title = request.Title,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = user!.Id
            };
            unitOfWork.CategoryRepository.Add(category);
            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}