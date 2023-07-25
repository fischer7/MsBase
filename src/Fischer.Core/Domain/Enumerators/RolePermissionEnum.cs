using System.ComponentModel;

namespace Fischer.Core.Domain.Enumerators;
public enum RolePermissionEnum
{
    [Description("Administrator")]
    Administrator,
    [Description("Audit")]
    Audit,
    [Description("User")]
    User
}