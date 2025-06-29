using MediatR;
using TAs.Application.Categories.HandleErrors;
using TAs.Application.Interfaces;
using TAs.Application.Users;
using TAs.Domain.Result;

namespace TAs.Application.Categories.Commands.Update
{
    public class UpdateCategoryCommandHandler(IUnitOfWork unitOfWork,
    IUserContext userContext)
    : IRequestHandler<UpdateCategoryCommand, Result>
    {
        public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser();
            var category = await unitOfWork.CategoryRepository.GetByIdAsync(request.categoryId);
            if (category is null)
                return Result.Failure(CategoryErrors.NoCategoryFound(request.categoryId));
            category.Accent = request.categoryRequest.Accent;
            category.Description = request.categoryRequest.Description;
            category.Difficult = request.categoryRequest.Difficult;
            category.Title = request.categoryRequest.Title;
            category.Duration = request.categoryRequest.Duration;
            category.UpdatedBy = currentUser!.Id;
            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}