using MediatR;
using TAs.Domain.Result;

namespace TAs.Application.ProgressApplication.Commands.Update
{
    public class UpdateProgressCommand : IRequest<Result>
    {
        public Guid LessonId { get; set; }
        public int SentenceIndex { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsCorrect { get; set; }
        public string UserAnswer { get; set; } = string.Empty;
    }
}