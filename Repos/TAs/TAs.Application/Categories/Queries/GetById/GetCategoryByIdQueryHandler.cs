using MediatR;
using TAs.Application.Categories.DTOs.Retrieval;
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
            // var dto = new GetCategoryIdDTO
            // {
            //     Accent = category.Accent,
            //     Description = category.Description,
            //     Difficult = category.Difficult,
            //     Duration = category.Duration,
            //     Title = category.Title,
            //     Lessons = category.CategoryLessons.Select(c => new LessonDTO
            //     {
            //         Accent = c.Lesson.Accent,
            //         Description = c.Lesson.Description,
            //         Duration = c.Lesson.Duration,
            //         LessonId = c.Lesson.LessonId,
            //         Level = c.Lesson.Level,
            //         Sentences = c.Lesson.Sentences,
            //         Title = c.Lesson.Title,
            //         Topics = c.Lesson.Topics,
            //     }).ToList()
            // };
            // return Result<GetCategoryIdDTO>.Success(dto);

            return Result<GetCategoryIdDTO>
            .Success(GetCategoryIdDTO
            .FromEntity(category, category.CategoryLessons
            .Select(c => c.Lesson)
            .ToList()));
        }
    }
}