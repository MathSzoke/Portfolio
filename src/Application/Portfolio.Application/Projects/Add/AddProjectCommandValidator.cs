using FluentValidation;

namespace Portfolio.Application.Projects.Add;

internal sealed class AddProjectCommandValidator : AbstractValidator<ProjectResponse>
{
    public AddProjectCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}