using Microsoft.AspNetCore.Identity;

namespace Fischer.Auth.Domain.Entities;

public class AppUserRole : IdentityUserRole<Guid>
{
    public virtual AppRole Role { get; set; } = default!;
    public virtual AppUser User { get; set; } = default!;
}