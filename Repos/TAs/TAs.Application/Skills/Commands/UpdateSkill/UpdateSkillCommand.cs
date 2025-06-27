using MediatR;
using TAs.Application.Skills.Commands.DTOs;
using TAs.Domain.Result;

namespace TAs.Application.Skills.Commands.UpdateSkill
{
    public class UpdateSkillCommand(Guid id, CreateAndUpdateSkill updateSkill)
    : IRequest<Result>
    {
        public Guid Id = id;
        public CreateAndUpdateSkill command = updateSkill;
    }
}