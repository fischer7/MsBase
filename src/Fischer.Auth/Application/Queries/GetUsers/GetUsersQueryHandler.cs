using Fischer.Auth.Domain.Entities;
using Fischer.Auth.Persistence.Repositories.Interfaces;
using Fischer.Core.Application.Abstractions;
using Fischer.Core.Constants;
using Fischer.Core.Domain.Shared;

namespace Fischer.Auth.Application.Queries.GetUsers;

public class GetUsersQueryHandler : IQueryHandler<GetUsersQueryInput, PaginatedQueryResult<GetUsersQueryResult>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<Result<PaginatedQueryResult<GetUsersQueryResult>>> Handle(GetUsersQueryInput request, CancellationToken cancellationToken)
    {
        var users = _userRepository.GetAll();
        if (!users.Any())
            return Result.Failure<PaginatedQueryResult<GetUsersQueryResult>>(ErrorConstants.NullValue);
        
        var usersSet = await PaginatedQueryResult<AppUser>.CreateAsync(users, request.PageNumber, request.PageSize);
        return new PaginatedQueryResult<GetUsersQueryResult>(
            usersSet.Items.Select(x =>
                new GetUsersQueryResult()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email
                }).ToList(),
            usersSet.TotalCount,
            usersSet.PageNumber,
            request.PageSize);
    }

}
