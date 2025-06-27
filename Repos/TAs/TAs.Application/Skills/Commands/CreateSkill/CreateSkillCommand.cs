using MediatR;
using TAs.Application.Skills.Commands.DTOs;
using TAs.Domain.Result;
namespace TAs.Application.Skills.Commands.CreateSkill
{
    public class CreateSkillCommand : CreateAndUpdateSkill, IRequest<Result>;
}