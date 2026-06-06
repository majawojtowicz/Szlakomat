using Szlakomat.Parties.Domain.Model;

namespace Szlakomat.Parties.Domain.Roles.Policies;

public interface IRoleAdditionPolicy
{
    bool Allows(Party party, Role role);
    string ViolationReason { get; }
}
