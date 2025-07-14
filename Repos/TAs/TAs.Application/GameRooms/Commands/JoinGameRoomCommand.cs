using MediatR;
using System;

namespace TAs.Application.GameRooms.Commands
{
    public class JoinGameRoomCommand : IRequest<bool>
    {
        public Guid RoomId { get; set; }
    }
} 