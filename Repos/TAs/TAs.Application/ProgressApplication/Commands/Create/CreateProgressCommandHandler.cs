using MediatR;
using TAs.Domain.Result;

namespace TAs.Application.ProgressApplication.Commands.Create
{
    public class CreateProgressCommandHandler : IRequestHandler<CreateProgressCommand, Result>
    {
        public Task<Result> Handle(CreateProgressCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}