using MediatR;
using Microsoft.Extensions.Logging;
using TAs.Application.Skills.HandleError;
using TAs.Domain.Repositories;
using TAs.Domain.Result;
namespace TAs.Application.Skills.Commands.UpdateSkill
{
    public class UpdateSkillCommandHandler(ISkillRepository skillRepository,
    ILogger<UpdateSkillCommandHandler> logger)
     : IRequestHandler<UpdateSkillCommand, Result>
    {
        public async Task<Result> Handle(UpdateSkillCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Update Skill");
            var skill = await skillRepository.GetSkillByIdAsync(request.Id);
            if (skill is null)
                return Result.Failure(SkillErrors.IdNotFound(request.Id));
            skill.Color = request.command.Color;
            skill.Description = request.command.Description;
            skill.Icon = request.command.Icon;
            skill.Name = request.command.Name;
            skill.Path = request.command.Path;
            skill.UpdatedAt = DateTime.UtcNow;
            skillRepository.Update(skill);
            await skillRepository.SaveChangeAsync();
            return Result.Success();
        }
    }
}