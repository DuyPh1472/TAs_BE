using MediatR;
using Microsoft.Extensions.Logging;
using TAs.Application.Interfaces;
using TAs.Application.Skills.HandleError;
using TAs.Application.Users;
using TAs.Domain.Entities;
using TAs.Domain.Result;

namespace TAs.Application.Skills.Commands.CreateSkill
{
    public class CreateSkillCommandHandler(IUnitOfWork unitOfWork,
    ILogger<CreateSkillCommandHandler> logger,
    IUserContext userContext)
    : IRequestHandler<CreateSkillCommand, Result>
    {
        public async Task<Result> Handle(CreateSkillCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Create Skill from the repository.");
            var user = userContext.GetCurrentUser();
            
            var isDuplicated = await unitOfWork
            .SkillRepository
            .IsDuplicated(request.Name, request.Path);

            if (isDuplicated)
                return Result.Failure(SkillErrors.SameSkill);
            var model = new Skill
            {
                Name = request.Name,
                Path = request.Path,
                Icon = request.Icon,
                Color = request.Color,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = user!.Id
            };
            unitOfWork.SkillRepository.Add(model);
            await unitOfWork.SkillRepository.SaveChangeAsync();
            return Result.Success();
        }
    }
}