using MediatR;
using TAs.Domain.Result;

namespace TAs.Application.GameRooms.Commands.Update.SelectLesson
{
    public class SelectLessonCommand : IRequest<Result<bool>>
    {
        public Guid RoomId { get; set; }
        public Guid LessonId { get; set; }
        public SelectLessonCommand(Guid roomId, Guid lessonId)
        {
            RoomId = roomId;
            LessonId = lessonId;
        }
    }
}