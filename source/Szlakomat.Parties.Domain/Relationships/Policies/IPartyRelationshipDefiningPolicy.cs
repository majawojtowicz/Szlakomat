using Szlakomat.Parties.Domain.Model;
using Szlakomat.Parties.Domain.Roles;

namespace Szlakomat.Parties.Domain.Relationships.Policies;

public interface IPartyRelationshipDefiningPolicy
{
    bool CanDefine(Party from, Role fromRole, Party to, Role toRole, RelationshipName name);
    string ViolationReason { get; }
}
