using Szlakomat.Parties.Domain.Model;
using Szlakomat.Parties.Domain.Roles;

namespace Szlakomat.Parties.Domain.Relationships;

public record PartyRelationship(
    PartyRelationshipId Id,
    PartyId FromPartyId, Role FromRole,
    PartyId ToPartyId,   Role ToRole,
    RelationshipName Name,
    DateOnly ValidFrom,
    DateOnly? ValidTo)
{
    public static PartyRelationship Of(PartyId from, Role fromRole, PartyId to, Role toRole, RelationshipName name)
    {
        Guard.IsNotNull(from);
        Guard.IsNotNull(to);
        Guard.IsNotNull(name);
        Guard.IsFalse(from.Equals(to), "A party cannot have a relationship with itself");
        return new(PartyRelationshipId.Random(), from, fromRole, to, toRole, name,
            DateOnly.FromDateTime(DateTime.Today), null);
    }

    public bool IsActive()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        return today >= ValidFrom && (ValidTo is null || today <= ValidTo.Value);
    }

    public PartyRelationship Terminate(DateOnly date) => this with { ValidTo = date };

    public override string ToString() =>
        $"{FromPartyId.AsString()}[{FromRole.Name}] --[{Name.Value}]--> {ToPartyId.AsString()}[{ToRole.Name}]";
}
