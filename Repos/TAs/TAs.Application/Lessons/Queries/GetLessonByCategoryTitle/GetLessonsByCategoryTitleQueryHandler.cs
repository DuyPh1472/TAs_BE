using MediatR;
using TAs.Application.Categories.HandleErrors;
using TAs.Application.Interfaces;
using TAs.Application.Lessons.DTOs;
using TAs.Domain.Result;

namespace TAs.Application.Lessons.Queries.GetLessonByCategoryTitle
{
    public class GetLessonsByCategoryTitleQueryHandler(IUnitOfWork unitOfWork) :
     IRequestHandler<GetLessonsByCategoryTitleQuery, Result<List<GetAllLessonDTO>>>
    {
        public async Task<Result<List<GetAllLessonDTO>>> Handle
        (GetLessonsByCategoryTitleQuery request, CancellationToken cancellationToken)
        {
            var lessons = await unitOfWork
                                       .LessonRepository
                                       .GetLessonsByCategoryTitle(request.Title);
            var category = await unitOfWork
                                    .CategoryRepository
                                    .GetCategoryByTitle(request.Title);
            if (category?.Title != request.Title)
                return Result<List<GetAllLessonDTO>>
                .Failure(CategoryErrors.NoTitleFound(request.Title));
            return Result<List<GetAllLessonDTO>>
                .Success(
                [.. lessons.Select(l => new GetAllLessonDTO{

                Accent = l.Accent,
                AudioUrl = l.AudioUrl,
                Description = l.Description,
                Duration = l.Duration,
                LessonId = l.Id,
                Level = l.Level,
                Sentences = l.Sentences,
                Topics = l.Topics,
                Title = l.Title,
                VideoId = l.VideoId,
                YoutubeUrl = l.YoutubeUrl
            }).ToList()]);
        }
    }
}