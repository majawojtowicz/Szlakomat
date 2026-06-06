using Szlakomat.Parties.Domain.Model;

namespace Szlakomat.Parties.Domain.Roles.Policies;

public sealed class AllowAllRoleAdditionPolicy : IRoleAdditionPolicy
{
    public bool Allows(Party party, Role role) => true;
    public string ViolationReason => "";
}
