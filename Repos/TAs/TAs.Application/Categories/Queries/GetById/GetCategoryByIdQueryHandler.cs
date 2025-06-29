using MediatR;
using TAs.Application.Categories.DTOs;
using TAs.Application.Categories.HandleErrors;
using TAs.Application.Interfaces;
using TAs.Domain.Result;

namespace TAs.Application.Categories.Queries.GetById
{
    public class GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork) :
     IRequestHandler<GetCategoryByIdQuery, Result<GetCategoryIdDTO>>
    {
        public async Task<Result<GetCategoryIdDTO>> Handle
        (GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await unitOfWork.CategoryRepository.GetCategoryByIdAsync(request.Id);
            if (category is null)
                return Result<GetCategoryIdDTO>
                .Failure(CategoryErrors
                .NoCategoryFound(request.Id));
            return Result<GetCategoryIdDTO>
                .Success(GetCategoryIdDTO
                .FromEntity(
                category, category
                .CategoryLessons
                .Select(c => c.Lesson)
                .ToList()));
        }
    }
}