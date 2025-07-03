using MediatR;
using TAs.Application.Interfaces;
using TAs.Application.Lessons.DTOs;
using TAs.Domain.Result;

namespace TAs.Application.Lessons.Queries.GetAll
{
    public class GetAllLessonsQueryHandler(IUnitOfWork unitOfWork) :
     IRequestHandler<GetAllLessonsQuery, Result<List<GetAllLessonDTO>>>
    {
        public async Task<Result<List<GetAllLessonDTO>>> Handle(GetAllLessonsQuery request, CancellationToken cancellationToken)
        {
            var queries = await unitOfWork.LessonRepository.GetAllAsync();
            var response = queries.Select(l => new GetAllLessonDTO
            {
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

            }).ToList();
            return Result<List<GetAllLessonDTO>>.Success(response);
        }
    }
}