using Microsoft.AspNetCore.Identity;

namespace Fischer.Auth.Domain.Entities;
public class AppRoleClaim : IdentityRoleClaim<Guid>
{
    public virtual AppRole Role { get; set; } = default!;
}