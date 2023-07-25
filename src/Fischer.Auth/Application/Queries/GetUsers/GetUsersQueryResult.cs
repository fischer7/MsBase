namespace Fischer.Auth.Application.Queries.GetUsers;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public sealed record GetUsersQueryResult
{
    public Guid Id { get; internal set; }
    public string Name { get; internal set; }
    public string? Email { get; internal set; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.