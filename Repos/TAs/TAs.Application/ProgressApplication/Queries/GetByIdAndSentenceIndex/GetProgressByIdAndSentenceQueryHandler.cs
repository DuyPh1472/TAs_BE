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
                ProgressId = progress.ProgressId,
                CompletedAt = progress
                .ProgressDetails
                .FirstOrDefault(pd => pd.SentenceIndex == request.SentenceIndex)
                ?.CompletedAt,
                IsCompleted = progress.ProgressDetails
               .Where(pd => pd.SentenceIndex == request.SentenceIndex)
               .Select(pd => pd.IsCompleted)
               .FirstOrDefault(),
                SentenceIndex = progress.ProgressDetails
               .Where(pd => pd.SentenceIndex == request.SentenceIndex)
               .Select(pd => pd.SentenceIndex)
               .FirstOrDefault()
            };
            return Result<GetProgressByIdAndSentenceIndexDTO>.Success(response);
        }
    }
}