using MediatR;
using TAs.Domain.Result;
using System;

namespace TAs.Application.ProgressApplication.Commands.Create
{
    public class CreateProgressCommand : IRequest<Result<Guid>>
    {
        public Guid UserId { get; set; }
        public Guid LessonId { get; set; }
        public CreateProgressCommand(Guid userId, Guid lessonId)
        {
            UserId = userId;
            LessonId = lessonId;
        }
    }
}