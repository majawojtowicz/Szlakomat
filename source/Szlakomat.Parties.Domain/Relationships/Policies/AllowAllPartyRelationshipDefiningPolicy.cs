using Szlakomat.Parties.Domain.Model;
using Szlakomat.Parties.Domain.Roles;

namespace Szlakomat.Parties.Domain.Relationships.Policies;

public sealed class AllowAllPartyRelationshipDefiningPolicy : IPartyRelationshipDefiningPolicy
{
    public bool CanDefine(Party from, Role fromRole, Party to, Role toRole, RelationshipName name) => true;
    public string ViolationReason => "";
}
