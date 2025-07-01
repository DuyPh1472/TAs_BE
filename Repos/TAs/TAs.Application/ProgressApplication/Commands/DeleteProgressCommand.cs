using MediatR;
using System;
using TAs.Domain.Result;

namespace TAs.Application.ProgressApplication.Commands
{
    public class DeleteProgressCommand : IRequest<Result>
    {
        public Guid ProgressId { get; set; }
        public DeleteProgressCommand(Guid progressId)
        {
            ProgressId = progressId;
        }
    }
} 