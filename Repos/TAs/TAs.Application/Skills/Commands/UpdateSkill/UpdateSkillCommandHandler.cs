using MediatR;
using Microsoft.Extensions.Logging;
using TAs.Application.Interfaces.Repositories;
using TAs.Application.Skills.HandleError;
using TAs.Application.Users;
using TAs.Domain.Result;
namespace TAs.Application.Skills.Commands.UpdateSkill
{
    public class UpdateSkillCommandHandler(ISkillRepository skillRepository,
    ILogger<UpdateSkillCommandHandler> logger,
    IUserContext userContext)
     : IRequestHandler<UpdateSkillCommand, Result>
    {
        public async Task<Result> Handle(UpdateSkillCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Update Skill");
            var user = userContext.GetCurrentUser();
            var skill = await skillRepository.GetSkillByIdAsync(request.Id);
            if (skill is null)
                return Result.Failure(SkillErrors.IdNotFound(request.Id));
            skill.Color = request.command.Color;
            skill.Description = request.command.Description;
            skill.Icon = request.command.Icon;
            skill.Name = request.command.Name;
            skill.Path = request.command.Path;
            skill.UpdatedBy = user!.Id;
            skill.UpdatedAt = DateTime.UtcNow;
            skillRepository.Update(skill);
            await skillRepository.SaveChangeAsync();
            return Result.Success();
        }
    }
}