using System.ComponentModel;

namespace Fischer.Core.Domain.Enumerators;
public enum PermissionActionEnum
{
    [Description("C")]
    Create,
    [Description("R")]
    Read,
    [Description("U")]
    Update,
    [Description("D")]
    Delete,
    [Description("S")]
    Special
}