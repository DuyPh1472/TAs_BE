using MediatR;
using TAs.Application.Identity.DTOs;
using TAs.Domain.Result;

namespace TAs.Application.Identity.Commands
{
    public class RegisterCommand : RegisterDTO, IRequest<Result>;

}