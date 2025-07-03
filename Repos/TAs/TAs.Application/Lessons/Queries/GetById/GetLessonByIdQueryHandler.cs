using MediatR;
using TAs.Application.Interfaces;
using TAs.Application.Lessons.DTOs;
using TAs.Application.Lessons.HandlerLessonErrors;
using TAs.Domain.Result;

namespace TAs.Application.Lessons.Queries.GetById
{
    public class GetLessonByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetLessonByIdQuery, Result<LessonDTO>>
    {

        public async Task<Result<LessonDTO>> Handle(GetLessonByIdQuery request, CancellationToken cancellationToken)
        {
            var lesson = await unitOfWork.LessonRepository.GetLessonsById(request.LessonId);

            if (lesson == null)
                return Result<LessonDTO>.Failure(LessonError.IdNotFound(request.LessonId));
            var response = new LessonDTO
            {
                LessonId = lesson.Id,
                Title = lesson.Title,
                Description = lesson.Description,
                Level = lesson.Level,
                Sentences = lesson.Sentences,
                Accent = lesson.Accent,
                Duration = lesson.Duration,
                Topics = lesson.Topics,
                AudioUrl = lesson.AudioUrl,
                YoutubeUrl = lesson.YoutubeUrl,
                VideoId = lesson.VideoId,
                Challenges = lesson.DictationSentences?.OrderBy(x => x.Position).Select(ds => new DictationSentenceDTO
                {
                    Id = ds.Id,
                    Position = ds.Position,
                    Content = ds.Text,
                    AudioSrc = ds.AudioUrl,
                    TimeStart = ds.StartTime,
                    TimeEnd = ds.EndTime,
                }).ToList() ?? new()
            };
            return Result<LessonDTO>.Success(response);
        }
    }
}