using Szlakomat.Parties.Domain.Model;

namespace Szlakomat.Parties.Domain.Roles.Policies;

public sealed class NoAttractionOwnerForPersonPolicy : IRoleAdditionPolicy
{
    public bool Allows(Party party, Role role)
    {
        Guard.IsNotNull(party);
        Guard.IsNotNull(role);
        return !(role == Role.AttractionOwner && party is Person);
    }
    public string ViolationReason =>
        "ATTRACTION_OWNER cannot be assigned to Person – register as JDG or Organisation instead";
}
