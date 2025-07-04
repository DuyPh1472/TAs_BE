using MediatR;
using TAs.Application.Categories.DTOs.Retrieval;
using TAs.Application.Categories.HandleErrors;
using TAs.Application.Interfaces;
using TAs.Application.Lessons.DTOs;
using TAs.Domain.Result;

namespace TAs.Application.Categories.Queries.GetByTitles
{
    public class GetCategoryByTitleQueryHandler(IUnitOfWork unitOfWork) :
     IRequestHandler<GetCategoryByTitleQuery, Result<GetCategoryByTitleDTO>>
    {
        public async Task<Result<GetCategoryByTitleDTO>>
        Handle(GetCategoryByTitleQuery request, CancellationToken cancellationToken)
        {
            var category = await unitOfWork
            .CategoryRepository
            .GetCategoryByTitle(request.title);
            if (category is null)
                return Result<GetCategoryByTitleDTO>
                .Failure(CategoryErrors.NoTitleFound(request.title));
            return Result<GetCategoryByTitleDTO>.Success(
               new GetCategoryByTitleDTO
               {
                   Accent = category.Accent,
                   Description = category.Description,
                   Difficulty = category.Difficult,
                   Duration = category.Duration,
                   Title = category.Title,
                   Lessons = [.. category.Lessons.Select(l => new LessonDTO{
                         LessonId = l.Id,
                         Accent = l.Accent,
                         Description = l.Description,
                         Duration = l.Duration,
                         Level = l.Level,
                         Sentences = l.Sentences,
                         Title = l.Title,
                         Topics = l.Topics
                   })]
               }
            );
        }
    }
}