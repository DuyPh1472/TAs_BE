using MediatR;
using TAs.Application.GameRooms.DTOs.Commands;
using TAs.Domain.Result;

namespace TAs.Application.GameRooms.Commands.Update.StartGame
{
    public class StartGameRoomCommand : IRequest<Result<StartGameRoomDTO>>
    {
        public Guid RoomId { get; set; }
        public Guid? LessonId { get; set; }
        public StartGameRoomCommand(Guid roomId, Guid? lessonId = null)
        {
            RoomId = roomId;
            LessonId = lessonId;
        }
    }
}