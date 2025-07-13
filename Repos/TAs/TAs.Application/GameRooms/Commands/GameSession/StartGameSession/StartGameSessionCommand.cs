using MediatR;
using TAs.Application.GameRooms.DTOs.Queries;
using TAs.Domain.Result;

namespace TAs.Application.GameRooms.Commands.GameSession.StartGameSession
{
    public class StartGameSessionCommand : IRequest<Result<GameSessionDTO>>
    {
        public Guid RoomId { get; set; }
        public Guid LessonId { get; set; }
    }
} 