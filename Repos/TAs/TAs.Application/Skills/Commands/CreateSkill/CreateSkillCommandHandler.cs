using MediatR;
using Microsoft.Extensions.Logging;
using TAs.Application.Skills.HandleError;
using TAs.Domain.Entities;
using TAs.Domain.Repositories;
using TAs.Domain.Result;

namespace TAs.Application.Skills.Commands.CreateSkill
{
    public class CreateSkillCommandHandler(ISkillRepository skillRepository,
    ILogger<CreateSkillCommandHandler> logger)
    : IRequestHandler<CreateSkillCommand, Result>
    {
        public async Task<Result> Handle(CreateSkillCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Create Skill from the repository.");
            var isDuplicated = await skillRepository.IsDuplicated(request.Name, request.Path);
            if (isDuplicated) return Result.Failure(SkillErrors.SameSkill);

            var model = new Skill
            {
                Name = request.Name,
                Path = request.Path,
                Icon = request.Icon,
                Color = request.Color,
                Description = request.Description
            };
            skillRepository.Add(model);
            await skillRepository.SaveChangeAsync();
            return Result.Success();
        }
    }
}