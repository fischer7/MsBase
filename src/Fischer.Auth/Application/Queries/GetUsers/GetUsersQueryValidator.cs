using FluentValidation;

namespace Fischer.Auth.Application.Queries.GetUsers;
internal sealed class GetUsersQueryValidator : AbstractValidator<GetUsersQueryInput>
{
    public GetUsersQueryValidator()
    {

    }
}
