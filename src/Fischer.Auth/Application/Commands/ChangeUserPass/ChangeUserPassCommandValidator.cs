using FluentValidation;

namespace Fischer.Auth.Application.Commands.ChangeUserPass;
internal sealed class ChangeUserPassCommandValidator : AbstractValidator<ChangeUserPassCommandInput>
{
    public ChangeUserPassCommandValidator()
    {
        RuleFor(c => c.NewPass)
            .NotEmpty()
            .WithMessage("New Pass can't be empty.");

        RuleFor(c => c.NewPassConfirmation)
            .NotEmpty()
            .WithMessage("Pass confirmation can't be empty.");

        RuleFor(c => c.NewPass)
            .Equal(c => c.NewPassConfirmation)
            .WithMessage("New pass confirmation should be equal to New Pass");
    }
}
