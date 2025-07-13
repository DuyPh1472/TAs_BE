using System.Collections.Concurrent;

namespace TAs.Application.GameRooms
{
    public class InMemoryGameRoomService
    {
        private readonly ConcurrentDictionary<Guid, GameRoomState> _rooms = new();

        public IEnumerable<GameRoomState> GetAllRooms() => _rooms.Values;
        public GameRoomState? GetRoom(Guid roomId) => _rooms.TryGetValue(roomId, out var room) ? room : null;

        public GameRoomState CreateRoom(Guid roomId, string roomName, int maxPlayers, Guid categoryId)
        {
            var settings = new RoomSettings
            {
                MaxPlayers = maxPlayers,
                TimeLimit = 60,
                MaxRetries = 2,
                ShowRealTimeScore = true,
                AllowHints = true,
                LessonSelection = "host_choice"
            };

            var room = new GameRoomState
            {
                RoomId = roomId,
                RoomName = roomName,
                HostId = Guid.Empty, // Sẽ được set khi host join qua SignalR
                Settings = settings,
                Players = new List<PlayerState>()
            };
            _rooms[roomId] = room;
            return room;
        }

        public GameRoomState CreateRoom(Guid hostId, string hostName, RoomSettings settings)
        {
            var roomId = Guid.NewGuid();
            var room = new GameRoomState
            {
                RoomId = roomId,
                HostId = hostId,
                Settings = settings,
                Players = new List<PlayerState>
                {
                    new PlayerState { 
                        UserId = hostId, 
                        UserName = hostName, 
                        Avatar = hostName[0].ToString(),
                        IsHost = true 
                    }
                }
            };
            _rooms[roomId] = room;
            return room;
        }

        public bool JoinRoom(Guid roomId, Guid userId, string userName)
        {
            if (_rooms.TryGetValue(roomId, out var room))
            {
                if (room.Players.Count >= room.Settings.MaxPlayers) return false;
                if (room.Players.Exists(p => p.UserId == userId)) return false;
                
                var player = new PlayerState { 
                    UserId = userId, 
                    UserName = userName,
                    Avatar = userName[0].ToString()
                };
                
                // Nếu là player đầu tiên, set làm host
                if (room.Players.Count == 0)
                {
                    room.HostId = userId;
                    player.IsHost = true;
                }
                
                room.Players.Add(player);
                return true;
            }
            return false;
        }

        public void LeaveRoom(Guid roomId, Guid userId)
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

        // Thêm method để lấy danh sách phòng dưới dạng DTO
        public IEnumerable<object> GetActiveRoomsDTO()
        {
            return _rooms.Values.Select(room => {
                var host = room.Players.FirstOrDefault(p => p.IsHost);
                var hostAvatar = !string.IsNullOrEmpty(host?.Avatar) ? host.Avatar.Substring(0, 1) : "U";
                return new
                {
                    id = room.RoomId.ToString(),
                    roomName = room.RoomName,
                    hostId = room.HostId.ToString(),
                    hostName = host?.UserName ?? "Unknown",
                    hostAvatar = hostAvatar,
                    playerCount = room.Players.Count,
                    maxPlayers = room.Settings.MaxPlayers,
                    status = room.GameStatus,
                    categoryTitle = "Dictation",
                    categoryDescription = "Multiplayer Dictation",
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
                        status = "Connected",
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
                categoryId = "dictation",
                categoryTitle = "Dictation",
                categoryDescription = "Multiplayer Dictation",
                categoryDifficult = "Intermediate",
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
                    status = "Connected",
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
} 