using MediatR;
using TAs.Domain.Result;
namespace TAs.Application.Users.UserDetail
{
    public class UserDetailCommand : IRequest<Result<object>>
    {
        public DateTime? DateOfBirth { get; set; }
        public string Avatar { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;

    }
}