using MediatR;
using TAs.Application.Interfaces;
using TAs.Application.ProgressApplication.DTOs.Queries.GetByIdAndSentence;
using TAs.Application.ProgressApplication.HandleProgressErrors;
using TAs.Domain.Result;

namespace TAs.Application.ProgressApplication.Queries.GetByIdAndSentenceIndex
{
    public class GetProgressByIdAndSentenceQueryHandler(IUnitOfWork unitOfWork) :
     IRequestHandler<GetProgressByIdAndSentenceQuery, Result<GetProgressByIdAndSentenceIndexDTO>>
    {
        public async Task<Result<GetProgressByIdAndSentenceIndexDTO>>
        Handle(GetProgressByIdAndSentenceQuery request, CancellationToken cancellationToken)
        {
            var progress = await unitOfWork
            .ProgressRepository
            .GetProgressByIdAndSentenceIndex(request.ProgressId, request.SentenceIndex);
            if (progress == null)
                return Result<GetProgressByIdAndSentenceIndexDTO>
                .Failure(ProgressError
                .ProgressNotFound(request.ProgressId, request.SentenceIndex));
            var response = new GetProgressByIdAndSentenceIndexDTO
            {
                ProgressId = progress.Id,
                LessonId = progress.LessonId,
                UserId = progress.UserId,
                CurrentSentence = progress.CurrentSentence,
                CompletedSentences = progress.ProgressChallenge,
                TotalSentences = progress.TotalChallenge,
                Score = progress.Score,
                Status = progress.ProgressStatus ? "completed" : "in_progress",
                StartedAt = progress.StartedAt,
                CompletedAt = progress.CompletedAt,
                LastUpdatedAt = progress.UpdatedAt
            };
            return Result<GetProgressByIdAndSentenceIndexDTO>.Success(response);
        }
    }
}