using Szlakomat.Parties.Domain.Relationships;

namespace Szlakomat.Parties.Application.Relationships.Common;

public record PartyRelationshipView(
    string RelationshipId,
    string FromPartyId,
    string FromRole,
    string ToPartyId,
    string ToRole,
    string Name,
    DateOnly ValidFrom,
    DateOnly? ValidTo,
    bool IsActive);

internal static class RelationshipMapper
{
    public static PartyRelationshipView ToView(PartyRelationship r) =>
        new(r.Id.AsString(),
            r.FromPartyId.AsString(), r.FromRole.Name,
            r.ToPartyId.AsString(), r.ToRole.Name,
            r.Name.Value, r.ValidFrom, r.ValidTo, r.IsActive());
}
