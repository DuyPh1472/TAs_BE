using MediatR;
using TAs.Application.Identity.DTOs;
using TAs.Domain.Result;

namespace TAs.Application.Identity.Commands.Login
{
    public class LoginCommand :LoginDTO, IRequest<Result<string>>
    {
        
    }
}