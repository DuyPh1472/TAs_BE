using System.Collections.Concurrent;
using TAs.Domain.Enums;

namespace TAs.Application.GameRooms
{
    public class InMemoryGameRoomService : IInMemoryGameRoomService
    {
        private readonly ConcurrentDictionary<Guid, GameRoomState> _rooms = new();

        public IEnumerable<GameRoomState> GetAllRooms() => _rooms.Values;
        public GameRoomState? GetRoom(Guid roomId) => _rooms.TryGetValue(roomId, out var room) ? room : null;

        public GameRoomState AddRoom(GameRoomState room)
        {
            _rooms[room.RoomId] = room;
            return room;
        }

        public AddPlayerResult AddPlayer(Guid roomId, PlayerState player)
        {
            if (!_rooms.TryGetValue(roomId, out var room))
            {
                return new AddPlayerResult { Success = false, Status = "RoomNotFound", Message = $"Room {roomId} not found" };
            }
            if (room.Players.Count >= room.Settings.MaxPlayers)
            {
                return new AddPlayerResult { Success = false, Status = "RoomFull", Message = $"Room {roomId} is full" };
            }
            // Kiểm tra user đã ở phòng này chưa
            if (room.Players.Exists(p => p.UserId == player.UserId))
            {
                return new AddPlayerResult { Success = false, Status = "AlreadyInThisRoom", Message = $"User {player.UserId} already in this room" };
            }
            // Kiểm tra user đã ở phòng khác chưa
            var inOtherRoom = _rooms.Values.Any(r => r.RoomId != roomId && r.Players.Any(p => p.UserId == player.UserId));
            if (inOtherRoom)
            {
                var otherRoom = _rooms.Values.First(r => r.RoomId != roomId && r.Players.Any(p => p.UserId == player.UserId));
                return new AddPlayerResult { Success = false, Status = "AlreadyInAnotherRoom", Message = $"User {player.UserId} already in another room" };
            }
            if (room.Players.Count == 0)
            {
                room.HostId = player.UserId;
                player.IsHost = true;
            }
            room.Players.Add(player);
            return new AddPlayerResult { Success = true, Status = "Success" };
        }

        public void RemovePlayer(Guid roomId, Guid userId)
        {
            if (_rooms.TryGetValue(roomId, out var room))
            {
                room.Players.RemoveAll(p => p.UserId == userId);
                if (room.Players.Count == 0)
                    _rooms.TryRemove(roomId, out _);
                else if (room.HostId == userId)
                    room.HostId = room.Players[0].UserId;
            }
        }

        public bool SetLessonForRoom(Guid roomId, Guid lessonId)
        {
            if (_rooms.TryGetValue(roomId, out var room))
            {
                room.LessonId = lessonId;
                room.Settings.LessonId = lessonId; // Đảm bảo đồng bộ với FE
                return true;
            }
            return false;
        }

        public ReadyPlayerResult ReadyInMemory(Guid roomId, Guid userId, bool isReady)
        {
            if (!_rooms.TryGetValue(roomId, out var room))
            {
                return new ReadyPlayerResult { Success = false, Status = "RoomNotFound", Message = $"Room {roomId} not found" };
            }

            var player = room.Players.FirstOrDefault(p => p.UserId == userId);
            if (player == null)
            {
                return new ReadyPlayerResult { Success = false, Status = "PlayerNotFound", Message = $"Player {userId} not found in room {roomId}" };
            }

            player.IsReady = isReady;
            return new ReadyPlayerResult { Success = true, Status = "Success", Message = $"Player {userId} ready state set to {isReady}" };
        }

        public KickPlayerResult KickPlayerInMemory(Guid roomId, Guid hostId, Guid targetUserId)
        {
            if (!_rooms.TryGetValue(roomId, out var room))
            {
                return new KickPlayerResult { Success = false, Status = "RoomNotFound", Message = $"Room {roomId} not found" };
            }

            // Check if the user trying to kick is the host
            if (room.HostId != hostId)
            {
                return new KickPlayerResult { Success = false, Status = "Unauthorized", Message = "Only the host can kick players" };
            }

            // Check if target user exists in the room
            var targetPlayer = room.Players.FirstOrDefault(p => p.UserId == targetUserId);
            if (targetPlayer == null)
            {
                return new KickPlayerResult { Success = false, Status = "PlayerNotFound", Message = $"Player {targetUserId} not found in room {roomId}" };
            }

            // Check if target user is the host (host cannot kick themselves)
            if (targetPlayer.IsHost)
            {
                return new KickPlayerResult { Success = false, Status = "CannotKickHost", Message = "Host cannot kick themselves" };
            }

            // Remove the player from the room
            room.Players.RemoveAll(p => p.UserId == targetUserId);
            
            return new KickPlayerResult { Success = true, Status = "Success", Message = $"Player {targetUserId} has been kicked from room {roomId}" };
        }

