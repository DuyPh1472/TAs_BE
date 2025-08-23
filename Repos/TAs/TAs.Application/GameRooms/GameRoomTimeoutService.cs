using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TAs.Domain.Enums;
using TAs.Application.GameRooms.Commands.GameSession.SaveGameResult;
using MediatR;

namespace TAs.Application.GameRooms
{
    public class GameRoomTimeoutService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<GameRoomTimeoutService> _logger;

        public GameRoomTimeoutService(
            IServiceProvider serviceProvider,
            ILogger<GameRoomTimeoutService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("GameRoomTimeoutService started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CheckForGameTimeouts(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while checking for game timeouts");
                }

                // Check every 2 seconds
                await Task.Delay(2000, stoppingToken);
            }

            _logger.LogInformation("GameRoomTimeoutService stopped");
        }

        private async Task CheckForGameTimeouts(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var inMemoryService = scope.ServiceProvider.GetRequiredService<IInMemoryGameRoomService>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var activeRooms = inMemoryService.GetAllRooms()
                .Where(r => r.GameStatus == GameStatus.Playing && r.GameStartedAt.HasValue)
                .ToList();

            foreach (var room in activeRooms)
            {
                if (stoppingToken.IsCancellationRequested) break;

                var timeLimit = room.Settings?.TimeLimit ?? 60;
                var gameStartTime = room.GameStartedAt.GetValueOrDefault();
                var timeoutTime = gameStartTime.AddSeconds(timeLimit);

                if (DateTime.UtcNow >= timeoutTime)
                {
                    _logger.LogInformation("Game timeout detected for room {RoomId}. Started at {StartTime}, TimeLimit: {TimeLimit} seconds", 
                        room.RoomId, gameStartTime, timeLimit);

                    await EndGameAutomatically(room.RoomId, mediator, inMemoryService, scope.ServiceProvider);
                }
            }
        }

        private async Task EndGameAutomatically(Guid roomId, IMediator mediator, IInMemoryGameRoomService inMemoryService, IServiceProvider serviceProvider)
        {
            try
            {
                _logger.LogInformation("Automatically ending game for room {RoomId}", roomId);

                // Get game result from memory
                var gameResult = inMemoryService.GetGameResultForSaving(roomId);

                if (gameResult != null)
                {
                    // Lấy thông tin phòng từ in-memory để lưu đầy đủ
                    var room = inMemoryService.GetRoom(roomId);
                    if (room != null)
                    {
                        // Create command to save complete result - lưu đầy đủ 3 bảng
                        var saveCommand = new SaveCompleteGameResultCommand
                        {
                            RoomId = roomId,
                            RoomName = room.RoomName,
                            HostId = room.HostId,
                            CategoryId = room.CategoryId,
                            LessonId = gameResult.LessonId ?? Guid.Empty,
                            StartedAt = gameResult.GameStartedAt ?? DateTime.UtcNow.AddMinutes(-10),
                            EndedAt = DateTime.UtcNow,
                            TotalSentences = 10, // Can be improved to get from lesson
                            TimeLimit = gameResult.Settings?.TimeLimit ?? 60,
                            MaxRetries = gameResult.Settings?.MaxRetries ?? 2,
                            ShowRealTimeScore = gameResult.Settings?.ShowRealTimeScore ?? true,
                            AllowHints = gameResult.Settings?.AllowHints ?? true,
                            LessonSelection = gameResult.Settings?.LessonSelection ?? "host_choice",
                            PlayerResults = gameResult.Players.Select(p => new PlayerGameResult
                            {
                                UserId = p.UserId,
                                UserName = p.UserName,
                                FinalScore = p.Score,
                                CorrectAnswers = p.Score, // Assuming each point = 1 correct answer
                                IncorrectAnswers = 0, // Can be calculated from attempt history
                                TotalTimeSpent = gameResult.Settings?.TimeLimit ?? 60, // Assuming used all time
                                TotalRetries = 0, // Can be retrieved from attempt history
                                AverageTimePerSentence = (gameResult.Settings?.TimeLimit ?? 60) / 10.0f // Assuming 10 sentences
                            }).ToList()
                        };

                        // Send command to save result
                        var saveResult = await mediator.Send(saveCommand);

                        if (saveResult.Success)
                        {
                            _logger.LogInformation("Game result saved successfully for room {RoomId}. GameSessionId: {GameSessionId}",
                                roomId, saveResult.GameSessionId);
                        }
                        else
                        {
                            _logger.LogError("Failed to save game result for room {RoomId}: {Message}",
                                roomId, saveResult.Message);
                        }
                    }
                    else
                    {
                        _logger.LogWarning("No game result found for room {RoomId}", roomId);
                    }

                    // Update room status to finished
                    var updateRoom = inMemoryService.GetRoom(roomId);
                    if (updateRoom != null)
                    {
                        updateRoom.GameStatus = GameStatus.Finished;
                        _logger.LogInformation("Room {RoomId} status updated to Finished", roomId);
                    }

                    // Broadcast GameFinished event to all clients in the room
                    try
                    {
                        var eventService = serviceProvider.GetService<IGameRoomEventService>();
                        if (eventService != null)
                        {
                            await eventService.BroadcastGameFinishedAsync(roomId);
                            _logger.LogInformation("GameFinished event broadcasted to room {RoomId}", roomId);
                        }
                        else
                        {
                            _logger.LogWarning("GameRoomEventService not available for room {RoomId}", roomId);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to broadcast GameFinished event for room {RoomId}", roomId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while automatically ending game for room {RoomId}", roomId);
            }
        }
    }
} 