using Fischer.Core.Domain.Primitives;
using Microsoft.AspNetCore.Identity;

namespace Fischer.Auth.Domain.Entities;
public class AppUser : IdentityUser<Guid>, IAggregateRoot
{
    internal AppUser(string name)
    {
        Name = name;
        ShouldAlterPass = true;
    }

    public string Name { get; private set; }
    public string? CellPhone { get; private set; }
    public bool ShouldAlterPass { get; private set; }

    public virtual ICollection<AppUserRole>? UserRoles { get; set; }
    public void SetAlteredPassword() => ShouldAlterPass = false;

}