        public Guid CreateRoom(
            string roomName,
            int maxPlayers,
            Guid categoryId,
            string categoryTitle,
            Guid hostId,
            string hostName,
            string categoryDifficult
        )
        {
            var roomId = Guid.NewGuid();
            var room = new GameRoomState
            {
                RoomId = roomId,
                RoomName = roomName,
                HostId = hostId,
                CategoryId = categoryId,
                CategoryTitle = categoryTitle,
                CategoryDifficult = categoryDifficult,
                Settings = new RoomSettings
                {
                    MaxPlayers = maxPlayers,
                    TimeLimit = 60,
                    MaxRetries = 2,
                    ShowRealTimeScore = true,
                    AllowHints = true,
                    LessonSelection = "host_choice"
                },
                Players = new List<PlayerState>
                {
                    new PlayerState
                    {
                        UserId = hostId,
                        UserName = hostName,
                        Avatar = !string.IsNullOrEmpty(hostName) ? hostName[0].ToString() : "U",
                        IsHost = true,
                        IsReady = true,
                        // Add other properties as needed
                    }
                },
                // Add other properties as needed
            };
            _rooms[roomId] = room;
            return roomId;
        }

        // Thêm method để lấy danh sách phòng dưới dạng DTO
        public IEnumerable<object> GetActiveRoomsDTO()
        {
            return _rooms.Values.Select(room =>
            {
                var host = room.Players.FirstOrDefault(p => p.IsHost);
                var hostAvatar = !string.IsNullOrEmpty(host?.Avatar) ? host.Avatar.Substring(0, 1) : "U";
                return new
                {
                    id = room.RoomId.ToString(),
                    roomName = room.RoomName,
                    CategoryId = room.CategoryId, // Đổi từ categoryId sang CategoryId
                    hostId = room.HostId.ToString(),
                    hostName = host?.UserName ?? "Unknown",
                    hostAvatar = hostAvatar,
                    playerCount = room.Players.Count,
                    maxPlayers = room.Settings.MaxPlayers,
                    status = room.GameStatus.ToString().ToLower(),
                    categoryTitle = room.CategoryTitle,
                    categoryDescription = room.CategoryDescription,
                    createdAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    settings = new
                    {
                        timeLimit = room.Settings.TimeLimit,
                        maxRetries = room.Settings.MaxRetries,
                        showRealTimeScore = room.Settings.ShowRealTimeScore,
                        allowHints = room.Settings.AllowHints,
                        lessonSelection = room.Settings.LessonSelection
                    },
                    players = room.Players.Select(p => new
                    {
                        userId = p.UserId.ToString(),
                        userName = p.UserName,
                        avatar = p.Avatar,
                        isHost = p.IsHost,
                        isReady = p.IsReady,
                        score = 0,
                        currentProgress = 0,
                        status = PlayerStatus.Connected,
                        joinedAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                    }).ToArray()
                };
            });
        }

        public object? GetRoomDetailsDTO(Guid roomId)
        {
            var room = GetRoom(roomId);
            if (room == null) return null;
            var host = room.Players.FirstOrDefault(p => p.IsHost);
            var hostAvatar = !string.IsNullOrEmpty(host?.Avatar) ? host.Avatar.Substring(0, 1) : "U";
            return new
            {
                id = room.RoomId.ToString(),
                roomName = room.RoomName,
                hostId = room.HostId.ToString(),
                hostName = host?.UserName ?? "Unknown",
                hostAvatar = hostAvatar,
                status = room.GameStatus,
                maxPlayers = room.Settings.MaxPlayers,
                currentPlayers = room.Players.Count,
                selectedLessonId = room.Settings.LessonId?.ToString(),
                selectedLessonTitle = room.Settings.LessonId != null ? "Selected Lesson" : null,
                currentSentence = 0,
                categoryId = room.CategoryId,
                categoryTitle = room.CategoryTitle,
                categoryDescription = room.CategoryDescription,
                categoryDifficult = room.CategoryDifficult,
                createdAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                createdBy = room.HostId.ToString(),
                players = room.Players.Select(p => new
                {
                    userId = p.UserId.ToString(),
                    userName = p.UserName,
                    avatar = p.Avatar,
                    isHost = p.IsHost,
                    isReady = p.IsReady,
                    score = 0,
                    currentProgress = 0,
                    status = PlayerStatus.Connected,
                    joinedAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                }).ToArray(),
                settings = new
                {
                    timeLimit = room.Settings.TimeLimit,
                    maxRetries = room.Settings.MaxRetries,
                    showRealTimeScore = room.Settings.ShowRealTimeScore,
                    allowHints = room.Settings.AllowHints,
                    lessonSelection = room.Settings.LessonSelection
                }
            };
        }
    }


    public class AddPlayerResult
    {
        public bool Success { get; set; }
        public string? Status { get; set; } // "Success", "AlreadyInThisRoom", "AlreadyInAnotherRoom", "RoomFull", "RoomNotFound"
        public string? Message { get; set; }
    }

    public class ReadyPlayerResult
    {
        public bool Success { get; set; }
        public string? Status { get; set; } // "Success", "RoomNotFound", "PlayerNotFound"
        public string? Message { get; set; }
    }

    public class ReadyInMemoryResult
    {
        public bool Success { get; set; }
        public string? Status { get; set; } // "Success", "RoomNotFound", "PlayerNotFound", "Unauthorized"
        public string? Message { get; set; }
    }

    public class KickPlayerResult
    {
        public bool Success { get; set; }
        public string? Status { get; set; } // "Success", "RoomNotFound", "PlayerNotFound", "Unauthorized", "CannotKickHost"
        public string? Message { get; set; }
    }
}