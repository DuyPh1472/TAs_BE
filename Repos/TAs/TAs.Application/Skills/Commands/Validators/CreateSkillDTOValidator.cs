using FluentValidation;
using TAs.Application.Skills.Commands.CreateSkill;  
namespace TAs.Application.Skills.Commands.Validators
{
    public class CreateSkillCommandValidator : AbstractValidator<CreateSkillCommand>
    {
        public CreateSkillCommandValidator()
        {
            RuleFor(dto => dto.Name)
                            .Length(2, 100);

            RuleFor(dto => dto.Description)
                            .NotEmpty().WithMessage("Description is required.");

            RuleFor(dto => dto.Color)
                            .NotEmpty().WithMessage("Color is required.")
                            .Matches(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")
                            .WithMessage("Color must be a valid hex color code.");

            RuleFor(dto => dto.Icon)
                            .NotEmpty().WithMessage("Icon is required.");

            RuleFor(dto => dto.Path)
                            .NotEmpty().WithMessage("Path is required.")
                            .Matches(@"^[a-zA-Z0-9\-]+$")
                            .WithMessage("Path can only contain letters, numbers, and hyphens");

        }
    }
}