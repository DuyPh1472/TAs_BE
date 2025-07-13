using MediatR;
using TAs.Application.Interfaces;
using TAs.Application.GameRooms.DTOs.Queries;
using TAs.Domain.Result;

namespace TAs.Application.GameRooms.Queries.GetRoomPlayers
{
    public class GetRoomPlayersQueryHandler(IUnitOfWork unitOfWork) 
        : IRequestHandler<GetRoomPlayersQuery, Result<List<PlayerDTO>>>
    {
        public async Task<Result<List<PlayerDTO>>> Handle(GetRoomPlayersQuery request, CancellationToken cancellationToken)
        {
            var players = await unitOfWork.PlayerInRoomRepository.GetPlayersByRoomIdAsync(request.RoomId);
            
            var playerDTOs = players.Select(p => new PlayerDTO
            {
                Id = p.UserId,
                Name = p.User.FullName,
                Avatar = p.User.Avatar,
                IsHost = p.IsHost,
                IsReady = p.IsReady,
                Score = p.Score,
                Status = p.Status.ToString(),
                JoinedAt = p.JoinedAt
            }).ToList();

            return Result<List<PlayerDTO>>.Success(playerDTOs);
        }
    }
} 