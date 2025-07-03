using MediatR;
using TAs.Application.Lessons.DTOs;
using TAs.Domain.Result;

namespace TAs.Application.Lessons.Queries.GetById
{
    public class GetLessonByIdQuery : IRequest<Result<LessonDTO>>
    {
        public Guid LessonId { get; }
        public GetLessonByIdQuery(Guid lessonId)
        {
            LessonId = lessonId;
        }
    }

    public class GetLessonByIdResult
    {
        public bool IsSuccess { get; set; }
        public LessonDTO? Data { get; set; }
        public ErrorResult? Error { get; set; }
    }

    public class ErrorResult
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
} 