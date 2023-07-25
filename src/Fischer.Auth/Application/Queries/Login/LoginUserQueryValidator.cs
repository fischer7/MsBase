using FluentValidation;

namespace Fischer.Auth.Application.Queries.Login;
internal sealed class LoginUserQueryValidator : AbstractValidator<LoginUserQueryInput>
{
    public LoginUserQueryValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("Username cannot be empty.");

        RuleFor(x => x.Password).NotEmpty().WithMessage("Password cannot be empty.");

    }
}
