using Szlakomat.Parties.Domain.Model;

namespace Szlakomat.Parties.Domain.Relationships;

internal interface IPartyRelationshipRepository
{
    void Save(PartyRelationship relationship);
    PartyRelationship? FindById(PartyRelationshipId id);
    IReadOnlyList<PartyRelationship> FindFor(PartyId partyId);
    IReadOnlyList<PartyRelationship> FindActiveFor(PartyId partyId);
    IReadOnlyList<PartyRelationship> FindAll();
}
