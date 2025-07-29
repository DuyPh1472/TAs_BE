using MediatR;
using TAs.Application.Interfaces;
using TAs.Application.Lessons.DTOs;

namespace TAs.Application.GameRooms.Commands.SelectLessonInMemory
{
    public class SelectLessonInMemoryCommandHandler(IInMemoryGameRoomService roomService, IUnitOfWork unitOfWork) : IRequestHandler<SelectLessonInMemoryCommand, SelectLessonInMemoryResult>
    {
        private readonly IInMemoryGameRoomService _roomService = roomService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<SelectLessonInMemoryResult> Handle(SelectLessonInMemoryCommand request, CancellationToken cancellationToken)
        {
            var lesson = await _unitOfWork.LessonRepository.GetByIdAsync(request.LessonId);
            if (lesson == null)
            {
                return new SelectLessonInMemoryResult { Success = false, Message = "Lesson not found" };
            }

            var updated = _roomService.SetLessonForRoom(request.RoomId, request.LessonId);
            if (!updated)
            {
                return new SelectLessonInMemoryResult { Success = false, Message = "Room not found or update failed" };
            }

            return new SelectLessonInMemoryResult
            {
                Success = true,
                Lesson = new LessonDTO
                {
                    LessonId = lesson.Id,
                    Title = lesson.Title,
                    Description = lesson.Description,
                    Level = lesson.Level,
                    Sentences = lesson.Sentences,
                    Accent = lesson.Accent,
                    Duration = lesson.Duration,
                    Topics = lesson.Topics,
                    AudioUrl = lesson.AudioUrl,
                    YoutubeUrl = lesson.YoutubeUrl,
                    VideoId = lesson.VideoId,
                    // Nếu có DictationSentences thì map sang Challenges, nếu không thì để rỗng
                    Challenges = lesson.DictationSentences?.OrderBy(x => x.Position).Select(ds => new DictationSentenceDTO
                    {
                        Id = ds.Id,
                        Position = ds.Position,
                        Content = ds.Text,
                        AudioSrc = ds.AudioUrl,
                        TimeStart = ds.StartTime,
                        TimeEnd = ds.EndTime,
                        // Các property khác nếu cần
                    }).ToList() ?? new()
                }
            };
        }
    }
} 