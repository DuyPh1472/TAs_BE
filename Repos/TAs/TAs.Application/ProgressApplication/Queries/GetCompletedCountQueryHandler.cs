using MediatR;
using TAs.Domain.Result;
using TAs.Application.ProgressApplication.DTOs;
using TAs.Application.Interfaces;

namespace TAs.Application.ProgressApplication.Queries
{
    public class GetCompletedCountQueryHandler(IUnitOfWork unitOfWork) :
    IRequestHandler<GetCompletedCountQuery, Result<CompletedCountResponse>>
    {
        
        public async Task<Result<CompletedCountResponse>> Handle(GetCompletedCountQuery request, CancellationToken cancellationToken)
        {
            var count = await unitOfWork.ProgressRepository.CountCompleted(request.ProgressId);
            var response = new CompletedCountResponse
            {
                ProgressId = request.ProgressId,
                CompletedCount = count
            };
            return Result<CompletedCountResponse>.Success(response);
        }
    }
} 