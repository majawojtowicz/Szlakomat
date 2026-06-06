using System.Collections.Concurrent;
using Szlakomat.Parties.Domain.Model;
using Szlakomat.Parties.Domain.Relationships;

namespace Szlakomat.Parties.Infrastructure.Relationships;

internal sealed class InMemoryPartyRelationshipRepository : IPartyRelationshipRepository
{
    private readonly ConcurrentDictionary<Guid, PartyRelationship> _store = new();

    public void Save(PartyRelationship relationship) =>
        _store[relationship.Id.Value] = relationship;

    public PartyRelationship? FindById(PartyRelationshipId id) =>
        _store.TryGetValue(id.Value, out var r) ? r : null;

    public IReadOnlyList<PartyRelationship> FindFor(PartyId partyId) =>
        _store.Values
            .Where(r => r.FromPartyId.Equals(partyId) || r.ToPartyId.Equals(partyId))
            .ToList();

    public IReadOnlyList<PartyRelationship> FindActiveFor(PartyId partyId) =>
        _store.Values
            .Where(r => (r.FromPartyId.Equals(partyId) || r.ToPartyId.Equals(partyId)) && r.IsActive())
            .ToList();

    public IReadOnlyList<PartyRelationship> FindAll() => _store.Values.ToList();
}
