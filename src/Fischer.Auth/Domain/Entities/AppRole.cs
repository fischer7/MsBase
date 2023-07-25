using Fischer.Core.Domain.Primitives;
using Microsoft.AspNetCore.Identity;

namespace Fischer.Auth.Domain.Entities;
public class AppRole : IdentityRole<Guid>, IAggregateRoot
{
    public virtual ICollection<AppUserRole>? UserRoles { get; set; }
    public virtual ICollection<AppRoleClaim>? RoleClaims { get; set; }

}
