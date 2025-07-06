using MediatR;
using TAs.Domain.Result;

namespace TAs.Application.ProgressApplication.Commands.Create
{
    public class CreateProgressCommand : IRequest<Result<Guid>>
    {
        public Guid LessonId { get; set; }
        public CreateProgressCommand( Guid lessonId)
        {
            LessonId = lessonId;
        }
    }
}