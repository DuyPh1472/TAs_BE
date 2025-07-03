using MediatR;
using TAs.Application.Interfaces;
using TAs.Application.ProgressApplication.HandleProgressErrors;
using TAs.Domain.Result;

namespace TAs.Application.ProgressApplication.Commands
{
    public class DeleteProgressCommandHandler(IUnitOfWork unitOfWork)
     : IRequestHandler<DeleteProgressCommand, Result>
    {
        
        public async Task<Result> Handle(DeleteProgressCommand request, CancellationToken cancellationToken)
        {
            var progress = await unitOfWork.ProgressRepository.GetProgressByIdAsync(request.ProgressId);
            if (progress == null)
                return Result.Failure(ProgressError.ProgressIdNotFound(request.ProgressId));
            unitOfWork.ProgressRepository.Delete(progress);
            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
} 