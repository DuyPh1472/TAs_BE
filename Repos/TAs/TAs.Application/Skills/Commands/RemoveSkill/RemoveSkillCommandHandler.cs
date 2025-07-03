using MediatR;
using TAs.Application.Interfaces.Repositories;
using TAs.Application.Skills.HandleError;
using TAs.Domain.Result;

namespace TAs.Application.Skills.Commands.RemoveSkill
{
    public class RemoveSkillCommandHandler(ISkillRepository skillRepository)
     : IRequestHandler<RemoveSkillCommand, Result>
    {
        public async Task<Result> Handle(RemoveSkillCommand request, CancellationToken cancellationToken)
        {
            var skill = await skillRepository.GetByIdAsync(request.skillId);
            if (skill is null)
                return Result.Failure(SkillErrors.IdNotFound(request.skillId));
            skillRepository.Delete(skill);
            await skillRepository.SaveChangeAsync();
            return Result.Success();
        }
    }
}