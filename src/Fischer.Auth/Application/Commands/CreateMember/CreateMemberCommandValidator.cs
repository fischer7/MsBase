using FluentValidation;

namespace Fischer.Auth.Application.Commands.CreateMember;
internal sealed class CreateMemberCommandValidator : AbstractValidator<CreateMemberCommandInput>
{
    public CreateMemberCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty();

        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(10);

        RuleFor(x => x.LastName).NotEmpty().MaximumLength(15);
    }
}
