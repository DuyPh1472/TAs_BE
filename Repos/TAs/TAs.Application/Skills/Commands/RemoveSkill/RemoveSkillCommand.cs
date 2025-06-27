using MediatR;
using TAs.Domain.Result;
namespace TAs.Application.Skills.Commands.RemoveSkill
{
    public class RemoveSkillCommand(Guid id) : IRequest<Result>
    {
        public Guid skillId = id;
    }
}