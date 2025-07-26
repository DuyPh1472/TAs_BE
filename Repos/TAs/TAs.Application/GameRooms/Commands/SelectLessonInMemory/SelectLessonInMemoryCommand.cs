using MediatR;
using TAs.Application.Lessons.DTOs;

namespace TAs.Application.GameRooms.Commands.SelectLessonInMemory
{
    public class SelectLessonInMemoryCommand : IRequest<SelectLessonInMemoryResult>
    {
        public Guid RoomId { get; set; }
        public Guid LessonId { get; set; }
    }

    public class SelectLessonInMemoryResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public LessonDTO? Lesson { get; set; }
    }
} 