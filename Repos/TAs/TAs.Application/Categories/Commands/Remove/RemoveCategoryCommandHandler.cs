using MediatR;
using TAs.Application.Categories.HandleErrors;
using TAs.Application.Interfaces;
using TAs.Domain.Result;

namespace TAs.Application.Categories.Commands.Remove
{
    public class RemoveCategoryCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<RemoveCategoryCommand, Result>
    {
        public async Task<Result> Handle(RemoveCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await unitOfWork.CategoryRepository.GetByIdAsync(request.categoryId);
            if (category is null)
                return Result.Failure(CategoryErrors.NoCategoryFound(request.categoryId));
            unitOfWork.CategoryRepository.Delete(category);
            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}